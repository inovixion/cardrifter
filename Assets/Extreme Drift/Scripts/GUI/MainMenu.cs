using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum Panels { MainMenu = 0, SelectVehicle = 1, SelectLevel = 2, Settings = 3, InAppPanel = 4 }

public class MainMenu : MonoBehaviour
{
    private static MainMenu _instance;
    public static MainMenu Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MainMenu>();
            }
            return _instance;
        }
    }

    private int gameScore { get; set; }

    public float cameraRotateSpeed = 5;
    public Animator FadeBackGround;

    public AudioSource menuMusic;
    public Transform vehicleRoot;
    public Material[] allRestMaterials;

    public MenuPanels menuPanels;
    public EXITBACKBTNS EXITBACK;
    public MenuGUI menuGUI;
    public VehicleSetting[] vehicleSetting;
    public LevelSetting[] levelSetting;


    [System.Serializable]
    public class MenuGUI
    {
        public Text GameScore;
        public Text VehicleName;
        public Text VehicleStatus;
        public Text VehiclePrice;

        public Slider VehicleSpeed;
        public Text VehicleTYRES;
        public Slider VehicleBraking;
        public Text VehicleBRAKING;
        public Slider VehicleNitro;
        public Text VehicleHANDLING;

        public Slider sensitivity;

        public Toggle audio;
        public Toggle music;
        public Toggle vibrateToggle;
        public Toggle ButtonMode, AccelMode;

        public Image wheelColor, smokeColor, bodycolor;
        public Image loadingBar;
        public Text loadingtext;

        public GameObject lOCK;
        public GameObject loading;
        public GameObject customizeVehicle;
        public GameObject buyNewVehicle;
        public GameObject[] QualityHover;
        public GameObject[] LevelHover;
    }

    [System.Serializable]
    public class MenuPanels
    {
        public GameObject MainMenu;
        public GameObject SelectVehicle;
        public GameObject SelectLevel;
        public GameObject EnoughMoney;
        public GameObject Settings;
        public GameObject InAppPanel;
    }

    [System.Serializable]
    public class EXITBACKBTNS
    {
        public GameObject EXIT;
        public GameObject cUSTOMIZEBACK;
        public GameObject VEHICLEBACK;
        public GameObject LevelsBACK;
    }

    [System.Serializable]
    public class VehicleSetting
    {
        public string name = "Vehicle 1";

        public int price = 20000;

        public GameObject vehicle;
        public GameObject wheelSmokes;

        public Material ringMat, smokeMat, bodymat;
        public Transform[] rearWheels;

        public VehiclePower vehiclePower;

        [HideInInspector]
        public bool Bought = false;

        [System.Serializable]
        public class VehiclePower
        {
            public float speed = 80;
            public float braking = 1000;
            public float nitro = 10;
        }
        public Texture[] CARSTEXTURE;
    }

    [System.Serializable]
    public class LevelSetting
    {
        public bool locked = true;
        public Button panel;
        public Text bestTime;
        public GameObject lockImage;
        public StarClass stars;

        [System.Serializable]
        public class StarClass
        {
            public Image Star1, Star2, Star3;
        }
    }

    private Panels activePanel = Panels.MainMenu;

    private bool vertical, horizontal;
    private Vector2 startPos;
    private Vector2 touchDeltaPosition;
    private float x, y = 0;

    private VehicleSetting currentVehicle;

    private int currentVehicleNumber = 0;
    private int currentLevelNumber = 0;

    private Color mainColor;
    private int mainColorInt = 0;
    private bool randomColorActive = false;
    private bool startingGame = false;

    private float menuLoadTime = 0.0f;
    private AsyncOperation sceneLoadingOperation = null;
    public bool ischecked;
    public bool ischecked1;
    public GameObject zoonbuttonOn;
    public GameObject zoombuttonoff;
    public GameObject UICanvas;
    public Camera mainCam;


    //ControlMode//////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void ControlModeButtons(Toggle value)
    {
        menuGUI.AccelMode.isOn = false;
        if (value.isOn)
            PlayerPrefs.SetString("ControlMode", "Buttons");
    }
    public void ControlModeAccel(Toggle value)
    {
        menuGUI.ButtonMode.isOn = false;
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

    //Vehcile Color//////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public void ActiveCurrentColor(Image activeImage)
    {

        mainColor = activeImage.color;
        if (activeImage.name == "Blue")
        {
            mainColorInt = 0;
            print(mainColorInt);
        }
        else if (activeImage.name == "Green")
        {
            mainColorInt = 1;
        }
        else if (activeImage.name == "Orange")
        {
            mainColorInt = 2;
        }
        else if (activeImage.name == "Red")
        {
            mainColorInt = 3;
        }
        else if (activeImage.name == "Yellow")
        {
            mainColorInt = 4;
        }

        if (menuGUI.wheelColor.gameObject.activeSelf)
        {
            vehicleSetting[currentVehicleNumber].ringMat.SetColor("_Color", mainColor);
            PlayerPrefsX.SetColor("VehicleWheelsColor" + currentVehicleNumber, mainColor);
        }
        else if (menuGUI.smokeColor.gameObject.activeSelf)
        {
            vehicleSetting[currentVehicleNumber].smokeMat.SetColor("_TintColor", new Color(mainColor.r, mainColor.g, mainColor.b, 0.2f));
            PlayerPrefsX.SetColor("VehicleSmokeColor" + currentVehicleNumber, new Color(mainColor.r, mainColor.g, mainColor.b, 0.2f));
        }
        else if (menuGUI.bodycolor.gameObject.activeSelf)
        {
            vehicleSetting[currentVehicleNumber].bodymat.mainTexture = vehicleSetting[currentVehicleNumber].CARSTEXTURE[mainColorInt];
            //PlayerPrefsX.SetColor("VehiclebodyColor" + currentVehicleNumber, mainColor);
        }
    }


    public void ActiveWheelColor(Image activeImage)
    {
        randomColorActive = false;

        activeImage.gameObject.SetActive(true);
        menuGUI.wheelColor = activeImage;
        menuGUI.smokeColor.gameObject.SetActive(false);
        menuGUI.bodycolor.gameObject.SetActive(false);
    }


    public void ActiveSmokeColor(Image activeImage)
    {
        randomColorActive = false;

        activeImage.gameObject.SetActive(true);
        menuGUI.smokeColor = activeImage;
        menuGUI.wheelColor.gameObject.SetActive(false);
        menuGUI.bodycolor.gameObject.SetActive(false);
    }

    public void ActivebodyColor(Image activeImage)
    {
        randomColorActive = false;

        activeImage.gameObject.SetActive(true);
        menuGUI.bodycolor = activeImage;
        menuGUI.wheelColor.gameObject.SetActive(false);
        menuGUI.smokeColor.gameObject.SetActive(false);
    }


    public void OutCustomizeVehicle()
    {
        randomColorActive = false;
        menuGUI.wheelColor.gameObject.SetActive(false);
        menuGUI.smokeColor.gameObject.SetActive(false);
    }


    public void RandomColor()
    {

        randomColorActive = true;

        menuGUI.wheelColor.gameObject.SetActive(false);
        menuGUI.smokeColor.gameObject.SetActive(false);
        menuGUI.bodycolor.gameObject.SetActive(false);

        int INDEX;
        INDEX = Random.Range(0, vehicleSetting[currentVehicleNumber].CARSTEXTURE.Length);

        vehicleSetting[currentVehicleNumber].ringMat.SetColor("_Color", new Color(Random.Range(0.0f, 1.1f), Random.Range(0.0f, 1.1f), Random.Range(0.0f, 1.1f)));
        vehicleSetting[currentVehicleNumber].smokeMat.SetColor("_TintColor", new Color(Random.Range(0.0f, 1.1f), Random.Range(0.0f, 1.1f), Random.Range(0.0f, 1.1f), 0.2f));
        vehicleSetting[currentVehicleNumber].bodymat.mainTexture = vehicleSetting[currentVehicleNumber].CARSTEXTURE[INDEX];

        PlayerPrefsX.SetColor("VehicleWheelsColor" + currentVehicleNumber, vehicleSetting[currentVehicleNumber].ringMat.GetColor("_Color"));
        PlayerPrefsX.SetColor("VehicleSmokeColor" + currentVehicleNumber, vehicleSetting[currentVehicleNumber].smokeMat.GetColor("_TintColor"));
    }


    //Share//////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public void SettingActive(bool activePanel)
    {
        menuPanels.Settings.gameObject.SetActive(activePanel);
    }

    public void ClickExitButton()
    {
        Application.Quit();
    }

    public void BACK()
    {
        SceneManager.LoadScene(0);
    }
    //GamePanels//////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void CurrentPanel(int current)
    {

        activePanel = (Panels)current;

        if (currentVehicleNumber != PlayerPrefs.GetInt("CurrentVehicle"))
        {
            currentVehicleNumber = PlayerPrefs.GetInt("CurrentVehicle");

            foreach (VehicleSetting VSetting in vehicleSetting)
            {

                if (VSetting == vehicleSetting[currentVehicleNumber])
                {
                    VSetting.vehicle.SetActive(true);
                    currentVehicle = VSetting;
                }
                else
                {
                    VSetting.vehicle.SetActive(false);
                }
            }
        }

        switch (activePanel)
        {

            case Panels.MainMenu:
                menuPanels.MainMenu.SetActive(true);
                menuPanels.SelectVehicle.SetActive(false);
                menuPanels.SelectLevel.SetActive(false);
                EXITBACK.EXIT.SetActive(true);
                EXITBACK.cUSTOMIZEBACK.SetActive(false);
                EXITBACK.VEHICLEBACK.SetActive(false);
                EXITBACK.LevelsBACK.SetActive(false);
                if (menuGUI.wheelColor) menuGUI.wheelColor.gameObject.SetActive(true);

                break;
            case Panels.SelectVehicle:
                menuPanels.MainMenu.gameObject.SetActive(false);
                menuPanels.SelectVehicle.SetActive(true);
                menuPanels.SelectLevel.SetActive(false);
                EXITBACK.EXIT.SetActive(false);
                EXITBACK.cUSTOMIZEBACK.SetActive(false);
                EXITBACK.VEHICLEBACK.SetActive(true);
                EXITBACK.LevelsBACK.SetActive(false);
                break;
            case Panels.SelectLevel:
                menuPanels.MainMenu.SetActive(false);
                menuPanels.SelectVehicle.SetActive(false);
                menuPanels.SelectLevel.SetActive(true);
                EXITBACK.EXIT.SetActive(false);
                EXITBACK.cUSTOMIZEBACK.SetActive(false);
                EXITBACK.VEHICLEBACK.SetActive(false);
                EXITBACK.LevelsBACK.SetActive(true);
                break;
            case Panels.Settings:
                menuPanels.MainMenu.SetActive(false);
                menuPanels.SelectVehicle.SetActive(false);
                menuPanels.SelectLevel.SetActive(false);
                break;
            case Panels.InAppPanel:
                menuPanels.MainMenu.SetActive(false);
                menuPanels.SelectVehicle.SetActive(false);
                menuPanels.SelectLevel.SetActive(false);
                menuPanels.InAppPanel.SetActive(true);
                break;
        }
    }


    //Vehicles Switch//////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void BuyVehicle()
    {
        if ((gameScore >= vehicleSetting[currentVehicleNumber].price) && !vehicleSetting[currentVehicleNumber].Bought)
        {
            PlayerPrefs.SetInt("BoughtVehicle" + currentVehicleNumber.ToString(), 1);
            gameScore -= vehicleSetting[currentVehicleNumber].price;
            if (gameScore <= 0) { gameScore = 1; }
            PlayerPrefs.SetInt("GameScore", gameScore);
            vehicleSetting[currentVehicleNumber].Bought = true;
        }
        else
        {
            menuPanels.EnoughMoney.SetActive(true);
        }
    }

    public void UnlockAllCars()
    {
        for(int i = 0; i < vehicleSetting.Length; i++)
        {
            PlayerPrefs.SetInt("BoughtVehicle" + i.ToString(), 1);
            vehicleSetting[i].Bought = true;
        }
        
    }



    public void NextVehicle()
    {
        if (menuGUI.wheelColor) { menuGUI.wheelColor.gameObject.SetActive(false); }

        currentVehicleNumber++;
        currentVehicleNumber = (int)Mathf.Repeat(currentVehicleNumber, vehicleSetting.Length);

        foreach (VehicleSetting VSetting in vehicleSetting)
        {

            if (VSetting == vehicleSetting[currentVehicleNumber])
            {
                VSetting.vehicle.SetActive(true);
                currentVehicle = VSetting;
            }
            else
            {
                VSetting.vehicle.SetActive(false);

            }
        }
    }


    public void PreviousVehicle()
    {
        if (menuGUI.wheelColor) { menuGUI.wheelColor.gameObject.SetActive(false); }

        currentVehicleNumber--;
        currentVehicleNumber = (int)Mathf.Repeat(currentVehicleNumber, vehicleSetting.Length);

        foreach (VehicleSetting VSetting in vehicleSetting)
        {
            if (VSetting == vehicleSetting[currentVehicleNumber])
            {
                VSetting.vehicle.SetActive(true);
                currentVehicle = VSetting;
            }
            else
            {
                VSetting.vehicle.SetActive(false);
            }
        }
    }

    //GameSettings//////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void QualitySetting(int quality)
    {
        QualitySettings.SetQualityLevel(quality - 1, true);
        PlayerPrefs.SetInt("QualitySettings", quality);
        for (int i = 0; i < menuGUI.QualityHover.Length; i++)
        {
            menuGUI.QualityHover[i].SetActive(false);
        }
        menuGUI.QualityHover[quality - 1].SetActive(true);
    }

    public void EditSensitivity()
    {
        PlayerPrefs.SetFloat("Sensitivity", menuGUI.sensitivity.value);
    }

    public void DisableAudioButton(Toggle toggle)
    {
        if (toggle.isOn)
        {
            AudioListener.volume = 1;
            PlayerPrefs.SetInt("AudioActive", 0);
        }
        else
        {
            AudioListener.volume = 0;
            PlayerPrefs.SetInt("AudioActive", 1);
        }
    }


    public void DisableMusicButton(Toggle toggle)
    {
        if (toggle.isOn)
        {
            menuMusic.GetComponent<AudioSource>().mute = false;
            PlayerPrefs.SetInt("MusicActive", 0);
        }
        else
        {
            menuMusic.GetComponent<AudioSource>().mute = true;
            PlayerPrefs.SetInt("MusicActive", 1);
        }
    }


    public void EraseSave()
    {
        PlayerPrefs.DeleteAll();
        currentVehicleNumber = 0;
        Application.LoadLevel(0);

        vehicleSetting[0].bodymat.mainTexture = vehicleSetting[0].CARSTEXTURE[0];
        vehicleSetting[1].bodymat.mainTexture = vehicleSetting[1].CARSTEXTURE[4];
        vehicleSetting[2].bodymat.mainTexture = vehicleSetting[2].CARSTEXTURE[2];
        vehicleSetting[3].bodymat.mainTexture = vehicleSetting[3].CARSTEXTURE[3];
        vehicleSetting[4].bodymat.mainTexture = vehicleSetting[4].CARSTEXTURE[1];

        foreach (Material mat in allRestMaterials)
            mat.SetColor("_Color", new Color(0.7f, 0.7f, 0.7f));
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void StartGame()
    {
        if (startingGame) return;
        FadeBackGround.SetBool("FadeOut", true);
        StartCoroutine(LoadLevelGame(1.5f));
        startingGame = true;
    }


    IEnumerator LoadLevelGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        menuGUI.loading.SetActive(true);
        StartCoroutine(LoadLevelAsync());

    }

    IEnumerator LoadLevelAsync()
    {

        yield return new WaitForSeconds(0.4f);

        sceneLoadingOperation = Application.LoadLevelAsync(currentLevelNumber + 1);
        sceneLoadingOperation.allowSceneActivation = false;

        while (!sceneLoadingOperation.isDone || sceneLoadingOperation.progress < 0.9f)
        {
            menuLoadTime += Time.deltaTime;

            yield return 0;
        }
    }

    int levelvalue;
    public GameObject buyConfirmPanel;
    public void UnlockLevels(int curret)
    {
        levelvalue = curret;
        buyConfirmPanel.SetActive(true);
    }

    public void LevelUnlockYes()
    {
        if (levelSetting[levelvalue].locked == true)
        {
            if (gameScore >= 250)
            {
                levelSetting[levelvalue].panel.image.color = new Color(0.36f, 0.36f, 0.36f);
                levelSetting[levelvalue].panel.enabled = true;
                levelSetting[levelvalue].lockImage.gameObject.SetActive(false);
                levelSetting[levelvalue].locked = false;
                PlayerPrefs.SetInt("UnlockedLevels" + levelvalue.ToString(), PlayerPrefs.GetInt("UnlockedLevels") + 1);
                gameScore = gameScore - 250;
                PlayerPrefs.SetInt("GameScore", gameScore);
                buyConfirmPanel.SetActive(false);
            }
        }
        else
        {
            menuPanels.EnoughMoney.SetActive(true);
        }
    }

    public void UnlockAllTracks()
    {
        for(int i = 0; i < levelSetting.Length; i++)
        {
            levelSetting[i].panel.image.color = new Color(0.36f, 0.36f, 0.36f);
            levelSetting[i].panel.enabled = true;
            levelSetting[i].lockImage.gameObject.SetActive(false);
            levelSetting[i].locked = false;
            PlayerPrefs.SetInt("UnlockedLevels" + i.ToString(), PlayerPrefs.GetInt("UnlockedLevels") + 1);
        }
    }

    public void LevelUnlockNo()
    {
        buyConfirmPanel.SetActive(false);
    }


    public void currentLevel(int current)
    {

        for (int i = 0; i < levelSetting.Length; i++)
        {
            if (i == current)
            {
                currentLevelNumber = current;
                levelSetting[i].panel.image.color = Color.white;
                levelSetting[i].panel.enabled = true;
                levelSetting[i].lockImage.gameObject.SetActive(false);
                PlayerPrefs.SetInt("CurrentLevelNumber", currentLevelNumber);
            }
            else if (levelSetting[i].locked == false)
            {
                levelSetting[i].panel.image.color = new Color(0.36f, 0.36f, 0.36f);
                levelSetting[i].panel.enabled = true;
                levelSetting[i].lockImage.gameObject.SetActive(false);
            }
            else
            {
                levelSetting[i].panel.image.color = Color.white;
                levelSetting[i].panel.enabled = false;
                levelSetting[i].lockImage.gameObject.SetActive(true);
            }

            if (levelSetting[i].bestTime)
            {
                if (PlayerPrefs.GetFloat("BestTime" + (i + 1).ToString()) != 0.0f)
                {
                    //if (PlayerPrefs.GetInt("LevelStar" + (i + 1)) == 1)
                    //{
                    //    levelSetting[i].stars.Star1.color = Color.white;
                    //}
                    //else if (PlayerPrefs.GetInt("LevelStar" + (i + 1)) == 2)
                    //{
                    //    levelSetting[i].stars.Star1.color = Color.white;
                    //    levelSetting[i].stars.Star2.color = Color.white;
                    //}
                    //else if (PlayerPrefs.GetInt("LevelStar" + (i + 1)) == 3)
                    //{
                    //    levelSetting[i].stars.Star1.color = Color.white;
                    //    levelSetting[i].stars.Star2.color = Color.white;
                    //    levelSetting[i].stars.Star3.color = Color.white;
                    //}
                    levelSetting[i].stars.Star1.color = Color.white;
                    levelSetting[i].stars.Star2.color = Color.white;
                    levelSetting[i].stars.Star3.color = Color.white;
                    levelSetting[i].bestTime.text = "BEST : " + GetComponent<FormatSecondsScript>().FormatSeconds(PlayerPrefs.GetFloat("BestTime" + (i + 1))).ToString();
                }
            }
        }

        for (int j = 0; j < menuGUI.LevelHover.Length; j++)
        {
            menuGUI.LevelHover[j].SetActive(false);
        }
        menuGUI.LevelHover[current].SetActive(true);
    }



    public GameObject PRIVACYPANEL;


    public void ACCEPT()
    {
        PRIVACYPANEL.SetActive(false);
        menuPanels.MainMenu.SetActive(true);
        PlayerPrefs.SetInt("PRIVACY", 1);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Awake()
    {
        if(!PlayerPrefs.HasKey("UnlockedLevels"))
        {
            PlayerPrefs.SetInt("UnlockedLevels", 0);
        }


        AudioListener.pause = false;
        Time.timeScale = 1.0f;


        menuGUI.vibrateToggle.isOn = (PlayerPrefs.GetInt("VibrationActive") == 0) ? true : false;


        gameScore = 100;
        if(PlayerPrefs.GetInt("PRIVACY") == 1)
        {
            CurrentPanel(0);
        }
        else
        {
            PRIVACYPANEL.SetActive(true);
        }
        

        if (!PlayerPrefs.HasKey("track1") || !PlayerPrefs.HasKey("track2") || !PlayerPrefs.HasKey("track3") || !PlayerPrefs.HasKey("track4") || !PlayerPrefs.HasKey("track5"))
        {
            PlayerPrefs.SetInt("track1", 50);
            PlayerPrefs.SetInt("track2", 50);
            PlayerPrefs.SetInt("track3", 50);
            PlayerPrefs.SetInt("track4", 50);
            PlayerPrefs.SetInt("track5", 50);
        }

        if (PlayerPrefs.GetInt("QualitySettings") == 0)
        {
            PlayerPrefs.SetInt("QualitySettings", 3);
            QualitySettings.SetQualityLevel(3, true);
        }
        else
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualitySettings") - 1, true);
        }
        for (int j = 0; j < menuGUI.QualityHover.Length; j++)
        {
            menuGUI.QualityHover[j].SetActive(false);
        }
        menuGUI.QualityHover[PlayerPrefs.GetInt("QualitySettings") - 1].SetActive(true);

        if (PlayerPrefs.GetFloat("Sensitivity") == 0.0f)
        {
            menuGUI.sensitivity.value = 1.0f;
            PlayerPrefs.SetFloat("Sensitivity", 1.0f);
        }
        else
        {
            menuGUI.sensitivity.value = PlayerPrefs.GetFloat("Sensitivity");
        }


        switch (PlayerPrefs.GetString("ControlMode"))
        {
            case "":
                menuGUI.ButtonMode.isOn = true;
                menuGUI.AccelMode.isOn = false;
                break;
            case "Buttons":
                menuGUI.ButtonMode.isOn = true;
                menuGUI.AccelMode.isOn = false;
                break;
            case "Accel":
                menuGUI.AccelMode.isOn = true;
                menuGUI.ButtonMode.isOn = false;
                break;
        }


        currentLevelNumber = PlayerPrefs.GetInt("CurrentLevelNumber");


        for (int lvls = 0; lvls < levelSetting.Length; lvls++)
        {
            if (lvls <= PlayerPrefs.GetInt("CurrentLevelUnlocked"))
                levelSetting[lvls].locked = false;

        }

            print(PlayerPrefs.GetInt("UnlockedLevels"));
        int a = 0;
        foreach (LevelSetting LS in levelSetting)
        {
            print(PlayerPrefs.GetInt("UnlockedLevels") + a);
            if(PlayerPrefs.GetInt("UnlockedLevels" + a.ToString()) == 1)
            {
                levelSetting[a].locked = false;
                levelSetting[a].lockImage.SetActive(false);
            }
            a++;
        }


        currentLevel(currentLevelNumber);


        switch (PlayerPrefs.GetString("ControlMode"))
        {
            case "":
                PlayerPrefs.SetString("ControlMode", "Buttons");
                menuGUI.ButtonMode.isOn = true;
                break;
            case "Buttons":
                menuGUI.ButtonMode.isOn = true;
                break;
            case "Accel":
                menuGUI.AccelMode.isOn = true;
                break;
        }


        PlayerPrefs.SetInt("BoughtVehicle0", 1);


        //audio and music Toggle
        menuGUI.audio.isOn = (PlayerPrefs.GetInt("AudioActive") == 0) ? true : false;
        AudioListener.volume = (PlayerPrefs.GetInt("AudioActive") == 0) ? 1.0f : 0.0f;

        menuGUI.music.isOn = (PlayerPrefs.GetInt("MusicActive") == 0) ? true : false;
        menuMusic.mute = (PlayerPrefs.GetInt("MusicActive") == 0) ? false : true;

        currentVehicleNumber = PlayerPrefs.GetInt("CurrentVehicle");
        currentVehicle = vehicleSetting[currentVehicleNumber];


        int i = 0;

        foreach (VehicleSetting VSetting in vehicleSetting)
        {

            if (PlayerPrefsX.GetColor("VehicleWheelsColor" + i) == Color.clear)
            {
                vehicleSetting[i].ringMat.SetColor("_DiffuseColor", Color.white);
            }
            else
            {
                vehicleSetting[i].ringMat.SetColor("_DiffuseColor", PlayerPrefsX.GetColor("VehicleWheelsColor" + i));
            }



            if (PlayerPrefsX.GetColor("VehicleSmokeColor" + i) == Color.clear)
            {
                vehicleSetting[i].smokeMat.SetColor("_TintColor", new Color(0.8f, 0.8f, 0.8f, 0.2f));
            }
            else
            {
                vehicleSetting[i].smokeMat.SetColor("_TintColor", PlayerPrefsX.GetColor("VehicleSmokeColor" + i));
            }



            if (PlayerPrefs.GetInt("BoughtVehicle" + i.ToString()) == 1)
            {
                VSetting.Bought = true;

                if (PlayerPrefs.GetInt("GameScore") == 0)
                {
                    PlayerPrefs.SetInt("GameScore", gameScore);
                }
                else
                {
                    gameScore = PlayerPrefs.GetInt("GameScore");
                }
            }


            if (VSetting == vehicleSetting[currentVehicleNumber])
            {
                VSetting.vehicle.SetActive(true);
                currentVehicle = VSetting;
            }
            else
            {
                VSetting.vehicle.SetActive(false);
            }

            i++;
        }
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    void Update()
    {

        

        if (sceneLoadingOperation != null)
        {
            menuGUI.loadingBar.fillAmount = Mathf.MoveTowards(menuGUI.loadingBar.fillAmount, sceneLoadingOperation.progress + 0.2f, Time.deltaTime * 0.5f);
            float loadingtextfill = menuGUI.loadingBar.fillAmount * 100;
            int INTERGER = (int)loadingtextfill;
            menuGUI.loadingtext.text = INTERGER.ToString() + "%";
            if (menuGUI.loadingBar.fillAmount > sceneLoadingOperation.progress)
                sceneLoadingOperation.allowSceneActivation = true;
        }


        if (menuGUI.smokeColor.gameObject.activeSelf || randomColorActive)
        {
            vehicleSetting[currentVehicleNumber].rearWheels[0].Rotate(1000 * Time.deltaTime, 0, 0);
            vehicleSetting[currentVehicleNumber].rearWheels[1].Rotate(1000 * Time.deltaTime, 0, 0);
            vehicleSetting[currentVehicleNumber].wheelSmokes.SetActive(true);
        }
        else
        {
            vehicleSetting[currentVehicleNumber].wheelSmokes.SetActive(false);
        }



        menuGUI.VehicleSpeed.value = vehicleSetting[currentVehicleNumber].vehiclePower.speed / 100.0f;
        menuGUI.VehicleTYRES.text = vehicleSetting[currentVehicleNumber].vehiclePower.speed.ToString() + "%";
        menuGUI.VehicleBRAKING.text = vehicleSetting[currentVehicleNumber].vehiclePower.braking.ToString() + "%";
        menuGUI.VehicleHANDLING.text = vehicleSetting[currentVehicleNumber].vehiclePower.nitro.ToString() + "%";
        menuGUI.VehicleBraking.value = vehicleSetting[currentVehicleNumber].vehiclePower.braking / 100.0f;
        menuGUI.VehicleNitro.value = vehicleSetting[currentVehicleNumber].vehiclePower.nitro / 100.0f;
        menuGUI.GameScore.text = gameScore.ToString();


        if (vehicleSetting[currentVehicleNumber].Bought)
        {
            menuGUI.customizeVehicle.SetActive(true);
            menuGUI.buyNewVehicle.SetActive(false);
            menuGUI.lOCK.SetActive(false);

            menuGUI.VehicleName.text = vehicleSetting[currentVehicleNumber].name;
            menuGUI.VehicleStatus.text = "STATUS: BOUGHT";
            menuGUI.VehiclePrice.text = "BOUGHT";
            PlayerPrefs.SetInt("CurrentVehicle", currentVehicleNumber);
        }
        else
        {
            menuGUI.customizeVehicle.SetActive(false);
            menuGUI.buyNewVehicle.SetActive(true);
            menuGUI.lOCK.SetActive(true);

            menuGUI.VehicleName.text = vehicleSetting[currentVehicleNumber].name;
            menuGUI.VehicleStatus.text = "STATUS: LOCKED";
            menuGUI.VehiclePrice.text = "COST: " + vehicleSetting[currentVehicleNumber].price.ToString();
        }



#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
        if(zoonbuttonOn.activeSelf)
        {
            if (Input.GetMouseButton(0) && activePanel != Panels.SelectLevel)
            {
                x = Mathf.Lerp(x, Mathf.Clamp(Input.GetAxis("Mouse X"), -2, 2) * cameraRotateSpeed, Time.deltaTime * 5.0f);
                Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 50, 60);
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 50, Time.deltaTime);
            }
            else
            {
                x = Mathf.Lerp(x, cameraRotateSpeed * 0.01f, Time.deltaTime * 5.0f);
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime);
            }
        }
        else
        {
            if (ischecked == true)
            {
                mainCam.fieldOfView = mainCam.fieldOfView - 10 * Time.deltaTime;
                if (mainCam.fieldOfView <= 40)
                {
                    ischecked = false;
                }
            }
            if (ischecked1 == true)
            {
                mainCam.fieldOfView = mainCam.fieldOfView + 10 * Time.deltaTime;
                if (mainCam.fieldOfView >= 50)
                {
                    ischecked1 = false;
                }
            }
        }
        


