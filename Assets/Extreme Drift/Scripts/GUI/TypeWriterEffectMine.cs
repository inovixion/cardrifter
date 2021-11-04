using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TypeWriterEffectMine : MonoBehaviour
{

    public float delay = 0.1f;
    private string fullText;
    private string currentText = "";
    public GameObject[] Objective;

    public AudioSource TypingSoundAudioSource;

    // Use this for initialization
    void Awake()
    {
        int y = SceneManager.GetActiveScene().buildIndex;
        switch(y)
        {
            case 1:
                fullText = "Get "+ PlayerPrefs.GetInt("track1") + " Drift Coins to complete this Mission.";
                break;
            case 2:
                fullText = "Get " + PlayerPrefs.GetInt("track2") + " Drift Coins to complete this Mission.";
                break;
            case 3:
                fullText = "Get " + PlayerPrefs.GetInt("track3") + " Drift Coins to complete this Mission.";
                break;
            case 4:
                fullText = "Get " + PlayerPrefs.GetInt("track4") + " Drift Coins to complete this Mission.";
                break;
            case 5:
                fullText = "Get " + PlayerPrefs.GetInt("track5") + " Drift Coins to complete this Mission.";
                break;
            case 6:
                fullText = "Get " + PlayerPrefs.GetInt("track6") + " Drift Coins to complete this Mission.";
                break;
            default:
                break;
        }
    }
    void Start()
    {
        StartCoroutine(ShowText());
        //StartCoroutine(wait());
    }

    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(1f);
        Objective[5].SetActive(false);
        TypingSoundAudioSource.Play();
        for (int i = 0; i <= fullText.Length; i++)
        {
            if(i==fullText.Length-1)
            {
                Debug.Log(fullText.Length);
                TypingSoundAudioSource.Stop();
            }
           
            currentText = fullText.Substring(0, i);
            this.GetComponent<Text>().text = currentText;
            yield return new WaitForSeconds(delay);
            
        }
        

    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.45f);
        Time.timeScale = 0;
    }

    public void OK()
    {
        Objective[0].SetActive(false);
        Objective[5].SetActive(true);
        Objective[1].SetActive(true);
        Objective[2].SetActive(true);
        Objective[3].SetActive(true);
        Objective[4].GetComponent<VehicleCameraDaim>().enabled = true;
    }
}