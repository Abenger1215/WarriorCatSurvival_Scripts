using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleAdMobManager : MonoBehaviour
{
    private RewardedAd rewardedAd_revive;
    private string adUnitId_reward; // »ùÇÃ ±¤°í ID

    GoogleAdMobManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        RequestRewardedAd();
    }

    public void RequestRewardedAd()
    {

#if UNITY_ANDROID
        adUnitId_reward = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            //adUnitId_reward = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId_reward = "unexpected_platform";
#endif

        this.rewardedAd_revive = new RewardedAd(adUnitId_reward);


        // Called when an ad request has successfully loaded.
        this.rewardedAd_revive.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd_revive.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd_revive.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd_revive.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd_revive.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd_revive.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();

        this.rewardedAd_revive.LoadAd(request);

    }
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.LoadAdError);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args) // ±¤°í ´ÝÀ»¶§ - ¹Ì¸® ±¤°í¸¦ ·ÎµùÇØµÒ
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
        RequestRewardedAd();
    }

    public void HandleUserEarnedReward(object sender, Reward args) // ±¤°í ½ÃÃ» ÈÄ º¸»ó ¾òÀ½
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

        GameManager.instance.Revive();
    }
    public void ShowReviveAd()
    {
        if (this.rewardedAd_revive.IsLoaded())
        {
            this.rewardedAd_revive.Show();
        }
        else
        {
            Debug.Log("NOT Loaded Interstitial");
            RequestRewardedAd();
        }
    }
}
