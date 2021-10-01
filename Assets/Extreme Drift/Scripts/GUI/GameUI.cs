using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GameUI : MonoBehaviour
{

    public static GameUI manage;
    
    public Panels panels;
    public CarUI carUI;
    public Sounds sounds;

    [HideInInspector]
    public int totalDriftCoins;
    [HideInInspector]
    public int earnCoins, totalEarnCoins;
    [HideInInspector]
    public bool canDrift;
    [HideInInspector]
    public int driftAmount = 0;
    [HideInInspector]
    public float penaltyTime = 0.0f;
    [HideInInspector]
    public bool gameStarted, gamePaused, gameRest, gameFinished = false, gameFailed = false;
    [HideInInspector]
    public int racePrize, totalPrize = 0;

    private int currentLevelNumber = 0;

    private float menuLoadTime = 0.0f;
    private AsyncOperation sceneLoadingOperation = null;

    private AIVehicle AIVehicleScript;
    private float timerDrift = 1.0f;

    [HideInInspector]
   public bool carPenalty=false;
    [HideInInspector]
    public bool carWrongWay = false;
    [HideInInspector]
    public bool carBrakeWarning = false;

    private int gearst = 0;
    private float thisAngle = -150;

    private float startTimer = 1.0f;
    private int startCont = 3;

    [System.Serializable]
    public class Panels
    {

        public GameStart gameStart;
        public GamePlay gamePlay;
        public GamePuased gamePuased;
        public GameFinish gameFinish;
        public GameFinish gameFailed;
        public GameSettings Settings;

        [System.Serializable]
        public class GameStart
        {
            public GameObject root;
            public Text startTimeUI;

            public GameObject loading;
            public Image loadingBar;
            public Text loadingtext;

            public Animator FadeBackGround;
        }

        [System.Serializable]
        public class GamePlay
        {
            public GameObject root;
            public GameObject buttonsUI, accelUI;
            public Image wrongWay;
            public Image driftWheel;
            public Image brakeWarning;
            public Text currentTime, bestTime;
            public Text driftCoins;
            public Text driftText;
            public Text driftXText;
            public Text penaltyText;

        }
        [System.Serializable]
        public class GamePuased
        {
            public GameObject root;
            public Toggle audioToggle;
            public Toggle musicToggle;
        }
        [System.Serializable]
        public class GameFinish
        {
            public GameObject root;
            public Text yourTime, penaltyTime, bestTime;
            public Text racePrize, driftPrize;
            public Text totalPrize;
            public StarClass stars;
        }

        [System.Serializable]
        public class StarClass
        {
            public float Star1Time, Star2Time, Star3Time;
            public Image Star1, Star2, Star3;
        }
        [System.Serializable]
        public class GameSettings
        {
            public GameObject root;
        }
    }

    [System.Serializable]
    public class CarUI
    {
        public Image tachometerNeedle;
        public Image barShiftGUI;

        public Text speedText;
        public Text GearText;
    }


    [System.Serializable]
    public class Sounds
    {
        public AudioSource music;
        public AudioClip countDown, countStart;
        public GameObject[] QualityHover;
        public GameObject[] StartFinish;
    }


    public Slider Sensitivity;
    public Toggle ButtonMode, AccelMode;
    public Toggle VibrateToggle;

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Awake()
    {


        manage = this;
        AudioListener.pause = false;
        Time.timeScale = 1.0f;

        if (PlayerPrefs.GetFloat("BestTime" + Application.loadedLevel.ToString()) != 0.0f)
            panels.gamePlay.bestTime.text = "Best: " + FormatSeconds(PlayerPrefs.GetFloat("BestTime" + Application.loadedLevel.ToString()));

        panels.gamePuased.audioToggle.isOn = (PlayerPrefs.GetInt("AudioActive") == 0) ? true : false;
        panels.gamePuased.musicToggle.isOn = (PlayerPrefs.GetInt("MusicActive") == 0) ? true : false;

        AudioListener.volume = (PlayerPrefs.GetInt("AudioActive") == 0) ? 1.0f : 0.0f;
        sounds.music.mute = (PlayerPrefs.GetInt("MusicActive") == 0) ? false : true;


        if (PlayerPrefs.GetFloat("Sensitivity") == 0.0f)
        {
            Sensitivity.value = 1.0f;
            PlayerPrefs.SetFloat("Sensitivity", 1.0f);
        }
        else
        {
            Sensitivity.value = PlayerPrefs.GetFloat("Sensitivity");
        }
        switch (PlayerPrefs.GetString("ControlMode"))
        {
            case "":
                ButtonMode.isOn = true;
                AccelMode.isOn = false;
                break;
            case "Buttons":
                ButtonMode.isOn = true;
                AccelMode.isOn = false;
                break;
            case "Accel":
                AccelMode.isOn = true;
                ButtonMode.isOn = false;
                break;
        }


        for (int j = 0; j < sounds.QualityHover.Length; j++)
        {
            sounds.QualityHover[j].SetActive(false);
        }
        sounds.QualityHover[PlayerPrefs.GetInt("QualitySettings") - 2].SetActive(true);

    }


    void Start()
    {
        if (AIControl.manage.controlMode == ControlMode.Mobile)
        {
            print("dada");
            switch (PlayerPrefs.GetString("ControlMode"))
            {
                case "":
                    panels.gamePlay.buttonsUI.SetActive(true);
                    panels.gamePlay.accelUI.SetActive(false);
                    break;
                case "Buttons":
                    panels.gamePlay.buttonsUI.SetActive(true);
                    panels.gamePlay.accelUI.SetActive(false);
                    break;
                case "Accel":
                    panels.gamePlay.accelUI.SetActive(true);
                    panels.gamePlay.buttonsUI.SetActive(false);
                    break;
            }
        }
        StartCoroutine(StartToFinishtxt());
    }

    IEnumerator StartToFinishtxt()
    {
        yield return new WaitForSeconds(10f);
        sounds.StartFinish[0].SetActive(false);
        sounds.StartFinish[1].SetActive(true);
    }

    void Update()
    {

        StartingGameTimer();

        ShowCarUI();

        if (sceneLoadingOperation != null)
        {
            panels.gameStart.loadingBar.fillAmount = Mathf.MoveTowards(panels.gameStart.loadingBar.fillAmount, sceneLoadingOperation.progress + 0.2f, Time.deltaTime * 0.5f);

            float loadingtextfill = panels.gameStart.loadingBar.fillAmount * 100;
            int INTERGER = (int)loadingtextfill;
            panels.gameStart.loadingtext.text = INTERGER.ToString() + "%";

            if (panels.gameStart.loadingBar.fillAmount > sceneLoadingOperation.progress)
                sceneLoadingOperation.allowSceneActivation = true;
        }

        if (carPenalty) { penaltyTime += Time.deltaTime; panels.gamePlay.penaltyText.color = Color.red; } else { panels.gamePlay.penaltyText.color = Color.white; }

        if (carWrongWay) { panels.gamePlay.wrongWay.gameObject.SetActive(true); } else { panels.gamePlay.wrongWay.gameObject.SetActive(false); }

        if (carBrakeWarning) { panels.gamePlay.brakeWarning.color = new Color(1, 1, 1, 1); } else { panels.gamePlay.brakeWarning.color = new Color(1, 1, 1, 0.2f); }


        panels.gamePlay.currentTime.text = "Time: " + FormatSeconds(AIControl.CurrentVehicle.AIVehicle.playerCurrentTime);
        panels.gamePlay.bestTime.text = "Best: " + FormatSeconds(PlayerPrefs.GetFloat("BestTime" + Application.loadedLevel));
        panels.gamePlay.penaltyText.text = "Penalty: " + FormatSeconds(penaltyTime).ToString();

        panels.gamePlay.driftCoins.text = "Drift Coins: " + totalDriftCoins.ToString();


        if (!gameFinished && !carWrongWay && !gameFailed)
        {
            if (timerDrift == 0)
                canDrift = true;
            else
                timerDrift = Mathf.MoveTowards(timerDrift, 0.0f, Time.deltaTime);

            if ((driftAmount / 100.0f) > 0.2f)
            {
                timerDrift = 1.0f;
                earnCoins = (driftAmount - 60);

                panels.gamePlay.driftWheel.fillAmount = 1.0f;
                panels.gamePlay.driftWheel.rectTransform.Rotate(0, 0, -500.0f * Time.deltaTime);
                panels.gamePlay.driftText.text = (driftAmount - 20).ToString();

                panels.gamePlay.driftXText.gameObject.SetActive(true);


                if (earnCoins > 100)
                {
                    if (panels.gamePlay.driftXText.text == "2X")
                    {
                        panels.gamePlay.driftXText.GetComponent<Animator>().Play(0);
                        panels.gamePlay.driftXText.text = "3X";
                    }
                    totalEarnCoins = earnCoins*3;
                }
                else if (earnCoins > 50)
                {
                    if (panels.gamePlay.driftXText.text == "1X")
                    {
                        panels.gamePlay.driftXText.GetComponent<Animator>().Play(0);
                        panels.gamePlay.driftXText.text = "2X";
                    }
                    totalEarnCoins = earnCoins*2;

                }
                else if (earnCoins > 0)
                {
                    panels.gamePlay.driftXText.text = "1X";
                    totalEarnCoins = earnCoins;
                }
            }
            else if (canDrift)
            {
                if (panels.gamePlay.driftWheel.fillAmount == 1)
                {
                    totalDriftCoins += totalEarnCoins;
                    totalEarnCoins = 0;
                    earnCoins = 0;
                }

                panels.gamePlay.driftWheel.fillAmount = driftAmount / 100.0f;
                panels.gamePlay.driftWheel.rectTransform.rotation = Quaternion.identity;
                panels.gamePlay.driftText.text = "";
                panels.gamePlay.driftXText.gameObject.SetActive(false);
            }
        }
        else
        {
            driftAmount = 0;
            panels.gamePlay.driftWheel.fillAmount = 0.0f;
            panels.gamePlay.driftWheel.rectTransform.rotation = Quaternion.identity;
            panels.gamePlay.driftText.text = "";
            panels.gamePlay.driftXText.gameObject.SetActive(false);

            if (gameFinished)
            { 
                if (panels.gameFinish.root.activeSelf == false)
                {

                    AIVehicleScript = AIControl.CurrentVehicle.AIVehicle;

                    panels.gameFinish.yourTime.text = "Current: " + FormatSeconds(AIVehicleScript.playerBestTime);
                    panels.gameFinish.penaltyTime.text = "Penalty: " + FormatSeconds(penaltyTime);

                    if (AIVehicleScript.playerBestTime < PlayerPrefs.GetFloat("BestTime" + Application.loadedLevel) || PlayerPrefs.GetFloat("BestTime" + Application.loadedLevel) == 0)
                    {
                        PlayerPrefs.SetFloat("BestTime" + Application.loadedLevel, AIVehicleScript.playerBestTime);
                        panels.gameFinish.bestTime.text = "Best: " + FormatSeconds(AIVehicleScript.playerBestTime);
                    }
                    else
                    {
                        panels.gameFinish.bestTime.text = "Best: " + FormatSeconds(PlayerPrefs.GetFloat("BestTime" + Application.loadedLevel));
                    }

                    if (AIVehicleScript.playerBestTime < (panels.gameFinish.stars.Star3Time - penaltyTime))
                    {

                        //panels.gameFinish.stars.Star1.color = Color.white;
                        //panels.gameFinish.stars.Star2.color = Color.white;
                        //panels.gameFinish.stars.Star3.color = Color.white;

                        racePrize = (int)((Application.loadedLevel / 5.0f) * 3000.0f);
                       // PlayerPrefs.SetInt("LevelStar" + Application.loadedLevel, 3);

                    }
                    else if (AIVehicleScript.playerBestTime < (panels.gameFinish.stars.Star2Time - penaltyTime))
                    {

                        //panels.gameFinish.stars.Star1.color = Color.white;
                       // panels.gameFinish.stars.Star2.color = Color.white;

                        racePrize = (int)((Application.loadedLevel / 5.0f) * 2000.0f);
                        //PlayerPrefs.SetInt("LevelStar" + Application.loadedLevel, 2);

                    }
                    else if (AIVehicleScript.playerBestTime < (panels.gameFinish.stars.Star1Time - penaltyTime))
                    {

                        //panels.gameFinish.stars.Star1.color = Color.white;

                        racePrize = (int)((Application.loadedLevel / 5.0f) * 1500.0f);
                        //PlayerPrefs.SetInt("LevelStar" + Application.loadedLevel, 1);
                    }
                    else
                    {
                        //PlayerPrefs.SetInt("LevelStar" + Application.loadedLevel, 0);
                        racePrize = (int)((Application.loadedLevel / 5.0f) * 1000.0f);
                    }

                    if(PlayerPrefs.GetInt("CashDoubler") == 1)
                    {
                        totalPrize = (totalDriftCoins + racePrize);
                        totalPrize = totalPrize * 2;
                    }
                    else
                    {
                        totalPrize = (totalDriftCoins + racePrize);
                    }
                    

                    panels.gameFinish.racePrize.text = "Track: " + racePrize.ToString();
                    panels.gameFinish.driftPrize.text = "Drift Coins: " + totalDriftCoins.ToString();
                    panels.gameFinish.totalPrize.text = "Total: " + totalPrize.ToString();

                    panels.gamePlay.root.gameObject.SetActive(false);
                    panels.gamePuased.root.gameObject.SetActive(false);
                    panels.gameFinish.root.gameObject.SetActive(true);

                    PlayerPrefs.SetInt("GameScore", PlayerPrefs.GetInt("GameScore") + totalPrize);

                    if (PlayerPrefs.GetInt("CurrentLevelUnlocked") <= Application.loadedLevel)
                        PlayerPrefs.SetInt("CurrentLevelUnlocked", Application.loadedLevel);


                    InterstitialadGeneric();
                    HideBannerAd();

                }
            }

            if (gameFailed)
            {
                if (panels.gameFailed.root.activeSelf == false)
                {

                    AIVehicleScript = AIControl.CurrentVehicle.AIVehicle;

                    panels.gameFailed.yourTime.text = "Current: " + FormatSeconds(AIVehicleScript.playerBestTime);
                    panels.gameFailed.penaltyTime.text = "Penalty: " + FormatSeconds(penaltyTime);

                    if (AIVehicleScript.playerBestTime < PlayerPrefs.GetFloat("BestTime" + Application.loadedLevel) || PlayerPrefs.GetFloat("BestTime" + Application.loadedLevel) == 0)
                    {
                        PlayerPrefs.SetFloat("BestTime" + Application.loadedLevel, AIVehicleScript.playerBestTime);
                        panels.gameFailed.bestTime.text = "Best: " + FormatSeconds(AIVehicleScript.playerBestTime);
                    }
                    else
                    {
                        panels.gameFailed.bestTime.text = "Best: " + FormatSeconds(PlayerPrefs.GetFloat("BestTime" + Application.loadedLevel));
                    }

                    //if (AIVehicleScript.playerBestTime < (panels.gameFinish.stars.Star3Time - penaltyTime))
                    //{

                    //    panels.gameFinish.stars.Star1.color = Color.white;
                    //    panels.gameFinish.stars.Star2.color = Color.white;
                    //    panels.gameFinish.stars.Star3.color = Color.white;

                    //    racePrize = (int)((Application.loadedLevel / 5.0f) * 3000.0f);
                    //    PlayerPrefs.SetInt("LevelStar" + Application.loadedLevel, 3);

                    //}
                    //else if (AIVehicleScript.playerBestTime < (panels.gameFinish.stars.Star2Time - penaltyTime))
                    //{

                    //    panels.gameFinish.stars.Star1.color = Color.white;
                    //    panels.gameFinish.stars.Star2.color = Color.white;

                    //    racePrize = (int)((Application.loadedLevel / 5.0f) * 2000.0f);
                    //    PlayerPrefs.SetInt("LevelStar" + Application.loadedLevel, 2);

                    //}
                    //else if (AIVehicleScript.playerBestTime < (panels.gameFinish.stars.Star1Time - penaltyTime))
                    //{

                    //    panels.gameFinish.stars.Star1.color = Color.white;

                    //    racePrize = (int)((Application.loadedLevel / 5.0f) * 1500.0f);
                    //    PlayerPrefs.SetInt("LevelStar" + Application.loadedLevel, 1);
                    //}
                    //else
                    //{
                    //    PlayerPrefs.SetInt("LevelStar" + Application.loadedLevel, 0);
                    //    racePrize = (int)((Application.loadedLevel / 5.0f) * 1000.0f);
                    //}

                    totalPrize = (totalDriftCoins + racePrize);

                    panels.gameFailed.racePrize.text = "Track: 0";
                    panels.gameFailed.driftPrize.text = "Drift Coins: 0";
                    panels.gameFailed.totalPrize.text = "Total: 0";

                    panels.gamePlay.root.gameObject.SetActive(false);
                    panels.gamePuased.root.gameObject.SetActive(false);
                    panels.gameFailed.root.gameObject.SetActive(true);

                    PlayerPrefs.SetInt("GameScore", PlayerPrefs.GetInt("GameScore") + 0);

                    //if (PlayerPrefs.GetInt("CurrentLevelUnlocked") <= Application.loadedLevel)
                    //    PlayerPrefs.SetInt("CurrentLevelUnlocked", Application.loadedLevel);


                    InterstitialadGeneric();
                    HideBannerAd();
                }
            }
        }
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void DisableAudio(Toggle toggle)
    {
        if (toggle.isOn)
        {
            PlayerPrefs.SetInt("AudioActive", 0);
        }
        else
        {
            AudioListener.volume = 0;
            PlayerPrefs.SetInt("AudioActive", 1);
        }
    }


    public void DisableMusic(Toggle toggle)
    {
        if (toggle.isOn)
        {
            sounds.music.mute = false;
            PlayerPrefs.SetInt("MusicActive", 0);
        }
        else
        {
            sounds.music.mute = true;
            PlayerPrefs.SetInt("MusicActive", 1);
        }
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public void PauseGame()
    {

        if (gameRest) return;

        gamePaused = true;
        AudioListener.pause = true;
        panels.gamePuased.root.gameObject.SetActive(true);
        panels.gameStart.root.SetActive(false);
        panels.gamePlay.root.SetActive(false);
        InterstitialadGeneric();
        Time.timeScale = 0.0f;
    }


    public void ResumeGame()
    {

        if (gameRest) return;

        gamePaused = false;
        AudioListener.pause = false;
        AudioListener.volume = (PlayerPrefs.GetInt("AudioActive") == 0) ? 1.0f : 0.0f;
        VibrateToggle.isOn = (PlayerPrefs.GetInt("VibrationActive") == 0) ? true : false;
        panels.gamePuased.root.gameObject.SetActive(false);
        panels.gameStart.root.SetActive(true);
        panels.gamePlay.root.SetActive(true);
        Time.timeScale = 1.0f;
        if (PlayerPrefs.GetFloat("Sensitivity") == 0.0f)
        {
            Sensitivity.value = 1.0f;
            PlayerPrefs.SetFloat("Sensitivity", 1.0f);
        }
        else
        {
            Sensitivity.value = PlayerPrefs.GetFloat("Sensitivity");
        }
        if (AIControl.manage.controlMode == ControlMode.Mobile)
        {
            switch (PlayerPrefs.GetString("ControlMode"))
            {
                case "":
                    panels.gamePlay.buttonsUI.SetActive(true);
                    panels.gamePlay.accelUI.SetActive(false);
                    break;
                case "Buttons":
                    panels.gamePlay.buttonsUI.SetActive(true);
                    panels.gamePlay.accelUI.SetActive(false);
                    break;
                case "Accel":
                    panels.gamePlay.accelUI.SetActive(true);
                    panels.gamePlay.buttonsUI.SetActive(false);
                    break;
            }
        }
    }

    public void RestartGame()
    {
        if (gameRest) return;
        if (AdmobInterstitialAd.Instance)
        {
            AdmobInterstitialAd.Instance.showinter();
        }
        Time.timeScale = 1.0f;
        panels.gameStart.FadeBackGround.SetBool("FadeOut", true);
        StartCoroutine(LoadLevelGame(1.5f));
        currentLevelNumber = Application.loadedLevel;
        gameRest = true;
    }


    public void MainMenu()
    {
        if (gameRest) return;
        if (AdmobInterstitialAd.Instance)
        {
            AdmobInterstitialAd.Instance.showinter();
        }
        Time.timeScale = 1.0f;
        panels.gameStart.FadeBackGround.SetBool("FadeOut", true);
        currentLevelNumber = 0;
        StartCoroutine(LoadLevelGame(1.5f));
        gameRest = true;
    }


        IEnumerator LoadLevelGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        panels.gameStart.loading.SetActive(true);
        StartCoroutine(LoadLevelAsync());
    }

    IEnumerator LoadLevelAsync()
    {

        yield return new WaitForSeconds(0.4f);

        sceneLoadingOperation = Application.LoadLevelAsync(currentLevelNumber);
        sceneLoadingOperation.allowSceneActivation = false;

        while (!sceneLoadingOperation.isDone || sceneLoadingOperation.progress < 0.9f)
        {
            menuLoadTime += Time.deltaTime;
            yield return 0;
        }
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void ShowCarUI()
    {


        gearst = AIControl.CurrentVehicle.currentGear;
        carUI.speedText.text = ((int)AIControl.CurrentVehicle.speed).ToString();

        if (gearst > 0 && AIControl.CurrentVehicle.speed > 1)
        {
            carUI.GearText.color = Color.green;
            carUI.GearText.text = gearst.ToString();
        }
        else if (AIControl.CurrentVehicle.speed > 1)
        {
            carUI.GearText.color = Color.red;
            carUI.GearText.text = "R";
        }
        else
        {
            carUI.GearText.color = Color.white;
            carUI.GearText.text = "N";
        }

        thisAngle = (AIControl.CurrentVehicle.motorRPM / 20) - 175;
        thisAngle = Mathf.Clamp(thisAngle, -180, 90);

        carUI.tachometerNeedle.rectTransform.rotation = Quaternion.Euler(0, 0, -thisAngle);
        carUI.barShiftGUI.fillAmount = AIControl.CurrentVehicle.powerShift / 100.0f;

    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void StartingGameTimer()
    {

        if (gameStarted && startTimer == 0) { panels.gameStart.startTimeUI.gameObject.SetActive(false); return; }

        startTimer = Mathf.MoveTowards(startTimer, 0.0f, Time.deltaTime);

        if (startTimer == 0 && !gameStarted)
        {

            startTimer = 1.0f;
            panels.gameStart.startTimeUI.fontSize = 200;

            if (startCont < 0)
            {
                panels.gameStart.startTimeUI.text = "";
            }
            else if (startCont == 0)
            {
                gameStarted = true;

                panels.gameStart.startTimeUI.GetComponent<AudioSource>().clip = sounds.countStart;
                panels.gameStart.startTimeUI.GetComponent<AudioSource>().Play();
                panels.gameStart.startTimeUI.text = startCont.ToString("START");
            }
            else if (startCont > 0)
            {
                panels.gameStart.startTimeUI.GetComponent<AudioSource>().clip= sounds.countDown;
                panels.gameStart.startTimeUI.GetComponent<AudioSource>().Play();
                panels.gameStart.startTimeUI.text = startCont.ToString("F0");

            }
            startCont--;
        }
        else
        {
            panels.gameStart.startTimeUI.fontSize = (int)Mathf.Lerp(panels.gameStart.startTimeUI.fontSize, 1.0f, Time.deltaTime * 2.0f);
        }
    }


    string FormatSeconds(float elapsed)
    {
        int d = (int)(elapsed * 100.0f);
        int minutes = d / (60 * 100);
        int seconds = (d % (60 * 100)) / 100;
        int hundredths = d % 100;
        return String.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, hundredths);
    }


    public void ControlModeButtons(Toggle value)
    {
        AccelMode.isOn = false;
        if (value.isOn)
            PlayerPrefs.SetString("ControlMode", "Buttons");
    }
    public void ControlModeAccel(Toggle value)
    {
        ButtonMode.isOn = false;
        if (value.isOn)
            PlayerPrefs.SetString("ControlMode", "Accel");
    }


    public void DisableVibration(Toggle toggle)
    {
        if (toggle.isOn)
            PlayerPrefs.SetInt("VibrationActive", 0);
        else
            PlayerPrefs.SetInt("VibrationActive", 1);
    }
    public void EditSensitivity()
    {
        PlayerPrefs.SetFloat("Sensitivity", Sensitivity.value);
    }

    //public void DisableAudioButton(Toggle toggle)
    //{
    //    if (toggle.isOn)
    //    {
    //        AudioListener.volume = 1;
    //        PlayerPrefs.SetInt("AudioActive", 0);
    //    }
    //    else
    //    {
    //        AudioListener.volume = 0;
    //        PlayerPrefs.SetInt("AudioActive", 1);
    //    }
    //}

    public void QualitySetting(int quality)
    {
        QualitySettings.SetQualityLevel(quality - 1, true);
        PlayerPrefs.SetInt("QualitySettings", quality);
        for (int i = 0; i < sounds.QualityHover.Length; i++)
        {
            sounds.QualityHover[i].SetActive(false);
        }
        sounds.QualityHover[quality - 1].SetActive(true);
    }

    public void SettingPanel(string status)
    {
        if(status == "settings")
        {
            panels.Settings.root.SetActive(true);
        }
        else
        {
            panels.Settings.root.SetActive(false);
        }
        
    }

    public void InterstitialadGeneric()
    {
        if (AdmobInterstitialAd.Instance)
        {
            AdmobInterstitialAd.Instance.showinter();
        }
    }

    public void HideBannerAd()
    {
        if(AdmobBannerAd.Instance)
        {
            AdmobBannerAd.Instance.HideBanner();
        }
    }

}