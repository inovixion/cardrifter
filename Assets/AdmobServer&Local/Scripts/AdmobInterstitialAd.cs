using UnityEngine ;

public class AdmobInterstitialAd : MonoBehaviour {
    [Header("Admob script :")]
    [SerializeField] private Admob admob;
    private static AdmobInterstitialAd _instance;
    public static AdmobInterstitialAd Instance
    {
        get

        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AdmobInterstitialAd>();
            }
            return _instance;
        }

    }
    private void Start()
    {
        admob.OnInitComplete += () => admob.RequestInterstitialAd();
        admob.OnInterstitialAdClosed += () => admob.RequestInterstitialAd();
        admob.OnInterstitialAdFailedToLoad += () => admob.DestroyInterstitialAd();

    }
    public void showinter()
    {
        admob.ShowInterstitialAd();
    }
}
