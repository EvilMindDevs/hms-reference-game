# Unity HMS + GMS Integrated Reference Game App 

Unity Reference Game Application with GMS + HMS integration(Unity+HMS+GMS)

Hyper-casual / agility game


<img src="https://github.com/EvilMindDevs/hms-reference-game/blob/PluginV2/RefGame/testss%20(1).jpg" width="248">

##  GMS Integration Guideline

### 1 Requirements
Current prerequisites on above link for unity project on firebase:

https://firebase.google.com/docs/unity/setup#prerequisites
	


**Note:** Since this project also include hms , minimum prerequistes should be considered
for both GMS and HMS(section 2.1) together

### 2 Integrated GMS kits & services list :
* Google Sign-In Unity Plugin
* Firebase Cloud Messaging
* Google Admob
* Firebase Analytics 
* Firebase Crashlytics
* Firebase Remoteconfig

### 3 GMS Kit integration resources used :

*  Helper links for creating and configuring unity based firebase project:

    https://firebase.google.com/docs/unity/setup
	

*  UPM Method chosen to import  Firabase services

    ( Firebase app(core),

    Firebase authentication,
  
    Firebase cloud messaging,
  
    Firebase Crashlytics,
  
    Firebase remote config,
								
    Google Analytics for Firebase) to your unity project on unity editor which can be seen on following link:
	
    https://laptrinhx.com/managing-the-firebase-unity-plugin-the-easy-way-1600281350/



*  Google Sign-In Unity Plugin
 
   https://github.com/googlesamples/google-signin-unity
	

*  Firebase Cloud Messaging
 
   https://firebase.google.com/docs/cloud-messaging/unity/client
	

*  Google Admob
 
   https://developers.google.com/admob/unity/quick-start
	

*  Firebase Analytics 
 
   https://firebase.google.com/docs/analytics/unity/start
	

*  Firebase Crashlytics
 
   https://firebase.google.com/docs/crashlytics/get-started?platform=unity
	

*  Firebase Remoteconfig
 
   https://firebase.google.com/docs/remote-config/use-config-unity
	

## HMS Integration Guideline

**Note:** There are separate  "HMS Integration Installation"  and "Upgrade To HMS Unity Plugin VERSION 2.0" guide exist for this project.
		  "Upgrade To HMS Unity Plugin VERSION 2.0" guide should be used by who already integrated HMS Unity Plugin version 1.X on their 
		  project and want to use 2.0 version by upgrading it .


### HMS Integration Installation Guide
	For first time integration users

## Huawei Mobile Services Plugin

The HMS Unity plugin helps you integrate all the power of Huawei Mobile Services in your Unity game

Integrated HMS kits in this project:

* Huawei Account Kit
* In App purchases: Consumable, non consumables and Subscriptions.
* Ads: Interstitial, rewarded videos and banner
* Push notifications
* Game leaderboards and achievements
* Huawei Anayltics kit
* Crash Service
* Remote Config


## Requirements
Android SDK min 21
Net 4.x

## Important
This plugin supports:
* Unity version 2019, 2020 - Developed in 2.0 git branch


**Make sure to download the corresponding unity package for the Unity version you are using from the release section**

