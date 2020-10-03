using System;
using UnityEngine;
using System.Runtime.InteropServices;

namespace AMR.iOS
{
	public class AMRRewardedVideo : IAMRRewardedVideo
	{
#if UNITY_IOS
		[DllImport("__Internal")]
		private static extern void _setRewardedVideoSuccessCallback(RewardedVideoSuccessCallback cb);

		[DllImport("__Internal")]
		private static extern void _setRewardedVideoFailCallback(RewardedVideoFailCallback cb);

		[DllImport("__Internal")]
		private static extern void _setRewardedVideoShowCallback(RewardedVideoShowCallback cb);

		[DllImport("__Internal")]
		private static extern void _setRewardedVideoFailToShowCallback(RewardedVideoFailToShowCallback cb);

        [DllImport("__Internal")]
		private static extern void _setRewardedVideoClickCallback(RewardedVideoClickCallback cb);

		[DllImport("__Internal")]
		private static extern void _setRewardedVideoCompleteCallback(RewardedVideoCompleteCallback cb);

		[DllImport("__Internal")]
		private static extern void _setRewardedVideoDismissCallback(RewardedVideoDismissCallback cb);

		[DllImport("__Internal")]
		private static extern IntPtr _loadRewardedVideoForZoneId(string zoneId, IntPtr rewardedVideoHandle);

		[DllImport("__Internal")]
		private static extern void _showRewardedVideo(IntPtr rewardedVideoHandle);
	
		[DllImport("__Internal")]
		private static extern void _showRewardedVideoWithTag(String tag, IntPtr rewardedVideoHandle);
#endif

        private delegate void RewardedVideoSuccessCallback(IntPtr rewardedVideoHandlePtr, string networkName, double ecpm);
		private delegate void RewardedVideoFailCallback(IntPtr rewardedVideoHandlePtr, string error);
        private delegate void RewardedVideoShowCallback(IntPtr rewardedVideoHandlePtr);
		private delegate void RewardedVideoFailToShowCallback(IntPtr rewardedVideoHandlePtr, string errorCode);
        private delegate void RewardedVideoClickCallback(IntPtr rewardedVideoHandlePtr, string networkName);
        private delegate void RewardedVideoCompleteCallback(IntPtr rewardedVideoHandlePtr);
		private delegate void RewardedVideoDismissCallback(IntPtr rewardedVideoHandlePtr);

		private IntPtr rewardedVideoPtr;

		[MonoPInvokeCallback(typeof(RewardedVideoSuccessCallback))]
		private static void rewardedVideoSuccessCallback(IntPtr rewardedVideoHandlePtr, string networkName, double ecpm)
		{
			GCHandle rewardedVideoHandle = (GCHandle)rewardedVideoHandlePtr;
			AMRRewardedVideoViewDelegate delegateObject = rewardedVideoHandle.Target as AMRRewardedVideoViewDelegate;
			delegateObject.didReceiveRewardedVideo(networkName, ecpm);
		}

		[MonoPInvokeCallback(typeof(RewardedVideoFailCallback))]
		private static void rewardedVideoFailCallback(IntPtr rewardedVideoHandlePtr, string error)
		{
			GCHandle rewardedVideoHandle = (GCHandle)rewardedVideoHandlePtr;
			AMRRewardedVideoViewDelegate delegateObject = rewardedVideoHandle.Target as AMRRewardedVideoViewDelegate;
			delegateObject.didFailtoReceiveRewardedVideo(error);
		}

        [MonoPInvokeCallback(typeof(RewardedVideoShowCallback))]
        private static void rewardedVideoShowCallback(IntPtr rewardedVideoHandlePtr)
        {
            GCHandle rewardedVideoHandle = (GCHandle)rewardedVideoHandlePtr;
            AMRRewardedVideoViewDelegate delegateObject = rewardedVideoHandle.Target as AMRRewardedVideoViewDelegate;
            delegateObject.didShowRewardedVideo();
        }
		
		[MonoPInvokeCallback(typeof(RewardedVideoFailToShowCallback))]
		private static void rewardedVideoFailToShowCallback(IntPtr rewardedVideoHandlePtr, string errorCode)
		{
			GCHandle rewardedVideoHandle = (GCHandle)rewardedVideoHandlePtr;
			AMRRewardedVideoViewDelegate delegateObject = rewardedVideoHandle.Target as AMRRewardedVideoViewDelegate;

            if (errorCode.Equals("1081"))
            {
                delegateObject.didFailtoShowRewardedVideo();
            }
            else
            {
                delegateObject.didFailtoReceiveRewardedVideo(errorCode);
            }

		}

        [MonoPInvokeCallback(typeof(RewardedVideoClickCallback))]
        private static void rewardedVideoClickCallback(IntPtr rewardedVideoHandlePtr, string networkName)
        {
            GCHandle rewardedVideoHandle = (GCHandle)rewardedVideoHandlePtr;
            AMRRewardedVideoViewDelegate delegateObject = rewardedVideoHandle.Target as AMRRewardedVideoViewDelegate;
            delegateObject.didClickRewardedVideo(networkName);
        }

        [MonoPInvokeCallback(typeof(RewardedVideoCompleteCallback))]
		private static void rewardedVideoCompleteCallback(IntPtr rewardedVideoHandlePtr)
		{
			GCHandle rewardedVideoHandle = (GCHandle)rewardedVideoHandlePtr;
			AMRRewardedVideoViewDelegate delegateObject = rewardedVideoHandle.Target as AMRRewardedVideoViewDelegate;
			delegateObject.didCompleteRewardedVideo();
		}

		[MonoPInvokeCallback(typeof(RewardedVideoDismissCallback))]
		private static void rewardedVideoDismissCallback(IntPtr rewardedVideoHandlePtr)
		{
			GCHandle rewardedVideoHandle = (GCHandle)rewardedVideoHandlePtr;
			AMRRewardedVideoViewDelegate delegateObject = rewardedVideoHandle.Target as AMRRewardedVideoViewDelegate;
			delegateObject.didDismissRewardedVideo();
		}

#region - IAMRRewardedVideo

		public void loadRewardedVideoForZoneId(string zoneId, AMRRewardedVideoViewDelegate delegateObject)
		{
#if UNITY_IOS
				_setRewardedVideoSuccessCallback(rewardedVideoSuccessCallback);
				_setRewardedVideoFailCallback(rewardedVideoFailCallback);
                _setRewardedVideoShowCallback(rewardedVideoShowCallback);
				_setRewardedVideoFailToShowCallback(rewardedVideoFailToShowCallback);
                _setRewardedVideoClickCallback(rewardedVideoClickCallback);
				_setRewardedVideoCompleteCallback(rewardedVideoCompleteCallback);
				_setRewardedVideoDismissCallback(rewardedVideoDismissCallback);

				GCHandle handle = GCHandle.Alloc(delegateObject);
				IntPtr parameter = (IntPtr)handle;

				rewardedVideoPtr = _loadRewardedVideoForZoneId(zoneId, parameter);
#endif
        }

        public void showRewardedVideo()
		{
#if UNITY_IOS
				_showRewardedVideo(rewardedVideoPtr);
#endif           
		}

        public void showRewardedVideo(String tag)
        {
#if UNITY_IOS
				_showRewardedVideoWithTag(tag, rewardedVideoPtr);
#endif
        }

        public void destroyRewardedVideo()
        {
        }
        
#endregion
    }
}

