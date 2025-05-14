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
        yield return auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsCanceled)
            {
                Util.LogError("AuthManager : SignInAnonymouslyAsync was canceled.");
                return;
            }
            if(task.IsFaulted)
            {
                Util.LogError("AuthManager : SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            AuthResult result = task.Result;
            Managers.Data.SetUserID(result.User.UserId);
            
            Util.Log($"파베 익명 로그인 Success : {result.User.UserId} {result.User.DisplayName}");
        });
    }
}