## Troubleshooting
Please check our [wiki page](https://github.com/EvilMindDevs/hms-unity-plugin/wiki/Troubleshooting)

## Status
This is an ongoing project, currently WIP. Feel free to contact us if you'd like to collaborate and use Github issues for any problems you might encounter.

### Expected soon features
* Native Ads Integration

## Connect your game Huawei Mobile Services in 5 easy steps

1. Register your app at Huawei Developer
2. Import the Plugin to your Unity project
3. Connect your game with the HMS Kit Managers

### 1 - Register your app at Huawei Developer

#### 1.1-  Register at [Huawei Developer](https://developer.huawei.com/consumer/en/)

#### 1.2 - Create an app in AppGallery Connect.
During this step, you will create an app in AppGallery Connect (AGC) of HUAWEI Developer. When creating the app, you will need to enter the app name, app category, default language, and signing certificate fingerprint. After the app has been created, you will be able to obtain the basic configurations for the app, for example, the app ID and the CPID.

1. Sign in to Huawei Developer and click **Console**.
2. Click the HUAWEI AppGallery card and access AppGallery Connect.
3. On the **AppGallery Connect** page, click **My apps**.
4. On the displayed **My apps** page, click **New**.
5. Enter the App name, select App category (Game), and select Default language as needed.
6. Upon successful app creation, the App information page will automatically display. There you can find the App ID and CPID that are assigned by the system to your app.

#### 1.3 Add Package Name
Set the package name of the created application on the AGC.

1. Open the previously created application in AGC application management and select the **Develop TAB** to pop up an entry to manually enter the package name and select **manually enter the package name**.
2. Fill in the application package name in the input box and click save.

> Your package name should end in .huawei in order to release in App Gallery

#### Generate a keystore.

Create a keystore using Unity or Android Tools. make sure your Unity project uses this keystore under the **Build Settings>PlayerSettings>Publishing settings**


#### Generate a signing certificate fingerprint.

During this step, you will need to export the SHA-256 fingerprint by using keytool provided by the JDK and signature file.

1. Open the command window or terminal and access the bin directory where the JDK is installed.
2. Run the keytool command in the bin directory to view the signature file and run the command.

    ``keytool -list -v -keystore D:\Android\WorkSpcae\HmsDemo\app\HmsDemo.jks``
3. Enter the password of the signature file keystore in the information area. The password is the password used to generate the signature file.
4. Obtain the SHA-256 fingerprint from the result. Save for next step.


#### Add fingerprint certificate to AppGallery Connect
During this step, you will configure the generated SHA-256 fingerprint in AppGallery Connect.

1. In AppGallery Connect, click the app that you have created and go to **Develop> Overview**
2. Go to the App information section and enter the SHA-256 fingerprint that you generated earlier.
3. Click √ to save the fingerprint.

____

### 2 - Import the plugin to your Unity Project

To import the plugin:

1. Download the latest [.unitypackage](https://github.com/EvilMindDevs/hms-unity-plugin/releases)
2. Open your game in Unity
3. Choose Assets> Import Package> Custom
![Import Package](http://evil-mind.com/huawei/images/importCustomPackage.png "Import package")
4. In the file explorer select the downloaded HMS Unity plugin. The Import Unity Package dialog box will appear, with all the items in the package pre-checked, ready to install.
![image](https://user-images.githubusercontent.com/6827857/113576269-e8e2ca00-9627-11eb-9948-e905be1078a4.png)
5. Select Import and Unity will deploy the Unity plugin into your Assets Folder
____

### 3 - Update your agconnect-services.json file.

In order for the plugin to work, some kits are in need of agconnect-json file. Please download your latest config file from AGC and import into Assets/StreamingAssets folder.
![image](https://user-images.githubusercontent.com/6827857/113585485-f488bd80-9634-11eb-8b1e-6d0b5e06ecf0.png)
____
### 4 - Specify your project building settings on Unity Editor .
	Configure your unity editor build settings for GMS build as follow :
            In Unity Editor File-> Build Settings -> Player Settings... -> Other Settings 
			Set "Scripting Define Symbols" as GMS_BUILD (default value: GMS_BUILD)

### 5 - Connect your game with any HMS Kit

In order for the plugin to work, you need to select the needed kits Huawei > Kit Settings.

![image](https://user-images.githubusercontent.com/6827857/113576579-7b836900-9628-11eb-89a5-724c7188c819.png)

It will automaticly create the GameObject for you and it has DontDestroyOnLoad implemented so you don't need to worry about reference being lost.

Now you need your game to call the Kit Managers from your game. See below for further instructions.
    
##### Account Kit (Sign In)
Call login method in order to open the login dialog. Be sure to have AccountKit enabled in Huawei > Kit Settings.

```csharp
HMSAccountManager.Instance.SignIn();
```

##### Analytics kit
 
1. Enable Analtics kit from AGC
2. Update ...Assets\StreamingAssets\agconnect-services.json file
 
 Send analytics function:
 
``` csharp
HMSAnalyticsManager.Instance.SendEventWithBundle(eventId, key, value);
  ```
  
##### In App Purchases
Register your products via custom editor under Huawei > Kit Settings > IAP tab.
![image](https://user-images.githubusercontent.com/6827857/113579431-f8184680-962c-11eb-9bfd-13ec69402536.png)
Write your product identifier that is in AGC and select product type.

If you check "Initialize On Start" checkbox, it'll automaticly retrieve registered products on Start.
If you want to initialize the IAP by yourself, call the function mentioned in below. You can also set callbacks as well.

``` csharp
HMSIAPManager.Instance.CheckIapAvailability();

HMSIAPManager.Instance.OnCheckIapAvailabilitySuccess += OnCheckIapAvailabilitySuccess;
HMSIAPManager.Instance.OnCheckIapAvailabilityFailure += OnCheckIapAvailabilityFailure;

private void OnCheckIapAvailabilityFailure(HMSException obj)
    {
        
    }

    private void OnCheckIapAvailabilitySuccess()
    {
        
    }
```

Open the Purchase dialog by calling to BuyProduct method. You can set callbacks and check which product was purchased.
```csharp
HMSIAPManager.Instance.BuyProduct(string productID)

HMSIAPManager.Instance.OnBuyProductSuccess += OnBuyProductSuccess;

private void OnBuyProductSuccess(PurchaseResultInfo obj)
    {
        if(obj.InAppPurchaseData.ProductId == "removeAds")
        {
            // Write your remove ads logic here.
        }
    }
```

Restore purchases that have been bought by user before.
```csharp
 HMSIAPManager.Instance.RestorePurchases((restoredProducts) =>
        {
            //restoredProducts contains all products that has been restored.
        });
```

You can also use "Create Constant Classes" button to create a class called HMSIAPConstants which will contain all products as constants and you can call it from your code. Such as;
```csharp
HMSIAPManager.Instance.BuyProduct(HMSIAPConstants.testProduct);
```

##### Ads kit
There is a custom editor in Huawei > Kit Settings > Ads tab.
![image](https://user-images.githubusercontent.com/6827857/113583224-0ae14a00-9632-11eb-83c3-a45ab2699e4f.png)

You can enable/disable ads that you want in your project.
Insert your Ad Id into these textboxes in the editor.
If you want to use test ads, you can check UseTestAds checkbox that'll overwrite all ad ids with test ads. 

Then you can call certain functions such as
```csharp
    HMSAdsKitManager.Instance.ShowBannerAd();
    HMSAdsKitManager.Instance.HideBannerAd();
    HMSAdsKitManager.Instance.ShowInterstitialAd();
    
    HMSAdsKitManager.Instance.OnRewarded = OnRewarded;
    HMSAdsKitManager.Instance.ShowRewardedAd();
    
    public void OnRewarded(Reward reward)
    {
       
    }
```

##### Game kit
There is a custom editor in Huawei > Kit Settings > Game Service tab.
![image](https://user-images.githubusercontent.com/6827857/113587013-e6d43780-9636-11eb-8621-8fc4d0fdb433.png)

You can also use "Create Constant Classes" button to create a class called HMSLeaderboardConstants or HMSAchievementConstants which will contain all achievements and leaderboards as constants and you can call it from your code. Such as;
```csharp
    HMSLeaderboardManager.Instance.ReportScore(HMSLeaderboardConstants.topleaderboard,50);
    HMSAchievementsManager.Instance.RevealAchievement(HMSAchievementConstants.firstshot);
```

You can call native calls to list achievements or leaderboards.
```csharp
  HMSAchievementsManager.Instance.ShowAchievements();
  HMSLeaderboardManager.Instance.ShowLeaderboards();
```

## Kits Specification
Find below the specific information on the included functionalities in this plugin

1. Account
2. In App Purchases
3. Ads
4. Push notifications
5. Game
6. Analytics
7. Remote Config
8. Crash
9. Cloud DB

### Account

Official Documentation on Account Kit: [ Documentation](https://developer.huawei.com/consumer/en/doc/development/HMSCore-Guides/introduction-0000001050048870)

### In App Purchases

Official Documentation on IAP Kit: [ Documentation](https://developer.huawei.com/consumer/en/doc/development/HMSCore-Guides/introduction-0000001050033062)

### Ads

Official Documentation on Ads Kit: [ Documentation](https://developer.huawei.com/consumer/en/doc/development/HMSCore-Guides/publisher-service-introduction-0000001070671805)

### Push

Official Documentation on Push Kit: [Documentation](https://developer.huawei.com/consumer/en/doc/development/HMSCore-Guides/service-introduction-0000001050040060)

### Game

Official Documentation on Game Kit: [ Documentation](https://developer.huawei.com/consumer/en/doc/development/HMSCore-Guides/introduction-0000001050121216)

### Analytics

Official Documentation on Analytics Kit: [ Documentation](https://developer.huawei.com/consumer/en/doc/development/HMSCore-Guides/introduction-0000001050745149)

### Remote Config

Official Documentation on Analytics Kit: [ Documentation](https://developer.huawei.com/consumer/en/doc/development/AppGallery-connect-Guides/agc-get-started)

### Crash

Official Documentation on Analytics Kit: [ Documentation](https://developer.huawei.com/consumer/en/doc/development/AppGallery-connect-Guides/agc-crash-getstarted-0000001055260538)

### Cloud DB

Official Documentation on Analytics Kit: [ Documentation](https://developer.huawei.com/consumer/en/doc/development/AppGallery-connect-Guides/agc-clouddb-introduction)

## Upgrade To HMS Unity Plugin VERSION 2.0 Guide
	For users who already integrated HMS Unity Plugin Project's previous 1.X versions to their project and want to upgrade to HMS Unity Plugin version 2.0
	
		Here is some highlights about new plugin2.0, 

		•	Reduce integration time and risk of integration issues
		•	Minimum line of codes < almost codeless development experience> 
		•	Added new custom Unity editors for easy implementation of kits ( that'll help junior developers to integrate Huawei Services into projects with few clicks ) 
		•	Optimize and support latest HMS kit version
		•	Gradle build for smooth procedure
		•	Remove library files inside unity project
		•   InAppPurchaseData elements can be separately maintained(instead of as a whole in 1.X versions)

	Follow these steps to upgrade your existing HMS Unity Plugin Integrated game project to 2.0 version:
	Step 1. Backup your existing project
	
	Step 2. Delete Huawei Directory in your project
	
	Step 3. Download HMS Unity Plugin version 2 [.unitypackage](https://github.com/EvilMindDevs/hms-unity-plugin/releases/tag/2.0)
	
	Step 4. Change hms unity plugin manager class names(from 1.0 version) that you already use in your project with their HMS-prefixed counterparts exist in 2.0 version as follow
	(Reason:Manager Class names changed to more HMS specific ones on 2.0 and singleton fix included)
		achievementsManager  --> HMSAchievementsManager
		analyticsManager     --> HMSAnalyticsManager
		leaderboardManager   --> HMSLeaderboardManager
		interstitalAdManager --> HMSAdsKitManager
		rewardAdManager      --> HMSAdsKitManager
		AccountManager       --> HMSAccountManager
		AuthHuaweiId         --> AuthAccount
		iapManager           --> HMSIAPManager
		...
		
    Step 5. Singleton fix in 2.0 will allow null check removal & easier instance call .So if you have  implementations as below examples, change accordingly.
		5.1 null check removal example 
			private AccountManager accountManager;
			if (accountManager != null)
			{
				accountManager.SignIn();                     ==>       HMSAccountManager.Instance.SignIn();
			}
			else
			{
				Debug.LogError("Account Manager is null");
			}
		5.2 Instance call example 
				private IapManager iapManager;
				iapManager = IapManager.GetInstance();       ==>       HMSIAPManager.Instance.CheckIapAvailability();
				iapManager.CheckIapAvailability();
				
	Step 6.  In 2.0 , IAP Constants Class added so use it  instead of  class-local constant definition/usage
	
	private string removeAds = "com.samet.reffapp.huawei.removeads";  
	BuyProduct(removeAds);									   			==>		BuyProduct(HMSIAPConstants.comsametreffapphuaweiremoveads);
	
	Step 7. Remove all .aar & .aar.meta, .jar, manifest files of previous HMS Unity Plugin (1.X) versions on the path: Assets\Plugins\Android
	
	Step 8. Remove unnecessary agconnect-services.json files . Only Assets\StreamingAssets  path should include "agconnect-services.json" file
	(ex: \Assets\Plugins\Android\assets  or \Assets\Huawei path can include such not necessary-anymore agconnect-json file)
	
	Step 9.  Remove 1.0 version prefabs of the hms-based managers existing in your scenes if you use any in your project
	
	Step 10. Configure your unity editor build settings for HMS build as follow :
            In Unity Editor File-> Build Settings -> Player Settings... -> Other Settings 
			Set  "Scripting Define Symbols" as HMS_BUILD (default is GMS_BUILD)
______

## License

This project is licensed under the MIT License
______

###  Troubleshoot
 ***unamgious unity file change issue fix suggestion ***
 If you change content of codes or files related with unity project and face an unambigious issue:
	1. Save your existing unity project 
	2. Backup  existing meta files of unity files that you have a problem on 
	3. Delete existing (.meta) metafile of it and try re-building your unity project
______



## License

This project is licensed under the MIT License
