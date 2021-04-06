using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

#if HMS_BUILD
using HmsPlugin;
#endif
using HuaweiMobileServices.Id;
using System;
using HuaweiMobileServices.Utils;
using HuaweiMobileServices.Game;
using HuaweiMobileServices.IAP;

public class MainMenu : SimpleMenu<MainMenu>
{

    [SerializeField]
    private GameObject huaweiButton;
    [SerializeField]
    private GameObject signinGoogleButton;
    [SerializeField]
    private GameObject groupButton;

    [SerializeField]
    private GameObject removeAdsButton;

    private string removeAds = "com.samet.reffapp.huawei.removeads";

    List<ProductInfo> productInfoList = new List<ProductInfo>();
    List<string> productPurchasedList = new List<string>();
#if GMS_BUILD
    private GMSAuthManager gmsaccountManager = null;
#endif
    void Start()
    {
#if GMS_BUILD
        gmsaccountManager = GMSAuthManager.GetInstance();
        //tapToPlayText.DOFade(1f, 0.6f).SetLoops(-1, LoopType.Yoyo).SetDelay(2f);
        //tapToPlayText.enabled = false;
        huaweiButton.SetActive(false);
        signinGoogleButton.SetActive(!gmsaccountManager.IsSignedIn());
        groupButton.SetActive(gmsaccountManager.IsSignedIn());
#endif
#if HMS_BUILD
        signinGoogleButton.SetActive(false);

        HMSAccountManager.Instance.OnSignInSuccess = OnLoginSuccess;
        HMSAccountManager.Instance.OnSignInFailed = OnSignInFailed;
        huaweiButton.SetActive(!HMSAccountManager.Instance.IsSignedIn());
        groupButton.SetActive(HMSAccountManager.Instance.IsSignedIn());
 
#endif
    }

    public void Login()
    {
#if GMS_BUILD
        if (gmsaccountManager != null)
        {
            gmsaccountManager.SignInWithGoogle();
            if (gmsaccountManager.IsSignedIn())
            {
                StartCoroutine(OnLoginCO());
            }
        }
        else
        {
            Debug.LogError("Account Manager is null");
        }
#endif
#if HMS_BUILD
        
        HMSAccountManager.Instance.SignIn();
 
#endif
    }
    IEnumerator OnLoginCO()
    {
        Debug.Log("Waiting for frame");
        yield return new WaitForEndOfFrame();
        //yield return new WaitForSeconds(5);
        //yield return new WaitForSecondsRealtime(1);
#if GMS_BUILD
        signinGoogleButton.SetActive(false);
        groupButton.SetActive(true);
#endif
    }

    private void OnLoginSuccess(AuthAccount obj)
    {
        StartCoroutine(OnLoginCO(obj));
    }

    IEnumerator OnLoginCO(AuthAccount obj)
    {
        Debug.Log("Waiting for frame");
        yield return new WaitForEndOfFrame();
        GameManager.Instance.ArrangeManagers();
#if GMS_BUILD
        signinGoogleButton.SetActive(false);
        groupButton.SetActive(true);
#endif

#if HMS_BUILD

        HMSIAPManager.Instance.OnCheckIapAvailabilityFailure = (error) =>
        {
            Debug.Log($"[HMSPlugin]: IAP check failed. {error.Message}");
        };
        HMSIAPManager.Instance.OnCheckIapAvailabilitySuccess = OnCheckIapAvailabilitySuccess;
        HMSIAPManager.Instance.CheckIapAvailability();
        groupButton.SetActive(true);
        huaweiButton.SetActive(false);
#endif
    }

    private void OnCheckIapAvailabilitySuccess()
    {

#if HMS_BUILD
        Debug.Log("[HMS]: LoadStore");
        HMSIAPManager.Instance.OnObtainProductInfoSuccess = (productInfoResultList) =>
        {
            if (productInfoResultList != null)
            {
                foreach (ProductInfoResult productInfoResult in productInfoResultList)
                {
                    foreach (ProductInfo productInfo in productInfoResult.ProductInfoList)
                    {
                        productInfoList.Add(productInfo);
                    }
                }
            }
            RestorePurchases();
        };
        // Set Callback for ObtainInfoFailure
        HMSIAPManager.Instance.OnObtainProductInfoFailure = (error) =>
        {
            Debug.Log($"[HMSPlugin]: IAP ObtainProductInfo failed. {error.WrappedCauseMessage + error.WrappedExceptionMessage}");
        };

        // Call ObtainProductInfo 
        HMSIAPManager.Instance.ObtainProductInfo(new List<string>(), new List<string>() { removeAds }, new List<string>());
#endif
    }

    public void BuyProduct(string productID)
    {
#if HMS_BUILD
        
        HMSIAPManager.Instance.BuyProduct(productID);
#endif
    }

    private void RestorePurchases()
    {
#if HMS_BUILD
        HMSIAPManager.Instance.OnObtainOwnedPurchasesSuccess = (ownedPurchaseResult) =>
        {
            productPurchasedList = (List<string>)ownedPurchaseResult.InAppPurchaseDataList;
            removeAdsButton.gameObject.SetActive(false);
            HMSAdsKitManager.Instance.HideBannerAd();
        };

        HMSIAPManager.Instance.OnObtainOwnedPurchasesFailure = (error) =>
        {
            Debug.Log("[HMS:] RestorePurchasesError" + error.Message);
        };

        HMSIAPManager.Instance.ObtainOwnedPurchases();
#endif
    }

    public ProductInfo GetProductInfo(string productID)
    {
        return productInfoList.Find(productInfo => productInfo.ProductId == productID);
    }

    private void OnSignInFailed(HMSException obj)
    {
        Debug.LogError("Error :" + obj.ErrorCode);
    }

    public void OnPlayClick()
    {
#if GMS_BUILD
        if (Time.timeSinceLevelLoad < Const.GAME_WAIT_TIME) return;
        if (gmsaccountManager.getUser() != null)
        {
            GameMenu.Show(gmsaccountManager.getUser().DisplayName);
        }
        else
        {
            Debug.LogError("GMS Account Manager is null,please login first or check internet connection");
            ShowAndroidToastMessage("Please Login First!");
            ShowAndroidToastMessage("GMS Account Manager is null!");
        }
#elif HMS_BUILD

        if (Time.timeSinceLevelLoad < Const.GAME_WAIT_TIME) return;
        GameMenu.Show(HMSAccountManager.Instance.HuaweiId.DisplayName);
#endif
    }

    public void OnLeaderboardsClick()
    {
        #if HMS_BUILD
    
         HMSLeaderboardManager.Instance.SetUserScoreShownOnLeaderboards(1);
         HMSLeaderboardManager.Instance.ShowLeaderboards();
       
        #endif
    }

    public void OnAchievementsClick()
    {
        #if HMS_BUILD

        HMSAchievementsManager.Instance.ShowAchievements();

        #endif
    }

    public void OnRemoveAdsClick()
    {
        BuyProduct(removeAds);
    }

    public void OnRestoreClick()
    {
        RestorePurchases();
    }

    private void ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }
}
