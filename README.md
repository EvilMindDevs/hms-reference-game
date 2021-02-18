# Unity HMS + GMS Integrated Reference Game App 

Unity Reference Game Application with GMS + HMS integration(Unity+HMS+GMS)

Hyper-casual / agility game

## 1. GMS Integration Guideline

### 1.1 Requirements
Current prerequisites on above link for unity project on firebase:

https://firebase.google.com/docs/unity/setup#prerequisites
	


**Note:** Since this project also include hms , minimum prerequistes should be considered
for both GMS and HMS(section 2.1) together

### 1.2 Integrated GMS kits & services list :
* Google Sign-In Unity Plugin
* Firebase Cloud Messaging
* Google Admob
* Firebase Analytics 
* Firebase Crashlytics
* Firebase Remoteconfig

### 1.3 GMS Kit integration resources used :

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
	

## 2. HMS Integration Guideline

###  2.1 Requirements
* Android SDK min 21
* Net 4.x

###  Important
HMS Plugin which is going to be imported to this project supports:
* Unity version 2019 - Developed in Master Branch
	https://github.com/EvilMindDevs/hms-unity-plugin
* Unity version 2018 - Developed in 2018 Branch

###  HMS Integrated kits & services list :
* Account Kit
* Push Kit
* Ads Kit
* Analytics Kit
* Game Services
* InAppPurchase Kit
* Crash
* Remote Config


### Connect your game Huawei Mobile Services in 5 easy steps

1. Register your app at Huawei Developer
2. Import the Plugin to your Unity project
3. Configure your manifest
4. Connect your game with the HMS Managers
5. Connect the HMS Callbacks with your game

### 2.2  Register your app at Huawei Developer

#### 2.2.1 Register at [Huawei Developer](https://developer.huawei.com/consumer/en/)

