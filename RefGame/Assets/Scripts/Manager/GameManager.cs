using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

#if HMS_BUILD
using HmsPlugin;
#endif
using HuaweiMobileServices.Base;
using HuaweiMobileServices.Id;

public class GameManager : MonoBehaviour
{
    public delegate void GameStart();
    public static event GameStart OnGameStarted;

    public delegate void GameEnd();
    public static event GameEnd OnGameEnded;

    private string leaderboardId = "9C8A3E87D3E073F88FD3A13C224E7C686DFD17FFFBF69543D0234B8E9146762A";
    private string achievementId = "FF99BD6F84AF2A962F24233120074590818992F80350AD62052109A5BB3B8D20";

    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }


    private int score;
    public int Score
    {
        get
        {
            score = PlayerPrefs.GetInt(Const.PREF_SCORE);
            return score;
        }
        set
        {
            score = value;
            if (GameMenu.Instance != null)
                GameMenu.Instance.UpdateScore(score);
            PlayerPrefs.SetInt(Const.PREF_SCORE, score);
            int bestScore = PlayerPrefs.GetInt(Const.PREF_BEST_SCORE);
            if (score > bestScore)
                PlayerPrefs.SetInt(Const.PREF_BEST_SCORE, score);
        }
    }

    [SerializeField] private Color[] colors;
    [SerializeField] private Material platformMaterial;
    [SerializeField] private Color currentPlatformColor;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private int platformCount = 30;

    private List<GameObject> listPlatform = new List<GameObject>();
    private int positionCube = 0;
    private int spawnCount = 1;

    System.Random rand = new System.Random();
#if HMS_BUILD

    private AnalyticsManager analyticsManager;
    private InterstitialAdManager interstitalAdManager;
    private LeaderboardManager leaderboardManager;
    private AchievementsManager achievementsManager;
    private HuaweiIdAuthService authService;
#endif
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            instance = this;
            Init();
        }
    }

    IEnumerator Start()
    {
        platformMaterial.color = colors[0];
        Application.targetFrameRate = 60;
        GC.Collect();
        yield return null;
        MainMenu.Show();
        StartCoroutine(CoroutSpawnCube());
        StartCoroutine(ChangeMaterialColor());
    }

    private void Init()
    {
        int count = 0;
        PlayerPrefs.SetInt(Const.PREF_SCORE, 0);
        if (!PlayerPrefs.HasKey(Const.PREF_BEST_SCORE))
            PlayerPrefs.SetInt(Const.PREF_BEST_SCORE, 0);
        while (true)
        {
            GameObject platformObj = Instantiate(platformPrefab) as GameObject;
            platformObj.transform.SetParent(transform, true);
            platformObj.SetActive(false);
            listPlatform.Add(platformObj);
            count++;
            if (count > platformCount)
                break;
        }
#if HMS_BUILD

        analyticsManager = AnalyticsManager.GetInstance();
        interstitalAdManager = InterstitialAdManager.GetInstance();
#endif
    }

    public void OnGameStart()
    {
        OnGameStarted?.Invoke();
    }

    public void OnGameEnd()
    {
        OnGameEnded?.Invoke();
#if HMS_BUILD

        if (Score > 10)
            achievementsManager.UnlockAchievement(achievementId);
        analyticsManager.SendEventWithBundle("$Scoring", "$Score", Score);
        if (leaderboardManager == null)
            Debug.Log("Leaderboard null");
        leaderboardManager.SubmitScore(leaderboardId, Score);
        if (interstitalAdManager == null)
            Debug.Log("interstitalAdManager null");
        interstitalAdManager.ShowInterstitialAd();
#endif
    }

    public Transform SpawnPlatform(int pos)
    {
        Transform t = GetPooledPlatform();
        t.SetParent(transform, true);
        t.position = new Vector3(0, 0, 2 * spawnCount);
        positionCube = pos;
        spawnCount++;
        t.name = spawnCount.ToString();
        t.GetComponent<Cube>().DoPosition();
        return t;
    }

    private Transform GetPooledPlatform()
    {
        var p = listPlatform.Find(f => f.activeInHierarchy == false);

        if (p == null)
        {
            GameObject platformObj = Instantiate(platformPrefab) as GameObject;
            platformObj.transform.SetParent(transform, true);
            platformObj.SetActive(false);
            listPlatform.Add(platformObj);
            p = platformObj;
        }
        p.SetActive(true);
        return p.transform;
    }

    private IEnumerator CoroutSpawnCube()
    {
        while (true)
        {
            if (spawnCount < 5)
            {
                SpawnPlatform(0);
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
                if (GetRandom() == 0)
                    positionCube--;
                else
                    positionCube++;
                var t = SpawnPlatform(positionCube);
                t.eulerAngles = new Vector3(0, 0, positionCube * 45f);
            }

            while (listPlatform.FindAll(o => o.activeInHierarchy == true).Count > 20)
                yield return null;
        }
    }

    int GetRandom()
    {
        return rand.Next(0, 2);
    }

    IEnumerator ChangeMaterialColor()
    {
        yield return new WaitForSeconds(Const.COLOR_START_WAIT_TIME);
        while (true)
        {
            Color colorTemp = colors[UnityEngine.Random.Range(0, colors.Length)];
            platformMaterial.DOColor(colorTemp, Const.COLOR_TRANSITION_TIME);
            yield return new WaitForSeconds(Const.COLOR_CHANGE_TIME);
        }
    }

    private void OnDisable()
    {
        platformMaterial.color = colors[0];
    }

    private void OnEnable()
    {
        #if HMS_BUILD
        if (AccountManager.Instance != null && AccountManager.Instance.IsSignedIn)
            ArrangeManagers();
        #elif GMS_BUILD
        if (GMSAuthManager.GetInstance() != null)
        {
            ArrangeManagers();
        }
    #endif
    }

    private void OnApplicationQuit()
    {
        platformMaterial.color = colors[0];
    }

    public void ShowAds()
    {
        //TODO: Implement huawei ads here.
    }

    public void IncrementScore()
    {
        Score++;
        //if (Score > HMSRemoteConfigManager.Instance.GetValueAsLong("LevelTwoScore"))
        //    PlayerController.Instance.ChangeSpeed(HMSRemoteConfigManager.Instance.GetValueAsLong("LevelTwoSpeed"));
        //else if (Score > HMSRemoteConfigManager.Instance.GetValueAsLong("LevelThreeScore"))
        //    PlayerController.Instance.ChangeSpeed(HMSRemoteConfigManager.Instance.GetValueAsLong("LevelThreeSpeed"));
        //else if (Score > HMSRemoteConfigManager.Instance.GetValueAsLong("LevelFourScore"))
        //    PlayerController.Instance.ChangeSpeed(HMSRemoteConfigManager.Instance.GetValueAsLong("LevelFourSpeed"));

        if (Score > 50)
            PlayerController.Instance.ChangeSpeed(3);
        else if (Score > 100)
            PlayerController.Instance.ChangeSpeed(4);
        else if (Score > 150)
            PlayerController.Instance.ChangeSpeed(5);
    }

    public void ArrangeManagers()
    {
#if HMS_BUILD

        HMSGameManager.GetInstance().Init();
        achievementsManager = AchievementsManager.GetInstance();
        leaderboardManager = LeaderboardManager.GetInstance();
#endif
    }
}
