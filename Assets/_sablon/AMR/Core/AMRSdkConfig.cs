using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AMR
{
    public class AMRSdkConfig : MonoBehaviour
    {
        public string ApplicationIdAndroid;
        public string ApplicationIdIOS;
        public string BannerIdAndroid;
        public string BannerIdIOS;
        public string InterstitialIdAndroid;
        public string InterstitialIdIOS;
        public string RewardedVideoIdAndroid;
        public string RewardedVideoIdIOS;
        public string OfferWallIdAndroid;
        public string OfferWallIdIOS;
        public string UserConsent;
        public string SubjectToGDPR;
        public bool IsUserChild;

        public static AMRSdkConfig instance;
        public void Awake()
        {
            if (instance == null)
                instance = this;
        }
        void Start()
        {
            AMRSdkConfig config = new AMRSdkConfig();
            config.ApplicationIdAndroid = "";
            config.ApplicationIdIOS = "72bd5c31-3ddb-411a-b355-fdd88bd223dc";

            config.BannerIdAndroid = "";
            config.BannerIdIOS = "c4b90936-20e5-4919-b83c-f4d7fd3d4e25";

            config.InterstitialIdAndroid = "";
            config.InterstitialIdIOS = "d3523276-87eb-42f3-a5f1-c38ea3f0a702";

            config.RewardedVideoIdAndroid = "";
            config.RewardedVideoIdIOS = "a5690814-40ba-4afc-a028-26b1857ab816";

            //config.OfferWallIdAndroid = "<Your Android Offerwall Zone Id>";
            //config.OfferWallIdIOS = "<Your IOS Offerwall Zone Id>";

            AMRSDK.startWithConfig(config);

            //AMRSDK.loadBanner(Enums.AMRSDKBannerPosition.BannerPositionBottom, true);

            AMRSDK.loadInterstitial();
            AMRSDK.loadRewardedVideo();

            AMRSDK.setOnBannerReady(onBannerReady);
            AMRSDK.setOnBannerFail(onBannerFail);
            AMRSDK.setOnBannerClick(onBannerClick);

            AMRSDK.setOnInterstitialReady(OnInterstitialReady);
            AMRSDK.setOnInterstitialFail(OnInterstitialFail);
            AMRSDK.setOnInterstitialShow(OnInterstitialShow);
            AMRSDK.setOnInterstitialClick(OnInterstitialClick);
            AMRSDK.setOnInterstitialDismiss(OnInterstitialDismiss);

            AMRSDK.setOnRewardedVideoReady(OnVideoReady);

            AMRSDK.setOnRewardedVideoFail(OnVideoFail);
            AMRSDK.setOnRewardedVideoShow(OnVideoShow);
            AMRSDK.setOnRewardedVideoClick(OnVideoClick);
            AMRSDK.setOnRewardedVideoDismiss(OnVideoDismiss);
            AMRSDK.setOnRewardedVideoComplete(OnVideoComplete);

        }
        public void onBannerReady(string networkName, double ecpm) { }
        public void onBannerFail(string error) { }
        public void onBannerClick(string networkName) { }

        // It indicates that the interstitial ad is loaded and ready to show.
        public void OnInterstitialReady(string networkName, double ecpm) { }

        // It indicates that the interstitial ad received no-fill response from all of its placements. Therefore, the ad can not be shown. You may choose to try loading it again.
        public void OnInterstitialFail(string errorMessage) { }

        // It indicates that the loaded interstitial ad is shown to the user.
        public void OnInterstitialShow() { }

        // It indicates that the interstitial ad is clicked.
        public void OnInterstitialClick(string networkName) { }

        // It indicates that the interstitial ad is closed by clicking cross button/back button
        public void OnInterstitialDismiss()
        {
            //timer i sifirliyoruz
            Timer.Reset();

            AdManager.instance.onComplete(PlayerDataController.data.whichVideoFor, AdManager.AD_STATES.NOT_COMPLETE);
            PlayerDataController.SaveData("whichVideoFor", "");
            AMRSDK.loadInterstitial();
            Debug.Log("Intersititial Ad dismiss!");
        }

        // It indicates that the rewarded video ad received no-fill response from all of its placements.

        public void OnVideoReady(string networkName, double ecpm) { }
        //Therefore, the ad can not be shown. You may choose to try loading it again.
        public void OnVideoFail(string errorMessage) { }

        // It indicates that the loaded rewarded video ad is shown to the user.(Note: It does not mean that the user deserves a reward)
        // It is immediately called after the loaded ad is shown to the user using AMRSDK.showRewardedVideo()
        public void OnVideoShow()
        {
            PlayerDataController.SaveData("isRewardedVideoWatched", false);
        }

        // It indicates that the rewarded video ad is clicked.
        public void OnVideoClick(string networkName) { }

        // It indicates that the rewarded video ad is closed by clicking cross button/back button.
        // It does not mean that the user deserves to receive a reward. You need to check whether OnVideoComplete callback is called or not.
        public void OnVideoDismiss()
        {
            if (PlayerDataController.data.isRewardedVideoWatched)
            {
                AdManager.instance.onComplete(PlayerDataController.data.whichVideoFor, AdManager.AD_STATES.COMPLETE);
            }
            else
            {
                AdManager.instance.onComplete(PlayerDataController.data.whichVideoFor, AdManager.AD_STATES.NOT_COMPLETE);
            }
            PlayerDataController.SaveData("whichVideoFor", "");
            AMRSDK.loadRewardedVideo();
            Debug.Log("Video Ad dismiss!");
        }

        // It indicates that the user deserves to receive a reward. You may need to store this information in a variable and give a reward
        // to the user after OnVideoDismiss() callback is called by showing some animations for instance.
        // Note: If OnVideoComplete callback is called for the ad, it is always called before OnVideoDismiss() callback.
        public void OnVideoComplete()
        {
            PlayerDataController.SaveData("isRewardedVideoWatched", true);

            AdManager.instance.onComplete(PlayerDataController.data.whichVideoFor, AdManager.AD_STATES.COMPLETE);
            if (PlayerDataController.data.GetVideoReward(PlayerDataController.data.whichVideoFor) > 0)
            {
                PlayerDataController.SaveData(PlayerDataController.data.whichVideoFor, PlayerDataController.data.GetVideoReward(PlayerDataController.data.whichVideoFor), true);

            }
            Debug.Log("Video Ad complete!");
        }
    }
}
