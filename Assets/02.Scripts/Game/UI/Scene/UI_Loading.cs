using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_Loading : UI_Base
{
    enum Images
    {
        LoadBackground,
        LoadBar
    }

    public static string NextScene = "YGM_Research";
    float minWaitTime = 1.0f;
    float elapsedTime = 0f;

    private void Start()
    {
        Init();
        StartCoroutine(LoadSceneProcess());

    }
    public override void Init()
    {
        Bind<Image>(typeof(Images));

        GetImage((int)Images.LoadBar).fillAmount = 0.0f;
    }

    public static void LoadScene(string sceneName)
    {
        NextScene =  sceneName;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Ygm_LoadingScene");
    }


    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(NextScene);
        
        op.allowSceneActivation = false;

        //while(!op.isDone)
        //{
        //    yield return null;
        //    elapsedTime += Time.unscaledDeltaTime;

        //    if (op.progress <  0.9f)
        //    {
        //        GetImage((int)Images.LoadBar).fillAmount = op.progress;
        //    }
        //    else
        //    {

        //        float fill = Mathf.Lerp(0.9f, 1f, (elapsedTime - minWaitTime));
        //        GetImage((int)Images.LoadBar).fillAmount = fill;
        //        //timer += Time.unscaledDeltaTime;
        //        //GetImage((int)Images.LoadBar).fillAmount = Mathf.Lerp(0.9f, 1f, timer);

        //        if (fill > 1.0f)
        //        {
        //            op.allowSceneActivation = true;
        //            yield break;
        //        }
        //    }
        //}

        while (!op.isDone)
        {
            yield return null;
            elapsedTime += Time.unscaledDeltaTime;

            if (op.progress < 0.9f)
            {
                GetImage((int)Images.LoadBar).fillAmount = op.progress;
            }
            else
            {
                float fill = Mathf.Lerp(0.9f, 1f, (elapsedTime - minWaitTime));
                GetImage((int)Images.LoadBar).fillAmount = fill;

                if (fill >= 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }

    }


}