using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelFinish : MonoBehaviour
{
    public Text[] rankTxt;
 
    void Update()
    {
        rankTxt[0].text = GameManager.instance.firstPlace;
        rankTxt[1].text = GameManager.instance.secondPlace;
        rankTxt[2].text = GameManager.instance.thirdPlace;



    }
}
