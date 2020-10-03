using System;
namespace AMR
{
	public interface IAMRRewardedVideo
	{
		void loadRewardedVideoForZoneId(string zoneId, AMRRewardedVideoViewDelegate delegateObject);
		void showRewardedVideo();
        void showRewardedVideo(String tag);
        void destroyRewardedVideo();
    }
}

