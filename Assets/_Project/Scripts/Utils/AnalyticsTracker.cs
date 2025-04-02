using UnityEngine;
using VG2;
using Zenject;

public class AnalyticsTracker : MonoBehaviour
{

    [Inject] private LevelController _levelController;


    private void Start()
    {
        Ads.Rewarded.onShown += OnAdRewardedSgown;
        Ads.Interstitial.onShown += OnInterstitialShow;
    }

    private void OnInterstitialShow(string adKey, Ads.Interstitial.Result result)
    {
        if (result == Ads.Interstitial.Result.Success)
            Analytics.SendEvent(EventKey.InterstitialShown);
    }

    private void OnAdRewardedSgown(string adKey, Ads.Rewarded.Result result)
    {
        if (result == Ads.Rewarded.Result.Success)
            Analytics.SendEvent(EventKey.AdRewardedShown(adKey));
    }




}
