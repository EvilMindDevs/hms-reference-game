using Firebase.RemoteConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


#if GMS_BUILD
public class RemoteConfigController : MonoBehaviour
{
    protected bool isFirebaseInitialized = false;
    public GameObject ball;
    void Awake()
    {
        //Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
        //    var dependencyStatus = task.Result;
        //    if (dependencyStatus == Firebase.DependencyStatus.Available)
        //    {
        //        Debug.Log("Firebase: Firebase is ready");
                // Burada Firebase kullanıma hazır olduğunu gösteriyor.
                InitializeRemoteConfig();
        //    }
        //    else
        //    {
        //        Debug.LogError(System.String.Format(
        //          "Firebase: Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        //        // Burası ise bir hata olduğunu gösteriyor.
        //    }
        //});
    }

    private void InitializeRemoteConfig()
    {
        System.Collections.Generic.Dictionary<string, object> defaults = new System.Collections.Generic.Dictionary<string, object>();
        // These are the values that are used if we haven't fetched data from the
        // server
        // yet, or if we ask for values that the server doesn't have:
        defaults.Add("christmas_background", false);
        FirebaseRemoteConfig.SetDefaults(defaults);
        Debug.Log("Firebase: RemoteConfig configured and ready!");
        isFirebaseInitialized = true;
        FetchDataAsync();
    }

    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.Find("Sphere");
        Firebase.Analytics.FirebaseAnalytics.LogEvent("custom_progress_event", "percent", 0.5f);

    }
    // Update is called once per frame
    void Update()
    {

    }

    public void RefrectProperties()
    {
        bool result = FirebaseRemoteConfig.GetValue("christmas_background").BooleanValue;
        Debug.Log("Firebase: RemoteConfig RefrectProperties christmas_background! " + result);

        Material newMat = Resources.Load("Materials/Material/NewYear", typeof(Material)) as Material;
        ball.GetComponent<MeshRenderer>().material = newMat;
    }
    // Start a fetch request.
    // FetchAsync only fetches new data if the current data is older than the provided
    // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
    // By default the timespan is 12 hours, and for production apps, this is a good
    // number. For this example though, it's set to a timespan of zero, so that
    // changes in the console will always show up immediately.
    public Task FetchDataAsync()
    {
        Debug.Log("Firebase: Fetching data...");
        Task fetchTask = FirebaseRemoteConfig.FetchAsync(
            TimeSpan.Zero);
        return fetchTask.ContinueWith(FetchComplete);
    }
    void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            Debug.Log("Firebase: Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            Debug.Log("Firebase: Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            Debug.Log("Firebase: Fetch completed successfully!");
        }

        var info = FirebaseRemoteConfig.Info;
        switch (info.LastFetchStatus)
        {
            case LastFetchStatus.Success:
                FirebaseRemoteConfig.ActivateFetched();
                Debug.Log(String.Format("Firebase: Remote data loaded and ready (last fetch time {0}).",
                                       info.FetchTime));
                RefrectProperties();
                break;
            case LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case FetchFailureReason.Error:
                        Debug.Log("Firebase: Fetch failed for unknown reason");
                        break;
                    case FetchFailureReason.Throttled:
                        Debug.Log("Firebase: Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case LastFetchStatus.Pending:
                Debug.Log("Firebase: Latest Fetch call still pending.");
                break;
        }
    }

}
#endif