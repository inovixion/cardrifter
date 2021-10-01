using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour {
	public void RateUs()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.inovixion.madcar.driverpro");
    }

    public void MoreGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/dev?id=8451194537616848614");
    }

    public void PrivacyPolicy()
    {
        Application.OpenURL("https://syedali1305911.blogspot.com/2021/09/privacy-policy-for-mad-drift.html");
    }

}
