using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Enums;
public class UI_Loading : UI_Base
{
    [SerializeField] private GameObject LoadingIcon;
    [SerializeField] private TextMeshProUGUI LoadingText;
    [SerializeField] private TextMeshProUGUI TipText;

    public string baseText = "Loading";
    public float interval = 0.5f;

    public float speed = 180f;
    private RectTransform iconRectTransform;
    private Tweener rotateTween;


    private void Start()
    {
        Init();

        // TipText 메세지 랜덤 선정
        List<DefaultTable.LoadingTip> tipDatas = Util.TableConverter<DefaultTable.LoadingTip>(Managers.Data.Datas[Sheet.LoadingTip]);
        TipText.text = tipDatas[Random.Range(0, tipDatas.Count)].Message;

        // 아이콘 무한 회전
        iconRectTransform = LoadingIcon.GetComponent<RectTransform>();
        rotateTween = iconRectTransform
            .DORotate(Vector3.forward * 180, 1f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental)
            .SetAutoKill(false);

        // Loading... 텍스트 애니메이션
        StartCoroutine(AnimateDots());
    }

    private void OnDisable()
    {
        rotateTween.Kill();
        Destroy(this.gameObject);
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