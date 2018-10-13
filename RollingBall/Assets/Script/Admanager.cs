using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements; 


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


    public string android_Ads_id = "ca-app-pub-3940256099942544~3347511713"; //"ca-app-pub-1646746921531266~8010099889"
    public string ios_Ads_id = "ca-app-pub-1646746921531266~4785170747";


    public string android_banner_id = "ca-app-pub-3940256099942544/6300978111"; //"ca-app-pub-3940256099942544/6300978111";    
    public string ios_banner_id = "ca-app-pub-1646746921531266/5930731456";

    private BannerView bannerView;

    public string android_interstitial_id = "ca-app-pub-3940256099942544/1033173712"; //"ca-app-pub-1646746921531266/2327426150"; 
    public string ios_interstitial_id = "ca-app-pub-1646746921531266/8643630482";
    private InterstitialAd interstitial;

    private const string android_game_id = "2836744";
    private const string ios_game_id = "2836743";

    private const string video_id = "video";


    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);

#if UNITY_ANDROID
        string appId = android_Ads_id;
#elif UNITY_IPHONE
            string appId = ios_Ads_id;
#else
            string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        RequestBannerAd();
        RequestInterstitial();
        Initialize();
    }

    //게임 하단에 표시되는 애드몹 배너
    public void RequestBannerAd()
    {
        string adUnitId = string.Empty;

#if UNITY_ANDROID
        adUnitId = android_banner_id;
#elif UNITY_IOS
        adUnitId = ios_banner_id;
#endif

        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();   

        bannerView.LoadAd(request);
        bannerView.Show();
    }

    public void ShowBannerAd()
    {
        bannerView.Show();
    }

    public void HandleOnInterstitialAdClosed(object sender, EventArgs args)
    {
        print("HandleOnInterstitialAdClosed event received.");

        interstitial.Destroy();

        RequestInterstitial();
    }

    private void RequestInterstitial()
    {
        string adUnitId = string.Empty;

#if UNITY_ANDROID
        adUnitId = android_interstitial_id;
#elif UNITY_IOS
        adUnitId = ios_interstitial_id;
#endif

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
        interstitial.OnAdClosed += HandleOnInterstitialAdClosed;
    }

    //게임 오버에 표시되는 전면 애드몹 광고
    public void ShowinterstitialAd()
    {
        if (!interstitial.IsLoaded())
        {
            RequestInterstitial();
            interstitial.Show();
            return;
        }
        interstitial.Show();

    }


    private void Initialize()
    {
#if UNITY_ANDROID
        Advertisement.Initialize(android_game_id);
#elif UNITY_IOS
        Advertisement.Initialize(ios_game_id);
#endif
    }

    //게임 시작 전에 재생되는 유니티 애즈 동영상광고
    public void ShowVideoAds()
    {
        if (Advertisement.IsReady(video_id))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };

            Advertisement.Show(video_id, options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                {
                    Debug.Log("The ad was successfully shown.");

                    GamePlayManager.Instance.GameReady();
                    // to do ...
                    // 광고 시청이 완료되었을 때 처리

                    break;
                }
            case ShowResult.Skipped:
                {
                    Debug.Log("The ad was skipped before reaching the end.");
                    GamePlayManager.Instance.GameReady();
                    // to do ...
                    // 광고가 스킵되었을 때 처리

                    break;
                }
            case ShowResult.Failed:
                {
                    Debug.LogError("The ad failed to be shown.");
                    GamePlayManager.Instance.GameReady();
                    // to do ...
                    // 광고 시청에 실패했을 때 처리

                    break;
                }
        }
    }


	// Update is called once per frame
	void Update () {
		
	}
}