![Huawei Developer](http://evil-mind.com/huawei/images/huaweiDeveloper.png "Huawei Developer")

#### 2.2.2 Create an app in AppGallery Connect.
During this step, you will create an app in AppGallery Connect (AGC) of HUAWEI Developer. When creating the app, you will need to enter the app name, app category, default language, and signing certificate fingerprint. After the app has been created, you will be able to obtain the basic configurations for the app, for example, the app ID and the CPID.

1. Sign in to Huawei Developer and click **Console**.
2. Click the HUAWEI AppGallery card and access AppGallery Connect.
3. On the **AppGallery Connect** page, click **My apps**.
4. On the displayed **My apps** page, click **New**.
5. Enter the App name, select App category (Game), and select Default language as needed.
6. Upon successful app creation, the App information page will automatically display. There you can find the App ID and CPID that are assigned by the system to your app.

#### 2.2.3 Add Package Name
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

### 2.3 - Import the plugin to your Unity Project

To import the plugin:

1. Backup your unity projects' **Plugins** & **Resources** directories
2. Download the [.unitypackage] code itself as zip from 
	https://github.com/EvilMindDevs/hms-unity-plugin
3. Extract  .zip file to a path 
4. Copy **Huawei**, **Plugins**, **Resources** directories along with meta files(.meta) from extracted path into your project's  Assets folder
5. Open your game project in Unity

***Note:*** If you face unity related unambigous problems after import, try suggestions on "Troubleshoot" part of this guide
____

### 2.4 - Configure your Manifest

In order for the plugin to work you need to add some information to your Android's Manifest. Make sure you have this information before proceeding.

* App ID. The app's unique ID.
* CPID. The developer's unique ID.
* Package Name

Get all this info from [Huawei Developer](https://developer.huawei.com/consumer/en/). Open the developers console go to My Services > HUAWEI IAP, and click on your apps name to enter the Detail page.

![Detail page](http://evil-mind.com/huawei/images/appInfo.png "Detail page")
____

#### How to configure the Manifest

1. Open Unity and choose **Huawei> App Gallery> Configure** The manifest configuration dialog will appear.

    ![Editor Tool](http://evil-mind.com/huawei/images/unityMenu.png "Editor tool")

2. Fill out the fields: AppID, CPID and package name.
3. Click Configure Manifest
    The plugin will include all the necessary information inside the Android Manifest
    * Permissions
    * Meta Data
    * Providers
And your manifest should look now like these:

``` xml
<?xml version="1.0" encoding="utf-8"?>
    <manifest xmlns:android="http://schemas.android.com/apk/res/android" package="com.unity3d.player" xmlns:tools="http://schemas.android.com/tools" android:installLocation="preferExternal">
        <supports-screens android:smallScreens="true" android:normalScreens="true" android:largeScreens="true" android:xlargeScreens="true" android:anyDensity="true" />
        <application android:theme="@style/UnityThemeSelector" android:icon="@mipmap/app_icon" android:label="@string/app_name">
        <activity android:name="com.unity3d.player.UnityPlayerActivity" android:label="@string/app_name">
            <intent-filter>
            <action android:name="android.intent.action.MAIN" />
            <category android:name="android.intent.category.LAUNCHER" />
            </intent-filter>
            <meta-data android:name="unityplayer.UnityActivity" android:value="true" />
        </activity>
        <meta-data android:name="com.huawei.hms.client.appid" android:value="appid=9999" />
        <meta-data android:name="com.huawei.hms.client.cpid" android:value="cpid=1234567890" />
        <meta-data android:name="com.huawei.hms.version" android:value="2.6.1" />
        <provider android:name="com.huawei.hms.update.provider.UpdateProvider" android:authorities="com.yourco.huawei.hms.update.provider" android:exported="false" android:grantUriPermissions="true" />
        <provider android:name="com.huawei.updatesdk.fileprovider.UpdateSdkFileProvider" android:authorities="com.yourco.huawei.updateSdk.fileProvider" android:exported="false" android:grantUriPermissions="true" />
        </application>
        <uses-permission android:name="com.huawei.appmarket.service.commondata.permission.GET_COMMON_DATA" />
        <uses-permission android:name="android.permission.REQUEST_INSTALL_PACKAGES" />
        <uses-permission android:name="android.permission.INTERNET" />
        <uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
        <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
        <uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
        <uses-permission android:name="android.permission.READ_PHONE_STATE" />
    </manifest>
```
____
### 2.5 Connect your game with any HMS Manager

In order for the plugin to work, you need to deploy the needed HMS Manager prefab inside your scene.

1. In Unity's project view, locate the plugins prefab folder
2. Drag and drop the HMS Manager to your scene

Now you need your game to call the HMS Manager from your game. You can do this by code or as a UI event. See below for further instructions.
    
#### Call the HMS by code

First, get the reference to the HMSManager

```csharp
private HMSManager hmsManager =  GameObject.Find("HMSManager").GetComponent<HMSManager>();
```
##### Account Kit (login)
Call login method in order to open the login dialog
```csharp
hmsManager.Login();
```

##### In App Purchases
You can retrieve a products information from App Gallery:
* Name
* Description
* Price

``` csharp
GetProductDetail(string productID);
```

Open the Purchase dialog by calling to BuyProduct method
```csharp
BuyProduct(string productID)
```

#### Call the HMS from your UI

1. Select you button and open the inspector
2. Find the On Click () section and drag and drop the HMS Manager object to the object selector
![On Click Event configuration](http://evil-mind.com/huawei/images/onClick.png "On Click Event configuration")
3. Select the method you want to call from the dropdown list:
    * Login
    * BuyProduct
    * GetProductDetail
    * GetPurchaseInfo
    * CheckForUpdates

If you are not sure how to do this, search the demo folder and open the sample scene.

![Sample store](http://evil-mind.com/huawei/images/demo.jpg "Sample store")
____

### 2.6 Connect the HMS Callbacks with you game
In order to receive the callback information from Huawei Mobile Services, you need to set the callbacks that will control the information retrieved from Huawei Servers.

## Kits Specification
Find below the specific information on the included functionalities in this plugin

1. Account
2. In App Purchases
3. Ads
4. Push notifications
5. Game

### Account

Official Documentation on Account Kit: [ Documentation](https://developer.huawei.com/consumer/en/doc/development/HMS-Guides/account-introduction-v4)


### In App Purchases

Official Documentation on IAP Kit: [ Documentation](https://developer.huawei.com/consumer/en/doc/development/HMS-Guides/iap-service-introduction-v4)

### Ads

Official Documentation on Ads Kit: [ Documentation](https://developer.huawei.com/consumer/en/doc/development/HMS-Guides/ads-sdk-introduction)

### Push

Official Documentation on Push Kit: [Documentation](https://developer.huawei.com/consumer/en/doc/development/HMS-Guides/push-introduction)

### Game

Official Documentation on Game Kit: [ Documentation](https://developer.huawei.com/consumer/en/doc/development/HMS-Guides/game-introduction-v4)

***Note:*** Those are java/kotlin based solutions to give you and idea of capabilities, of the kits
______

### 3 Troubleshoot
 ***unamgious unity file change issue fix suggestion ***
 If you change content of codes or files related with unity project and face an unambigious issue:
	1. Save your existing unity project 
	2. Backup  existing meta files of unity files that you have a problem on 
	3. Delete existing (.meta) metafile of it and try re-building your unity project
