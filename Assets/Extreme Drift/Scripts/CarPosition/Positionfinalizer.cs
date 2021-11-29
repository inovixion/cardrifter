using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Positionfinalizer : MonoBehaviour
{
    public Text FinalPosition1;
    public Text FinalPosition2;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (InGameUI.Instance.pos.text == "Position: 1/3")
            {
                FinalPosition1.text = "1st";
                FinalPosition2.text = "1st";
            }
            else if (InGameUI.Instance.pos.text == "Position: 2/3")
            {
                FinalPosition1.text = "2nd";
                FinalPosition2.text = "2nd";
            }
            else if (InGameUI.Instance.pos.text == "Position: 3/3")
            {
                FinalPosition1.text = "3rd";
                FinalPosition2.text = "3rd";
            }
            Destroy(this.gameObject);
        }
    }
}
