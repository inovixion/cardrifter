using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITrigger : MonoBehaviour
{
    public static AITrigger Instance;
    public bool TriggerAI = false;
    void Awake()
    {
        Instance = this;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "AIhere")
        {
            TriggerAI = true;
        }
        else if(other.gameObject.tag == "Player")
        {
            this.gameObject.SetActive(false);
        }
    }
}
