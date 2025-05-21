using Firebase.Auth;
using System;
using System.Collections;

public class AuthManager
{
    public void AnonymousLogin()
    {
        Managers.StartCoroutine(AnonymousLoginCoroutine());
    }

    public IEnumerator AnonymousLoginCoroutine()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        var authTask = auth.SignInAnonymouslyAsync();

        while (!authTask.IsCompleted)
        {
            yield return null;
        }

        if (authTask.IsCanceled)
        {
            Util.LogError("AuthManager : SignInAnonymouslyAsync was canceled.");
            yield break;
        }

        if (authTask.IsFaulted)
        {
            Util.LogError("AuthManager : SignInAnonymouslyAsync encountered an error: " + authTask.Exception);
            yield break;
        }

        // 성공 처리
        AuthResult result = authTask.Result;
        Managers.Data.SetUserID(result.User.UserId);

        Util.Log($"파베 익명 로그인 Success : {result.User.UserId} {result.User.DisplayName}");

        // 애널리틱스 daily_login
        #region daily_login
        PlayerManager player = Managers.Player;
        DateTime last_loginTime = player.Last_LoginTime;
        DateTime curTime = Managers.Game.ServerUtcNow;
        TimeSpan ResultTime = curTime - last_loginTime;
        player.consecutive_days = (ResultTime.TotalDays == 1) ? player.consecutive_days + 1 : player.consecutive_days = 0;


        Managers.Analytics.AnalyticsDailyLogin(result.User.UserId, last_loginTime.ToString("yyyy-MM-dd HH:mm:ss"), curTime.ToString("yyyy-MM-dd HH:mm:ss"), 
            (float)ResultTime.TotalHours, player.consecutive_days);
        #endregion
    }
}
