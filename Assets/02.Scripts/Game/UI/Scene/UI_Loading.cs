using UnityEngine;
using System.Collections;
using TMPro;
using static Enums;
public class UI_Loading : UI_Base
{
    enum LoadObject
    {
        LoadingIcon
    }

    enum Texts
    {
        LoadingText
    }

    GameObject loadingObject;
    TextMeshProUGUI loadingText;
    public string baseText = "Loading";
    public float interval = 0.5f;

    public float speed = 180f;
    private bool spinning = true;


    private void Start()
    {
        Init();
        LoateIcon();

        StartCoroutine(AnimateDots());
        StartCoroutine(Spin());
        //StartCoroutine(LoadSceneProcess());

    }

    public override void Init()
    {
        Bind<GameObject>(typeof(LoadObject));
        Bind<TextMeshProUGUI>(typeof(Texts));

        loadingObject = GetObject((int)LoadObject.LoadingIcon).gameObject;
        loadingText = Get<TextMeshProUGUI>((int)Texts.LoadingText);


    }

    public void LoateIcon()
    { 

    }


    IEnumerator AnimateDots()
    {
        int dotCount = 0;

        while (true)
        {
            dotCount = (dotCount + 1) % 4;  // 0 ~ 3
            string dots = new string('.', dotCount);
            loadingText.text = baseText + dots;
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator Spin()
    {
        while (spinning)
        {
            loadingObject.transform.Rotate(0f, 0f, -speed * Time.deltaTime);
            yield return null;
        }
    }


}