using HuaweiMobileServices.RemoteConfig;
using HuaweiMobileServices.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if HMS_BUILD
public class RemoteConfigManager : MonoBehaviour
{
    private static RemoteConfigManager _instance;
    public static RemoteConfigManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }



    private void Start()
    {
       OnRemoteConfigInit();
    }

    private void OnRemoteConfigInit()
    {
        HMSRemoteConfigManager.Instance.SetDeveloperMode(true);
        HMSRemoteConfigManager.Instance.OnFecthSuccess = OnFetchSuccess;
        HMSRemoteConfigManager.Instance.OnFecthFailure = OnFecthFailure;
        HMSRemoteConfigManager.Instance.Fetch();
    }

    private void OnFecthFailure(HMSException exception)
    {
        Debug.Log($"[RemoteConfigManager]: fetch() Failed Error Code => {exception.ErrorCode} Message => {exception.WrappedExceptionMessage + exception.WrappedCauseMessage}");
        ApplyDefaultKeys();
    }

    private void OnFetchSuccess(ConfigValues config)
    {
        Debug.Log("Fetch success");
        HMSRemoteConfigManager.Instance.Apply(config);
        Debug.Log("Config Applied");
    }

    private void ApplyDefaultKeys()
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        dictionary.Add("LevelTwoScore", 50);
        dictionary.Add("LevelTwoSpeed", 3);
        dictionary.Add("LevelThreeScore", 100);
        dictionary.Add("LevelThreeSpeed", 4);
        dictionary.Add("LevelFourScore", 150);
        dictionary.Add("LevelFourSpeed", 5);
        HMSRemoteConfigManager.Instance.ApplyDefault(dictionary);
        GetMergedAll();
    }

    public void GetMergedAll()
    {
        Dictionary<string, object> dictionary = HMSRemoteConfigManager.Instance.GetMergedAll();
        Debug.Log($"Count of Variables : {dictionary.Count}");
    }
}
#endif