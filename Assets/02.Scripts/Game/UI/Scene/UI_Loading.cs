using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading;

public class UI_Loading : UI_Base
{
    enum Images
    {
        LoadBackground,
        LoadBar
    }

    public static string NextScene = "YGM_Research";


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

    /// <summary>
    /// 로딩씬 로드하고 씬전환
    /// </summary>
    public static void LoadScene(string sceneName)
    {
        NextScene =  sceneName;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Ygm_LoadingScene");
    }

    /// <summary>
    /// 씬전화 로딩
    /// </summary>
    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(NextScene);

        op.allowSceneActivation = false;

        float minWaitTime = 1.0f;
        float elapsedTime = 0f;
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