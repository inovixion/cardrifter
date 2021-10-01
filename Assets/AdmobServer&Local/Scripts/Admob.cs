using UnityEngine ;
using GoogleMobileAds.Api ;
using GoogleMobileAds.Common ;
using UnityEngine.Events ;

public class Admob : MonoBehaviour {
    [Header("Admob Ad Units :")]
    string idBanner;
    string idRectBanner;
    string idInterstitial;
    string idReward;

    [Header("Admob Server AD Units :")]
    [SerializeField] [Tooltip("Check it if you want to load ad units IDs from a server.   if ad units didn't load successfully or some error happened, Admob will use ad units above.")] private bool serverAdUnitsEnabled = false;

    [HideInInspector] public BannerView AdBanner;
    [HideInInspector] public BannerView AdRectBanner;
    [HideInInspector] public InterstitialAd AdInterstitial;
    [HideInInspector] public RewardedAd AdReward;
    public bool isTest;

    public UnityAction OnInitComplete;
    private static Admob _instance;
    public static Admob Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Admob>();
            }
            return _instance;
        }

    }
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("RemoveAD"))
        {
            PlayerPrefs.SetInt("RemoveAD", 0);
        }
        if(isTest)
        {
            print("IsTest is On");
            idBanner = "ca-app-pub-3940256099942544/6300978111";
            idRectBanner = "ca-app-pub-3940256099942544/6300978111";
            idInterstitial = "ca-app-pub-3940256099942544/1033173712";
            idReward = "ca-app-pub-3940256099942544/5224354917";
        }
        else
        {
            print("IsTest is OFF");
            idBanner = "ca-app-pub-2426608367111061/5788811554";
            idRectBanner = "ca-app-pub-2426608367111061/8439025625";
            idInterstitial = "ca-app-pub-2426608367111061/6031998917";
            idReward = "ca-app-pub-3940256099942544/5224354917";
        }
    }
    private void Start()
    {
        // if server Ad Units is enabled then use server units:
        if (serverAdUnitsEnabled)
        {
            idBanner = (string.IsNullOrEmpty(AdmobServerAdUnits.BannerId)) ? idBanner : AdmobServerAdUnits.BannerId;
            idRectBanner = (string.IsNullOrEmpty(AdmobServerAdUnits.RectBannerId)) ? idRectBanner : AdmobServerAdUnits.RectBannerId;
            idInterstitial = (string.IsNullOrEmpty(AdmobServerAdUnits.InterstitialId)) ? idInterstitial : AdmobServerAdUnits.InterstitialId;
            idReward = (string.IsNullOrEmpty(AdmobServerAdUnits.RewardId)) ? idReward : AdmobServerAdUnits.RewardId;
        }
#if UNITY_EDITOR
        //this block is just for debugging in the editor
        string color;
        if (AdmobServer.adUnitsLoaded)
        {
            color = "yellow";
            Debug.Log(">><color=yellow>You're using <b>SERVER</b> ad units :</color>");
        }
        else
        {
            color = "orange";
            Debug.Log(">><color=orange>You're using default ad units :</color>");
        }
        Debug.Log("<color=" + color + "><b>Banner       : </b>" + idBanner + "</color>");
        Debug.Log("<color=" + color + "><b>RectBanner       : </b>" + idRectBanner + "</color>");
        Debug.Log("<color=" + color + "><b>Interstitial : </b>" + idInterstitial + "</color>");
        Debug.Log("<color=" + color + "><b>Reward      : </b>" + idReward + "</color>");
#endif

        RequestConfiguration requestConfiguration =
           new RequestConfiguration.Builder()
              .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
              .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);


        MobileAds.Initialize(initstatus =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (OnInitComplete != null)
                    OnInitComplete.Invoke();
            });
        });
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
           .TagForChildDirectedTreatment(false)
           .AddExtra("npa", PlayerPrefs.GetString("npa", "1"))
           .Build();
    }

    private void OnDestroy()
    {
        DestroyBannerAd();
        DestroyRectBannerAd();
        DestroyInterstitialAd();
    }


    #region Banner Ad ------------------------------------------------------------------------------

    public UnityAction OnBannerAdOpening;

    public void ShowBannerAd()
    {
        if (PlayerPrefs.GetInt("RemoveAD") == 0)
        {
            AdBanner = new BannerView(idBanner, AdSize.Banner, AdPosition.Top);
            AdBanner.OnAdOpening += (sender, e) =>
            {
                if (OnBannerAdOpening != null)
                    OnBannerAdOpening.Invoke();
            };
            AdBanner.LoadAd(CreateAdRequest());
        }
    }

    public void DestroyBannerAd()
    {
        if (AdBanner != null)
            AdBanner.Destroy();
    }

    public void HideBannerAd()
    {
        if (AdBanner != null)
            AdBanner.Hide();
    }

    #endregion


    #region RectBanner Ad ------------------------------------------------------------------------------

    public UnityAction OnRectBannerAdOpening;

    public void ShowRectBannerAd()
    {
        if (PlayerPrefs.GetInt("RemoveAD") == 0)
        {


            AdRectBanner = new BannerView(idRectBanner, AdSize.MediumRectangle, AdPosition.BottomLeft);
            AdRectBanner.OnAdOpening += (sender, e) =>
            {
                if (OnRectBannerAdOpening != null)
                    OnRectBannerAdOpening.Invoke();
            };
            AdRectBanner.LoadAd(CreateAdRequest());
        }
    }

    public void DestroyRectBannerAd()
    {
        if (AdRectBanner != null)
            AdRectBanner.Destroy();
    }
    public void HideRectBanner()
    {
        if (AdRectBanner != null)
            AdRectBanner.Hide();

    }

    #endregion

    #region Interstitial Ad ------------------------------------------------------------------------

    public UnityAction OnInterstitialAdLoaded;
    public UnityAction OnInterstitialAdFailedToLoad;
    public UnityAction OnInterstitialAdOpening;
    public UnityAction OnInterstitialAdClosed;

    public void RequestInterstitialAd()
    {
        AdInterstitial = new InterstitialAd(idInterstitial);
        AdInterstitial.OnAdClosed += (sender, e) =>
        {
            if (OnInterstitialAdClosed != null)
                OnInterstitialAdClosed.Invoke();
        };
        AdInterstitial.OnAdOpening += (sender, e) =>
        {
            if (OnInterstitialAdOpening != null)
                OnInterstitialAdOpening.Invoke();
        };
        AdInterstitial.OnAdFailedToLoad += (sender, e) =>
        {
            if (OnInterstitialAdFailedToLoad != null)
                OnInterstitialAdFailedToLoad.Invoke();
        };
        AdInterstitial.OnAdLoaded += (sender, e) =>
        {
            if (OnInterstitialAdLoaded != null)
                OnInterstitialAdLoaded.Invoke();
        };
        AdInterstitial.LoadAd(CreateAdRequest());
    }

    public void ShowInterstitialAd()
    {
        if (AdInterstitial.IsLoaded())
        {

            if (PlayerPrefs.GetInt("RemoveAD") == 0)
            {
                AdInterstitial.Show();
            }

        }
    }

    public void DestroyInterstitialAd()
    {
        if (AdInterstitial != null)
            AdInterstitial.Destroy();
    }

    #endregion

    #region Reward Ad ------------------------------------------------------------------------------

    public UnityAction<Reward> OnRewardAdWatched;
    public UnityAction OnRewardAdLoaded;
    public UnityAction OnRewardAdFailedToLoad;
    public UnityAction OnRewardAdOpening;
    public UnityAction OnRewardAdClosed;

    public void RequestRewardAd()
    {
        AdReward = new RewardedAd(idReward);
        AdReward.OnAdClosed += (sender, e) =>
        {
            if (OnRewardAdClosed != null)
                OnRewardAdClosed.Invoke();
        };
        AdReward.OnAdOpening += (sender, e) =>
        {
            if (OnRewardAdOpening != null)
                OnRewardAdOpening.Invoke();
        };
        AdReward.OnAdFailedToLoad += (sender, e) =>
        {
            if (OnRewardAdFailedToLoad != null)
                OnRewardAdFailedToLoad.Invoke();
        };
        AdReward.OnAdLoaded += (sender, e) =>
        {
            if (OnRewardAdLoaded != null)
                OnRewardAdLoaded.Invoke();
        };
        AdReward.OnUserEarnedReward += (sender, reward) =>
        {
            if (OnRewardAdWatched != null)
                OnRewardAdWatched.Invoke(reward);
        };
        AdReward.LoadAd(CreateAdRequest());
    }

    public void ShowRewardAd()
    {
        if (AdReward.IsLoaded())
        {
            AdReward.Show();
        }
    }

    #endregion



}
