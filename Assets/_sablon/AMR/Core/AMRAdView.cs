namespace AMR
{
    public class AMRAdView
    {
        public delegate void EventDelegateReady(string networkName, double ecpm);
        public delegate void EventDelegateFail(string errorMessage);
        public delegate void EventDelegateShow();
	    public delegate void EventDelegateFailToShow();
        public delegate void EventDelegateClick(string networkName);
        public delegate void EventDelegateComplete();
        public delegate void EventDelegateDismiss();        
    }
}