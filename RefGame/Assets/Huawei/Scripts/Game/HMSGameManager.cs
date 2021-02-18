using HuaweiMobileServices.Base;
using HuaweiMobileServices.Game;
using HuaweiMobileServices.Id;
using HuaweiMobileServices.Utils;
using System;
using UnityEngine;

#if HMS_BUILD

namespace HmsPlugin
{
    public class HMSGameManager : MonoBehaviour
    {
        public static HMSGameManager GetInstance(string name = "HMSGameManager") => GameObject.Find(name).GetComponent<HMSGameManager>();

        private AccountManager accountManager;
        private SaveGameManager saveGameManager;
        private LeaderboardManager leaderboardManager;
        private AchievementsManager achievementsManager;

        public Action<Player> OnGetPlayerInfoSuccess { get; set; }
        public Action<HMSException> OnGetPlayerInfoFailure { get; set; }
        public Action<AuthHuaweiId> SignInSuccess { get; set; }
        public Action<HMSException> SignInFailure { get; set; }

        private HuaweiIdAuthService authService;

        public void Init()
        {
            Debug.Log("HMS GAMES: Game init");
            HuaweiMobileServicesUtil.SetApplication();
            accountManager = AccountManager.Instance;
            saveGameManager = SaveGameManager.GetInstance();
            leaderboardManager = LeaderboardManager.GetInstance();
            achievementsManager = AchievementsManager.GetInstance();

            Debug.Log("HMS GAMES init");
            //authService = accountManager.GetGameAuthService();

            //ITask<AuthHuaweiId> taskAuthHuaweiId = authService.SilentSignIn();
            //taskAuthHuaweiId.AddOnSuccessListener((result) =>
            //{
            //accountManager.HuaweiId = result;
            Debug.Log("HMS GAMES: Setted app");
            IJosAppsClient josAppsClient = JosApps.GetJosAppsClient(accountManager.HuaweiId);
            Debug.Log("HMS GAMES: jossClient");
            josAppsClient.Init();
            Debug.Log("HMS GAMES: jossClient init");
            InitGameManagers();

            //}).AddOnFailureListener((exception) =>
            //{
            //    Debug.Log("HMS GAMES: The app has not been authorized");
            //    authService.StartSignIn(SignInSuccess, SignInFailure);
            //    InitGameManagers();
            //});
        }

        public void InitGameManagers()
        {
            leaderboardManager.rankingsClient = Games.GetRankingsClient(accountManager.HuaweiId);
            achievementsManager.achievementsClient = Games.GetAchievementsClient(accountManager.HuaweiId);
        }

        public void GetPlayerInfo()
        {
            if (accountManager.HuaweiId != null)
            {
                IPlayersClient playersClient = Games.GetPlayersClient(accountManager.HuaweiId);
                ITask<Player> task = playersClient.CurrentPlayer;
                task.AddOnSuccessListener((result) =>
                {
                    Debug.Log("[HMSP:] GetPlayerInfo Success");
                    OnGetPlayerInfoSuccess?.Invoke(result);

                }).AddOnFailureListener((exception) =>
                {
                    Debug.Log("[HMSP:] GetPlayerInfo Failed");
                    OnGetPlayerInfoFailure?.Invoke(exception);
                });
            }
        }
    }
}

#endif