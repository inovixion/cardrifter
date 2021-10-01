using UnityEngine ;

public class AdmobBannerAd : MonoBehaviour {
    
    
    [Header("Admob script :")]
    [SerializeField] private Admob admob;

    private static AdmobBannerAd _instance;
    public static AdmobBannerAd Instance
    {
        get

        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AdmobBannerAd>();
            }
            return _instance;
        }

    }

    private void Start()
    {
        //show banner ad when admob sdk is initialized:
        admob.OnInitComplete += () => admob.ShowBannerAd();


        //here you can use banner events:
        admob.OnBannerAdOpening += () => {
            Debug.Log("Banner ad is clicked");
        };
    }

    public void HideBanner()
    {
        admob.HideBannerAd();
    }

    public void ShowBanner()
    {
        admob.ShowBannerAd();
    }
}
