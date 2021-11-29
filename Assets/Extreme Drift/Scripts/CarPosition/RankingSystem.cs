using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingSystem : MonoBehaviour
{
    public int currentCheckPoint, lapCount;

    public float distance;
    private Vector3 checkPoint;

    public float counter;
    public int rank;
   //public GameObject panelcar;
   // public GameObject losepanel;
    
    void Start()
    {
        currentCheckPoint = 1;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateDistance();
    }
    void CalculateDistance()
    {
        distance = Vector3.Distance(transform.position, checkPoint);
        counter = lapCount * 1000 + currentCheckPoint * 100 + distance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CheckPoint")
        {
            currentCheckPoint = other.GetComponent<CurrentCheckPoint>().currentCheckPointNumber;
            checkPoint = GameObject.Find("CheckPoint" + currentCheckPoint).transform.position;
        }
        if(other.tag == "Finish")
        {
            if(gameObject.name == "Car") {
                //panelcar.SetActive(true);
            }
            if (gameObject.name == "yellowcar" || gameObject.name == "RedCar")
            {
                //losepanel.SetActive(true);
            }
        }
           
         
       
    }
}
