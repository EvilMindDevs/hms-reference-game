using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Constants
{
    public class Ads
    {
#if UNITY_RELEASE
        public const string ADMOB_BANNER_ID "ca-app-pub-3940256099942544/6300978111";
        public const string ADMOB_INTERSTITIAL_ID = "ca-app-pub-3940256099942544/1033173712";
        public const string ADMOB_REWARDED_ID = "ca-app-pub-3940256099942544/5354046379";
#else
        public const string ADMOB_BANNER_ID = "ca-app-pub-3940256099942544/6300978111";
        public const string ADMOB_INTERSTITIAL_ID = "ca-app-pub-3940256099942544/1033173712";
        public const string ADMOB_REWARDED_ID = "ca-app-pub-3940256099942544/5354046379";
#endif  
        public const int ADMOB_INTERSTITIAL_FREQUENCY = 5;
    }
}
