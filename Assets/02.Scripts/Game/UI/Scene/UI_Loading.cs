using UnityEngine;
using System.Collections;
using TMPro;
using static Enums;
using DG.Tweening;
public class UI_Loading : UI_Base
{
    [SerializeField] private GameObject LoadingIcon;
    [SerializeField] private TextMeshProUGUI LoadingText;

    public string baseText = "Loading";
    public float interval = 0.5f;

    public float speed = 180f;
    private RectTransform iconRectTransform;


    private void Start()
    {
        Init();

        StartCoroutine(AnimateDots());

        iconRectTransform = LoadingIcon.GetComponent<RectTransform>();
        Tweener tween = iconRectTransform
            .DORotate(Vector3.forward * 180, 1f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental)
            .SetAutoKill(false);

    }


    IEnumerator AnimateDots()
    {
        int dotCount = 0;

        WaitForSeconds dotsWaitForSeconds = new WaitForSeconds(interval);

        while (true)
        {
            dotCount = (dotCount + 1) % 4;  // 0 ~ 3
            string dots = new string('.', dotCount);
            LoadingText.text = baseText + dots;
            yield return dotsWaitForSeconds;
        }
    }
}