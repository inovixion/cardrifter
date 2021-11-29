using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public static InGameUI Instance;
    public Text[] namesTxt;
    //public Image thirdPlaceImg;
    public string a, b, c;
    public Text pos;

    void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        namesTxt[0].text = a;
        namesTxt[1].text = b;
        namesTxt[2].text = c;

        if(a == "Car-1(Clone)" || a == "Car-2(Clone)" || a == "Car-3(Clone)" || a == "Car-4(Clone)" || a == "Car-5(Clone)")
        {
            pos.text = "Position: " + "1" + "/3";
        }
        else if(b == "Car-1(Clone)" || b == "Car-2(Clone)" || b == "Car-3(Clone)" || b == "Car-4(Clone)" || b == "Car-5(Clone)")
        {
            pos.text = "Position: " + "2" + "/3";
        }
        else if(c == "Car-1(Clone)" || c == "Car-2(Clone)" || c == "Car-3(Clone)" || c == "Car-4(Clone)" || c == "Car-5(Clone)")
        {
            pos.text = "Position: " + "3" + "/3";
        }
    }
}
