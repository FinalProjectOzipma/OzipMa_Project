using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
