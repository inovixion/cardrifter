/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AudienceNetwork;
using AudienceNetwork.Utility;


public class FacebookAdsDaim : MonoBehaviour
{
    public bool IsTest = true;
    public static FacebookAdsDaim Instance;
    private InterstitialAd interstitialAd;
    private bool isLoaded;
    public Text text;
    private AdView adView;
    private ScreenOrientation currentScreenOrientation;
    int y;
    void Awake()
    {
        Instance = this;
        y = SceneManager.GetActiveScene().buildIndex;
    }

    public void DisposeBanner()
    {
        if(PlayerPrefs.GetInt("RemoveAD") == 0)
        {
            if (this.adView)
            {
                this.adView.Dispose();
            }
        }
        
    }
    public void DisposeInter()
    {
        if (PlayerPrefs.GetInt("RemoveAD") == 0)
        {
            if (this.interstitialAd != null)
            {
                this.interstitialAd.Dispose();
            }
        }
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("RemoveAD") == 0)
        {
            LoadInterstitial();
            LoadBanner();
        }
    }

    private void Update()
    {
        text.text = isLoaded.ToString();
    }

    public void LoadInterstitial()
    {
        if(IsTest == true)
        {
            this.interstitialAd = new InterstitialAd("IMG_16_9_LINK#YOUR_PLACEMENT_ID");
        }
        else
        {
            this.interstitialAd = new InterstitialAd("556535138746463_562882624778381");
        }
        
        this.interstitialAd.Register(this.gameObject);

        // Set delegates to get notified on changes or when the user interacts with the ad.
        this.interstitialAd.InterstitialAdDidLoad = (delegate () {
            Debug.Log("Interstitial ad loaded.");
            this.isLoaded = true;
        });
        interstitialAd.InterstitialAdDidFailWithError = (delegate (string error) {
            Debug.Log("Interstitial ad failed to load with error: " + error);
        });
        interstitialAd.InterstitialAdWillLogImpression = (delegate () {
            Debug.Log("Interstitial ad logged impression.");
        });
        interstitialAd.InterstitialAdDidClick = (delegate () {
            Debug.Log("Interstitial ad clicked.");
        });

        this.interstitialAd.interstitialAdDidClose = (delegate () {
            Debug.Log("Interstitial ad did close.");
            if (this.interstitialAd != null)
            {
                this.interstitialAd.Dispose();
                LoadInterstitial();
            }
        });

        // Initiate the request to load the ad.
        this.interstitialAd.LoadAd();
    }


    public void ShowInterstitial()
    {
        if (PlayerPrefs.GetInt("RemoveAD") == 0)
        {
            if (this.isLoaded)
            {
                this.interstitialAd.Show();
                this.isLoaded = false;
            }
            else
            {
                print("failed to Load Ad");
            }
        }
    }

    public void LoadBanner()
    {
        if (this.adView)
        {
            this.adView.Dispose();
        }
        if (IsTest == true)
        {
            this.adView = new AdView("IMG_16_9_APP_INSTALL#YOUR_PLACEMENT_ID", AdSize.BANNER_HEIGHT_50);
        }
        else
        {
            this.adView = new AdView("556535138746463_562883734778270", AdSize.BANNER_HEIGHT_50);
        }
        
        this.adView.Register(this.gameObject);

        // Set delegates to get notified on changes or when the user interacts with the ad.
        this.adView.AdViewDidLoad = (delegate () {
            //Screen.orientation = ScreenOrientation.Portrait;
            //currentScreenOrientation = Screen.orientation;
            Debug.Log("Banner loaded.");
            this.adView.Show(170,0);
        });
        adView.AdViewDidFailWithError = (delegate (string error) {
            Debug.Log("Banner failed to load with error: " + error);
        });
        adView.AdViewWillLogImpression = (delegate () {
            Debug.Log("Banner logged impression.");
        });
        adView.AdViewDidClick = (delegate () {
            Debug.Log("Banner clicked.");
        });

        // Initiate a request to load an ad.
        adView.LoadAd();
    }
}
*/