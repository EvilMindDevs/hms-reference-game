using Firebase.Analytics;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

#if GMS_BUILD
public class GMSAuthManager : MonoBehaviour
{
    public static GMSAuthManager GetInstance(string name = "GMSAuthManager") => GameObject.Find(name).GetComponent<GMSAuthManager>();
    private FirebaseAuth auth;
    protected Firebase.Auth.FirebaseUser user2 = null;
    protected Firebase.Auth.FirebaseAuth otherAuth;

    private string webClientId = "875790264116-9plfbf75iol0k58lfejiup3h9dclh641.apps.googleusercontent.com";
    private GoogleSignInConfiguration configuration;
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth =
      new Dictionary<string, Firebase.Auth.FirebaseUser>();
    //public static bool signInStatus=false;



    public GUISkin fb_GUISkin;
    protected string displayName = "";
    protected string email = "";
    protected string password = "";
    protected string phoneNumber = "";
    protected string receivedCode = "";
    private string logText = "";
    const int kMaxLogSize = 16382;
    // Whether to sign in / link or reauthentication *and* fetch user profile data.
    protected bool signInAndFetchProfile = false;
    // in IdTokenChanged() when the user presses the get token button.
    private bool fetchingToken = false;
    // iOS simulators.
    public bool usePasswordInput = false;
    private Vector2 controlsScrollViewVector = Vector2.zero;
    private Vector2 scrollViewVector = Vector2.zero;


