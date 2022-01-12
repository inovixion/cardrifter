using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using GoogleMobileAds.Api;

public class admob : MonoBehaviour
{
    public static admob instance;

    private string appId = "ca-app-pub-8313137749676161~7805263989";

    private BannerView bannerView;
    private string bannerId = "ca-app-pub-3940256099942544/6300978111";

    private InterstitialAd fullScreenAd;
    private string fullScreenAdId= "ca-app-pub-3940256099942544/1033173712";


   // private string UnityId= "2885080";

    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        MobileAds.Initialize(appId);
        requestFullScreenAd();
        //InitializeUnityAds();
        
    }

    public void requestBannerAd()
    {
        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Top);
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
        bannerView.Show();
    }


    public void HideBanner()
    {
        bannerView.Hide();
    }

    public void requestFullScreenAd()
    {
        fullScreenAd = new InterstitialAd(fullScreenAdId);
        AdRequest request = new AdRequest.Builder().Build();
        fullScreenAd.LoadAd(request);
    }

    public void showFullScreenAd()
    {
        if (fullScreenAd.IsLoaded())
        {
            fullScreenAd.Show();
        }
        else
        {
            Debug.Log("Full Screen Ad Not Loaded");
        }
        requestFullScreenAd();
    }

    public void RequestBanner()
    {
        requestBannerAd();
    }

    public void ShowAdmobInterstitial()
    {
        showFullScreenAd();
    }


/*
    #region Unity Ads Handler


    public void InitializeUnityAds()
    {
        

        if (!Advertisement.isInitialized)
            Advertisement.Initialize(UnityId);

    }

    public bool IsUnityRewardedVideoReady()
    {
        if (Advertisement.IsReady("rewardedVideo"))
            return true;
        else
            return false;
    }


    public void UnityVideo()
    {
        if (!PlayerPrefs.HasKey("DoNotShowAds"))
        {
            if (Advertisement.IsReady())
                Advertisement.Show();

        }
    }

    public void ShowUnityRewardedvideo()
    {

        if (Advertisement.IsReady("rewardedVideo"))
        {
           ShowOptions options = new ShowOptions();
           options.resultCallback = HandleShowResult;
            Advertisement.Show("rewardedVideo", options);

        }

    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case UnityEngine.Advertisements.ShowResult.Finished:
                		Debug.Log ("Video completed. User rewarded " + " credits.");
                break;
            case UnityEngine.Advertisements.ShowResult.Skipped:

                break;
            case UnityEngine.Advertisements.ShowResult.Failed:

                break;
        }
    }

    #endregion*/





}
