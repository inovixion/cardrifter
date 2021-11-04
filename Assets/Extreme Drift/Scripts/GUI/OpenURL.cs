using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour {
	public void RateUs()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.gamington.driftzy&hl=en&gl=US");
    }

    public void MoreGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.wincryptocash.bitcoin.earning&hl=en&gl=US");
    }

    public void PrivacyPolicy()
    {
        Application.OpenURL("https://syedali1305911.blogspot.com/2021/10/privacy-policy-for-driftzy-car-drift.html");
    }

}
