using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    public GameObject inGameUI, leaderboard;

    private Button nextLevel;
 
    private void Awake()
    {
        instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.failed)
        {
            if (leaderboard.activeInHierarchy)
            {
                GameManager.instance.failed = false;
               
            }
        }
    }


    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Quit()
    {
       SceneManager.LoadScene(0);
    }
}