    bool UIEnabled = true;
    // Options used to setup secondary authentication object.
    private Firebase.AppOptions otherAuthOptions = new Firebase.AppOptions
    {
        ApiKey = "",
        AppId = "",
        ProjectId = ""
    };


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(
                "Firebase CheckAndFixDependenciesAsync: ");
        //Firebase begins
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                //InitializeFirebase();
                auth = FirebaseAuth.DefaultInstance;
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
        //Firebase ends

    }

    private void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestIdToken = true,
            RequestEmail = true
        };
        //CheckFirebaseDependencies();
        Debug.Log(String.Format("webclientid = {0}, requestidtoken = {1}, requestemail = {2}, accountname={3}, requestauthcode ={4}, usegamesignin={5}",
            configuration.WebClientId, configuration.RequestIdToken, configuration.RequestEmail, configuration.AccountName, configuration.RequestAuthCode,
            configuration.UseGameSignIn));
    }
    //private void CheckFirebaseDependencies()
    //{
    //    Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
    //    {
    //        if (task.IsCompleted)
    //        {
    //            var dependencyStatus = task.Result;
    //            if (dependencyStatus == Firebase.DependencyStatus.Available)
    //            {
    //                // Create and hold a reference to your FirebaseApp,
    //                // where app is a Firebase.FirebaseApp property of your application class.
    //                auth = FirebaseAuth.DefaultInstance;
    //                Debug.Log(String.Format("dependencyStatus1 = {0}", dependencyStatus));
    //                // Set a flag here to indicate whether Firebase is ready to use by your app.
    //            }
    //            else
    //            {
    //                //UnityEngine.Debug.LogError(System.String.Format(
    //                //"Could not resolve all Firebase dependencies: {0}", dependencyStatus));
    //                Debug.LogError("Could not resolve all Firebase dependencies:" + task.Result.ToString());
    //                // Firebase Unity SDK is not safe to use here.
    //            }
    //        }
    //        else
    //        {
    //            Debug.LogError("Dependency check was not completed.Error: " + task.Exception.Message);
    //        }
    //    });
    //}
    public void Init()
    {
        Debug.Log("GMS GAMES: Game init");
        //auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        //InitializeFirebase();
    }

    // Update is called once per frame
    void Update()
    {

    }
    // Handle initialization of the necessary firebase modules:
    public void InitializeFirebase()
    {
        //Enabling Firebase Analytics begin 
        //Debug.Log("Enabling data collection.");
        //FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        //Debug.Log("Set user properties.");
        //// Set the user's sign up method.
        //FirebaseAnalytics.SetUserProperty(
        //  FirebaseAnalytics.UserPropertySignUpMethod,
        //  "Google");
        //// Set the user ID.
        //FirebaseAnalytics.SetUserId("uber_user_510");
        //// Set default session duration values.
        //FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));
        //firebaseInitialized = true;
        ////Enabling Firebase Analytics end
        //Enabling Firebase Auth begin 
        Debug.Log("Setting up Firebase Auth");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        auth.IdTokenChanged += IdTokenChanged;
        // Specify valid options to construct a secondary authentication object.
        if (otherAuthOptions != null &&
            !(String.IsNullOrEmpty(otherAuthOptions.ApiKey) ||
              String.IsNullOrEmpty(otherAuthOptions.AppId) ||
              String.IsNullOrEmpty(otherAuthOptions.ProjectId)))
        {
            try
            {
                otherAuth = Firebase.Auth.FirebaseAuth.GetAuth(Firebase.FirebaseApp.Create(
                  otherAuthOptions, "Secondary"));
                otherAuth.StateChanged += AuthStateChanged;
                otherAuth.IdTokenChanged += IdTokenChanged;
            }
            catch (Exception)
            {
                Debug.Log("ERROR: Failed to initialize secondary authentication object.");
                //DebugLog("ERROR: Failed to initialize secondary authentication object.");
            }
        }
        AuthStateChanged(this, null);
        //Enabling Firebase Auth end 
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
        Firebase.Auth.FirebaseUser user = null;
        if (senderAuth != null) userByAuth.TryGetValue(senderAuth.App.Name, out user);
        if (senderAuth == auth && senderAuth.CurrentUser != user)
        {
            bool signedIn = user != senderAuth.CurrentUser && senderAuth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = senderAuth.CurrentUser;
            userByAuth[senderAuth.App.Name] = user;
            if (signedIn)
            {
                Debug.Log("AuthStateChanged Signed in " + user.UserId);
                displayName = user.DisplayName ?? "";
                DisplayDetailedUserInfo(user, 1);
            }
        }
    }

    // Track ID token changes.
    void IdTokenChanged(object sender, System.EventArgs eventArgs)
    {
        Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
        if (senderAuth == auth && senderAuth.CurrentUser != null && !fetchingToken)
        {
            senderAuth.CurrentUser.TokenAsync(false).ContinueWithOnMainThread(
              task => Debug.Log(String.Format("Token[0:8] = {0}", task.Result.Substring(0, 8))));
        }
    }

    // Display a more detailed view of a FirebaseUser.
    protected void DisplayDetailedUserInfo(Firebase.Auth.FirebaseUser user, int indentLevel)
    {
        string indent = new String(' ', indentLevel * 2);
        DisplayUserInfo(user, indentLevel);
        Debug.Log(String.Format("{0}Anonymous: {1}", indent, user.IsAnonymous));
        Debug.Log(String.Format("{0}Email Verified: {1}", indent, user.IsEmailVerified));
        Debug.Log(String.Format("{0}Phone Number: {1}", indent, user.PhoneNumber));
        var providerDataList = new List<Firebase.Auth.IUserInfo>(user.ProviderData);
        var numberOfProviders = providerDataList.Count;
        if (numberOfProviders > 0)
        {
            for (int i = 0; i < numberOfProviders; ++i)
            {
                Debug.Log(String.Format("{0}Provider Data: {1}", indent, i));
                DisplayUserInfo(providerDataList[i], indentLevel + 2);
            }
        }
    }

    // Display user information.
    protected void DisplayUserInfo(Firebase.Auth.IUserInfo userInfo, int indentLevel)
    {
        string indent = new String(' ', indentLevel * 2);
        var userProperties = new Dictionary<string, string> {
        {"Display Name", userInfo.DisplayName},
        {"Email", userInfo.Email},
        {"Photo URL", userInfo.PhotoUrl != null ? userInfo.PhotoUrl.ToString() : null},
        {"Provider ID", userInfo.ProviderId},
        {"User ID", userInfo.UserId}
      };
        foreach (var property in userProperties)
        {
            if (!String.IsNullOrEmpty(property.Value))
            {
                Debug.Log(String.Format("{0}{1}: {2}", indent, property.Key, property.Value));
            }
        }
    }

    // Create a user with the email and password.
    public Task CreateUserWithEmailAsync()
    {
        Debug.Log(String.Format("Attempting to create user {0}...", email));
        DisableUI();

        // This passes the current displayName through to HandleCreateUserAsync
        // so that it can be passed to UpdateUserProfile().  displayName will be
        // reset by AuthStateChanged() when the new user is created and signed in.
        string newDisplayName = displayName;
        return auth.CreateUserWithEmailAndPasswordAsync(email, password)
          .ContinueWithOnMainThread((task) =>
          {
              EnableUI();
              if (LogTaskCompletion(task, "User Creation"))
              {
                  var user = task.Result;
                  DisplayDetailedUserInfo(user, 1);
                  return UpdateUserProfileAsync(newDisplayName: newDisplayName);
              }
              return task;
          }).Unwrap();
    }

    void DisableUI()
    {
        UIEnabled = false;
    }
    void EnableUI()
    {
        UIEnabled = true;
    }

    // Update the user's display name with the currently selected display name.
    public Task UpdateUserProfileAsync(string newDisplayName = null)
    {
        if (auth.CurrentUser == null)
        {
            Debug.Log("Not signed in, unable to update user profile");
            return Task.FromResult(0);
        }
        displayName = newDisplayName ?? displayName;
        Debug.Log("Updating user profile");
        DisableUI();
        return auth.CurrentUser.UpdateUserProfileAsync(new Firebase.Auth.UserProfile
        {
            DisplayName = displayName,
            PhotoUrl = auth.CurrentUser.PhotoUrl,
        }).ContinueWithOnMainThread(task =>
        {
            EnableUI();
            if (LogTaskCompletion(task, "User profile"))
            {
                DisplayDetailedUserInfo(auth.CurrentUser, 1);
            }
        });
    }

    // Log the result of the specified task, returning true if the task
    // completed successfully, false otherwise.
    protected bool LogTaskCompletion(Task task, string operation)
    {
        bool complete = false;
        if (task.IsCanceled)
        {
            Debug.Log(operation + " canceled.");
        }
        else if (task.IsFaulted)
        {
            Debug.Log(operation + " encounted an error.");
            foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
            {
                string authErrorCode = "";
                Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                if (firebaseEx != null)
                {
                    authErrorCode = String.Format("AuthError.{0}: ",
                      ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
                }
                Debug.Log(authErrorCode + exception.ToString());
            }
        }
        else if (task.IsCompleted)
        {
            Debug.Log(operation + " completed");
            complete = true;
        }
        return complete;
    }

    // Sign-in with an email and password.
    public Task SigninWithEmailAsync()
    {
        Debug.Log(String.Format("Attempting to sign in as {0}...", email));
        DisableUI();
        if (signInAndFetchProfile)
        {
            return auth.SignInAndRetrieveDataWithCredentialAsync(
              Firebase.Auth.EmailAuthProvider.GetCredential(email, password)).ContinueWithOnMainThread(
                HandleSignInWithSignInResult);
        }
        else
        {
            return auth.SignInWithEmailAndPasswordAsync(email, password)
              .ContinueWithOnMainThread(HandleSignInWithUser);
        }
    }
    public void SignInWithGoogle() { OnGoogleSignIn(); }
    public void SignOutFromGoogle() { OnGoogleSignOut(); }
    public void OnGoogleSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        Debug.Log("Google signin parameter initialization");
        //Debug.Log(String.Format("webclientid2 = {0}, requestidtoken2 = {1}, requestemail2 = {2}, accountname2={3}, requestauthcode2 ={4}, usegamesignin2={5}",
        //    configuration.WebClientId, configuration.RequestIdToken, configuration.RequestEmail, configuration.AccountName, configuration.RequestAuthCode,
        //    configuration.UseGameSignIn));

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished);

    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<System.Exception> enumerator =
                    task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error =
                            (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.LogError("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.LogError("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.LogError("Canceled");
        }
        else
        {
            Debug.Log("Welcome: " + task.Result.DisplayName + "!");
            Debug.Log(" Email: " + task.Result.Email);
            Debug.Log(" Google ID Token: " + task.Result.IdToken);
            SignInWithGoogleOnFirebase(task.Result.IdToken);
        }
    }
    public void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential =
    Firebase.Auth.GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });

    }

    public void OnGoogleSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        Debug.Log("Calling Google SignIn Silently");


        GoogleSignIn.DefaultInstance.SignInSilently()
             .ContinueWith(OnAuthenticationFinished);
    }

    public void OnGoogleGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        //AddStatusText("Calling Games SignIn");
        Debug.Log("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished);
    }

    // Called when a sign-in with profile data completes.
    void HandleSignInWithSignInResult(Task<Firebase.Auth.SignInResult> task)
    {
        EnableUI();
        if (LogTaskCompletion(task, "Sign-in"))
        {
            DisplaySignInResult(task.Result, 1);
        }
    }

    // Called when a sign-in without fetching profile data completes.
    void HandleSignInWithUser(Task<Firebase.Auth.FirebaseUser> task)
    {
        EnableUI();
        if (LogTaskCompletion(task, "Sign-in"))
        {
            Debug.Log(String.Format("{0} signed in", task.Result.DisplayName));
        }
    }

    // Display user information reported
    protected void DisplaySignInResult(Firebase.Auth.SignInResult result, int indentLevel)
    {
        string indent = new String(' ', indentLevel * 2);
        DisplayDetailedUserInfo(result.User, indentLevel);
        var metadata = result.Meta;
        if (metadata != null)
        {
            Debug.Log(String.Format("{0}Created: {1}", indent, metadata.CreationTimestamp));
            Debug.Log(String.Format("{0}Last Sign-in: {1}", indent, metadata.LastSignInTimestamp));
        }
        var info = result.Info;
        if (info != null)
        {
            Debug.Log(String.Format("{0}Additional User Info:", indent));
            Debug.Log(String.Format("{0}  User Name: {1}", indent, info.UserName));
            Debug.Log(String.Format("{0}  Provider ID: {1}", indent, info.ProviderId));
            DisplayProfile<string>(info.Profile, indentLevel + 1);
        }
    }

    // Display additional user profile information.
    protected void DisplayProfile<T>(IDictionary<T, object> profile, int indentLevel)
    {
        string indent = new String(' ', indentLevel * 2);
        foreach (var kv in profile)
        {
            var valueDictionary = kv.Value as IDictionary<object, object>;
            if (valueDictionary != null)
            {
                Debug.Log(String.Format("{0}{1}:", indent, kv.Key));
                DisplayProfile<object>(valueDictionary, indentLevel + 1);
            }
            else
            {
                Debug.Log(String.Format("{0}{1}: {2}", indent, kv.Key, kv.Value));
            }
        }
    }

    // Sign out the current user.
    protected void SignOut()
    {
        Debug.Log("Signing out.");
        auth.SignOut();
    }
    public void OnGoogleSignOut()
    {
        Debug.Log(" Google Sign Out called");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public bool IsSignedIn()
    {
        if (auth != null)
        {
            user2 = auth.CurrentUser;
            if (user2 != null)
            {
                string name = user2.DisplayName;

                string email = user2.Email;
                System.Uri photo_url = user2.PhotoUrl;
                // The user's Id, unique to the Firebase project.
                // Do NOT use this value to authenticate with your backend     server, if you
                // have one; use User.TokenAsync() instead.
                string uid = user2.UserId;
                Debug.Log(String.Format("{0} signed in ,email info:{1},uid:{2}", name, email, uid));
                return true;
            }
            else
            {
                Debug.LogError("signin failed");
                ShowAndroidToastMessage("signin failed");
                return false;
            }
        }
        else
        {
            Debug.LogError("auth empty");
            return false;
        }

    }

    public Firebase.Auth.FirebaseUser getUser()
    {
        return auth.CurrentUser;
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
#endif