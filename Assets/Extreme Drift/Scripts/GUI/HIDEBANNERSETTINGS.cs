using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIDEBANNERSETTINGS : MonoBehaviour
{
    void OnEnable()
    {
        if(AdmobBannerAd.Instance)
        {
            AdmobBannerAd.Instance.HideBanner();
        }
    }

    void OnDisable()
    {
        if (AdmobBannerAd.Instance)
        {
            AdmobBannerAd.Instance.ShowBanner();
        }
    }
}
