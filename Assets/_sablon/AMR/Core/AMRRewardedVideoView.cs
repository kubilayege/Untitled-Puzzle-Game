using System;
using UnityEngine;

namespace AMR
{
	public class AMRRewardedVideoView: AMRAdView
    {
        private class VideoDelegate : AMRRewardedVideoViewDelegate
        {
            private AMRRewardedVideoView videoView;
            public VideoDelegate(AMRRewardedVideoView vv)
            {
                videoView = vv;
            }

            public void didReceiveRewardedVideo(string networkName, double ecpm)
            {
                videoView.state = VideoState.Loaded;
                videoView.failCount = 0;
                if (videoView.onReadyDelegate != null)
                {
                    videoView.onReadyDelegate(networkName, ecpm);
                }
            }

            public void didFailtoReceiveRewardedVideo(string error)
            {
                videoView.failCount++;
                videoView.state = VideoState.New;
                if (videoView.failCount <= 0)
                { // Try once again when not loaded 
                    videoView.loadRewardedVideoForZoneId(videoView.iosZoneId, videoView.androidZoneId, videoView.isRefresh);
                }
                else
                {
                    videoView.failCount = 0;
                    if (videoView.onFailDelegate != null)
                    {
                        videoView.onFailDelegate(error);
                    }
                }
            }

            public void didShowRewardedVideo()
            {
                if (videoView.onShowDelegate != null)
                {
                    videoView.onShowDelegate();
                }
            }

            public void didFailtoShowRewardedVideo()
            {
                videoView.state = VideoState.Loaded;

                if (videoView.onFailToShowDelegate != null)
                {
                    videoView.onFailToShowDelegate();
                }
            }

            public void didClickRewardedVideo(string networkName)
            {
                if (videoView.onClickDelegate != null)
                {
                    videoView.onClickDelegate(networkName);
                }
            }

            public void didCompleteRewardedVideo()
            {
				if (videoView.onCompleteDelegate != null) 
				{
					videoView.onCompleteDelegate ();
				}
            }

            public void didDismissRewardedVideo()
            {
                videoView.state = VideoState.New;

				AMRSDK.resolveBannerConflict();
				if (videoView.onDismissDelegate != null) 
				{
                    videoView.onDismissDelegate ();
				}
            }
        }
        private static AMRRewardedVideoView instance;
        private IAMRRewardedVideo rewardedVideo;
        private VideoState state;

        private EventDelegateReady onReadyDelegate;
        private EventDelegateFail onFailDelegate;
        private EventDelegateShow onShowDelegate;
        private EventDelegateFailToShow onFailToShowDelegate;
        private EventDelegateClick onClickDelegate;
        private EventDelegateComplete onCompleteDelegate;
        private EventDelegateDismiss onDismissDelegate;
        

        private int failCount = 0;
        private String androidZoneId, iosZoneId;
        private bool isRefresh;

        private enum VideoState
        {
            New,
            Loading,
            Loaded,
            Playing
        }

        public static AMRRewardedVideoView Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AMRRewardedVideoView();
                }
                return instance;
            }
        }

        public void loadRewardedVideoForZoneId(string zoneIdiOS,
                                                string zoneIdAndroid,
                                                bool refresh)
        {
            if (refresh)
            {
                if (state != VideoState.New && state != VideoState.Loaded)
                {
                    return;
                }
            }
            else if (state != VideoState.New)
            {
	            //Debug.Log("<AMRSDK> loadRewardedVideoForZoneId - state != VideoState.New returning");
                return;
            }

            state = VideoState.Loading;
            androidZoneId = zoneIdAndroid;
            iosZoneId = zoneIdiOS;
            isRefresh = refresh;
            VideoDelegate videoDelegate = new VideoDelegate(this);

            if (Application.platform == RuntimePlatform.IPhonePlayer) 
            {
				if (zoneIdiOS != null)
				{
					rewardedVideo = new AMR.iOS.AMRRewardedVideo();
					rewardedVideo.loadRewardedVideoForZoneId(zoneIdiOS, videoDelegate);
				}
            }
            else if (Application.platform == RuntimePlatform.Android) 
            {
				if (zoneIdAndroid != null)
				{
					if (rewardedVideo != null)
					{
						rewardedVideo.destroyRewardedVideo();
					}
					rewardedVideo = new AMR.Android.AMRRewardedVideo();
					rewardedVideo.loadRewardedVideoForZoneId(zoneIdAndroid, videoDelegate);
				}
            }
        }

        public void showRewardedVideo()
        {
            state = VideoState.Playing;
            rewardedVideo.showRewardedVideo();
        }

        public void showRewardedVideo(String tag)
        {
            state = VideoState.Playing;
            rewardedVideo.showRewardedVideo(tag);
        }

        /* States */
        public Boolean isReady()
        {
            return state == VideoState.Loaded;
        }

        public Boolean isPlaying()
        {
            return state == VideoState.Playing;
        }

        /* Possible Callbacks */
        public void setOnReady(EventDelegateReady onReadyDelegate) {
            this.onReadyDelegate = onReadyDelegate;
        }

        public void setOnFail(EventDelegateFail onFailDelegate)
        {
            this.onFailDelegate = onFailDelegate;
        }

        public void setOnShow(EventDelegateShow onShowDelegate)
        {
            this.onShowDelegate = onShowDelegate;
        }

        public void setOnFailToShow(EventDelegateFailToShow onFailToShowDelegate)
        {
            this.onFailToShowDelegate = onFailToShowDelegate;
        }

        public void setOnClick(EventDelegateClick onClickDelegate)
        {
            this.onClickDelegate = onClickDelegate;
        }

        public void setOnComplete(EventDelegateComplete onCompleteDelegate) {
            this.onCompleteDelegate = onCompleteDelegate;
        }

	    public void setOnDismiss(EventDelegateDismiss onDismissDelegate) {
            this.onDismissDelegate = onDismissDelegate;
        }
    }
}

