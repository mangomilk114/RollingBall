using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admanager : MonoBehaviour {

    public static Admanager _instance = null;
    public static Admanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Admanager>() as Admanager;
            }
            return _instance;
        }
    }

    //public string android_banner_id = "ca-app-pub-1646746921531266/5192364858";
    public string android_banner_id = "ca-app-pub-3940256099942544/6300978111";
    
    public string ios_banner_id = "ca-app-pub-1646746921531266/5930731456";

    private BannerView bannerView;


    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
        RequestBannerAd();
    }

    public void RequestBannerAd()
    {
        string adUnitId = string.Empty;

#if UNITY_ANDROID
        adUnitId = android_banner_id;
#elif UNITY_IOS
        adUnitId = ios_bannerAdUnitId;
#endif


        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();

        bannerView.LoadAd(request);
        bannerView.Show();
    }

    public void ShowBannerAd()
    {
      
    }

	// Update is called once per frame
	void Update () {
		
	}
}
