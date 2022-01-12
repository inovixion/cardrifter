using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIDEBANNERSETTINGS : MonoBehaviour
{
    void OnEnable()
    {
        //if(AdmobBannerAd.Instance)
        //{
        //    AdmobBannerAd.Instance.HideBanner();
        //}
        /* if (FacebookAdsDaim.Instance)
         {
             FacebookAdsDaim.Instance.DisposeBanner();
         }*/
        admob.instance.HideBanner();
    }

    void OnDisable()
    {
        //if (AdmobBannerAd.Instance)
        //{
        //    AdmobBannerAd.Instance.ShowBanner();
        //}
        /* if (FacebookAdsDaim.Instance)
         {
             FacebookAdsDaim.Instance.LoadBanner();
         }*/
        admob.instance.RequestBanner();
    }
}
