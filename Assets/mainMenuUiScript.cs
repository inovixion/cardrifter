using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainMenuUiScript : MonoBehaviour
{
    [Space]
    [Header("Main Menu Button Images")]
    
    public GameObject image1;
    public GameObject image2;
    public GameObject image3;
    public GameObject image4;

    [Space]
    [Header("Settings Quality Texts")]
    
    public Text text1Qua;
    public Text text2Qua;
    public Text text3Qua;

   /* [Space]
    [Header("Vehicle Garage")]

    public GameObject VehicleGarage;*/
   /* public GameObject Garage;
    public GameObject VehicleRoot;*/


    public void Update()
    {
        /*if(VehicleGarage.gameObject.activeInHierarchy)
        {
            Garage.GetComponent<Transform>().position = new Vector3(-0.88f, 0.009f, -2.72f);
            VehicleRoot.GetComponent<Transform>().position = new Vector3(-0.88f, 0f, -2.72f);
        }
        else
        {
            Garage.GetComponent<Transform>().position = new Vector3(-0.59f, 0.009f, -0.34f);
            VehicleRoot.GetComponent<Transform>().position = new Vector3(0, 0, 0);
        }*/
    }


    public void myStoreBtn()
    {
        image1.SetActive(true);
        image2.SetActive(false);
        image3.SetActive(false);
        image4.SetActive(false);
    }
    public void rateUsBtn()
    {
        image1.SetActive(false);
        image2.SetActive(true);
        image3.SetActive(false);
        image4.SetActive(false);
    }
    public void removeAdsBtn()
    {
        image1.SetActive(false);
        image2.SetActive(false);
        image3.SetActive(true);
        image4.SetActive(false);
    }
    public void moreGamesBtn()
    {
        image1.SetActive(false);
        image2.SetActive(false);
        image3.SetActive(false);
        image4.SetActive(true);
    }
    public void lowQua()
    {
        text1Qua.GetComponent<Text>().color = new Color32(0, 0, 0,255);
        text2Qua.GetComponent<Text>().color = new Color32(255, 255, 255,255);
        text3Qua.GetComponent<Text>().color = new Color32(255, 255, 255,255);
    }
    public void medQua()
    {
        text1Qua.GetComponent<Text>().color = new Color32(255, 255, 255,255);
        text2Qua.GetComponent<Text>().color = new Color32(0, 0, 0,255);
        text3Qua.GetComponent<Text>().color = new Color32(255, 255, 255,255);
    }
    public void higQua()
    {
        text1Qua.GetComponent<Text>().color = new Color32(255, 255, 255,255);
        text2Qua.GetComponent<Text>().color = new Color32(255, 255, 255,255);
        text3Qua.GetComponent<Text>().color = new Color32(0, 0, 0,255);
    }
}