#elif UNITY_ANDROID||UNITY_IOS



        if (Input.touchCount == 1&& activePanel!=Panels.SelectLevel)
        {
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Moved:
                    x = Mathf.Lerp(x, Mathf.Clamp(Input.GetTouch(0).deltaPosition.x, -2, 2) * cameraRotateSpeed, Time.deltaTime*3.0f);
                    Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 50, 60);
                    Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 50, Time.deltaTime);
                    break;
            }

        }
        else {
            x = Mathf.Lerp(x, cameraRotateSpeed * 0.02f, Time.deltaTime*3.0f);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime);
        }

#endif

        transform.RotateAround(vehicleRoot.position, Vector3.up, x);


    }


    public void InterstitialadGeneric()
    {
        if(AdmobInterstitialAd.Instance)
        {
            AdmobInterstitialAd.Instance.showinter();
        }
    }


    public void zoomedCamOn()
    {
        UICanvas.gameObject.SetActive(false);
        ischecked = true;
        StartCoroutine(zoombuttonOnfunc());

    }
    public void zoomedCamOFF()
    {
        ischecked1 = true;
        StartCoroutine(zoombuttonOFFfunc());


    }
    IEnumerator zoombuttonOFFfunc()
    {
        yield return new WaitForSeconds(1.5f);
        zoonbuttonOn.SetActive(true);
        zoombuttonoff.SetActive(false);
        UICanvas.gameObject.SetActive(true);
    }
    IEnumerator zoombuttonOnfunc()
    {
        yield return new WaitForSeconds(1.5f);
        zoombuttonoff.SetActive(true);
        zoonbuttonOn.SetActive(false);
    }
}
