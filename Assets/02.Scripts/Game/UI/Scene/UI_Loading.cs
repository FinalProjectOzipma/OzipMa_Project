using UnityEngine;
using System.Collections;
using TMPro;
using static Enums;
public class UI_Loading : UI_Base
{
    [SerializeField] private GameObject LoadingIcon;
    [SerializeField] private TextMeshProUGUI LoadingText;

    GameObject loadingObject;
    TextMeshProUGUI textEffect;
    public string baseText = "Loading";
    public float interval = 0.5f;

    public float speed = 180f;
    private bool spinning = true;


    private void Start()
    {
        Init();

        StartCoroutine(AnimateDots());
        StartCoroutine(Spin());
        //StartCoroutine(LoadSceneProcess());

    }


    IEnumerator AnimateDots()
    {
        int dotCount = 0;

        while (true)
        {
            dotCount = (dotCount + 1) % 4;  // 0 ~ 3
            string dots = new string('.', dotCount);
            LoadingText.text = baseText + dots;
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator Spin()
    {
        while (spinning)
        {
            LoadingIcon.transform.Rotate(0f, 0f, -speed * Time.deltaTime);
            yield return null;
        }
    }


}