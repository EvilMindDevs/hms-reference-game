using Firebase;
using Firebase.Crashlytics;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if GMS_BUILD
public class FirebaseCrashlyticsManager : MonoBehaviour
{
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    protected bool firebaseInitialized = false;

    // Start is called before the first frame update
    void Start()
    {
        //Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Debug;
      checkFirebaseDependencies();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void checkFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                Debug.Log("Check Dependencies for Firebase Crashlytics");
                dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    Debug.Log("All Firebase Crashlytics Dependency Status : Available");
                    InitializeFirebase();

                }
                else
                {
                    Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
    }

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        var app = FirebaseApp.DefaultInstance;
        firebaseInitialized = true;
        Debug.Log("InitializedFirebaseAppInstance for Crashlytics");
        //Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Debug;
    }

    public void PerformAllCrashTriggerActions()
    {
        checkFirebaseDependencies();

        WriteCustomLog("This is a log message.");
        SetCustomKey("MyKey", "TheValue");
        SetUserID("SomeUserId");
        LogCaughtException2();
        ThrowUncaughtException2();

    }

    // Causes an error that will crash the app at the platform level (Android or iOS)
    public void ThrowUncaughtException()
    {

        Debug.Log("Causing a platform crash.");
        throw new InvalidOperationException("Uncaught exception created from UI.");
    }
    public void ThrowUncaughtException2()
    {

        string original = null;
        Debug.Log("Causing a platform crash.");
        throw new ArgumentException("Parameter cannot be null", nameof(original));
    }

    // Log a caught exception.
    public void LogCaughtException()
    {
        Debug.Log("Catching an logging an exception.");
        try
        {
            throw new InvalidOperationException("This exception should be caught");
        }
        catch (Exception ex)
        {
            Crashlytics.LogException(ex);
        }
    }
    // Log a caught exception.
    public void LogCaughtException2()
    {
        Debug.Log("Catching an logging an exception.");
        try
        {
            string original = null;
            throw new ArgumentException("Parameter cannot be null", nameof(original));
        }
        catch (Exception ex)
        {
            Crashlytics.LogException(ex);
        }
    }

    // Write to the Crashlytics session log
    public void WriteCustomLog(String s)
    {
        Debug.Log("Logging message to Crashlytics session: " + s);
        Crashlytics.Log(s);
    }

    // Add custom key / value pair to Crashlytics session
    public void SetCustomKey(String key, String value)
    {
        Debug.Log("Setting Crashlytics Custom Key: <" + key + " / " + value + ">");
        Crashlytics.SetCustomKey(key, value);
    }
    // Set User Identifier for this Crashlytics session 
    public void SetUserID(String id)
    {
        Debug.Log("Setting Crashlytics user identifier: " + id);
        Crashlytics.SetUserId(id);
    }


}
#endif