using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationUI : UI_Base
{
    [SerializeField] private TextMeshProUGUI NotificationTxt;

    private Sequence goodAnim;
    private Sequence badAnim;

    // 애니메이션 수치
    private float colorChangeTime = 0.6f;
    private float fadeoutTime = 2.4f;
    private float moveAmount = 100f;

    private RectTransform rect;
    private Vector3 originalPos;

    private void Awake()
    {
        rect = NotificationTxt.gameObject.GetComponent<RectTransform>();
        originalPos = rect.anchoredPosition;

        // 토스트 메세지 애니메이션 설정
        goodAnim = Util.RecyclableSequence();
        badAnim = Util.RecyclableSequence();

        goodAnim.Append(NotificationTxt.DOColor(Color.green, colorChangeTime))
                .Join(NotificationTxt.DOFade(0, fadeoutTime))
                .Join(rect.DOAnchorPosY(originalPos.y + moveAmount, fadeoutTime)).SetEase(Ease.OutQuad)
                .Append(DOVirtual.DelayedCall(0f, () => { Managers.UI.NotifyDequeue(); }));

        badAnim.Append(NotificationTxt.DOColor(Color.red, colorChangeTime))
                .Join(NotificationTxt.DOFade(0, fadeoutTime))
                .Join(rect.DOAnchorPosY(originalPos.y + moveAmount, fadeoutTime)).SetEase(Ease.OutQuad)
                .Append(DOVirtual.DelayedCall(0f, () => { Managers.UI.NotifyDequeue(); }));
    }

    public void SetMessage(string msg, bool isGreen = true)
    {
        rect.anchoredPosition = originalPos;
        NotificationTxt.color = Color.yellow;
        NotificationTxt.text = msg;


        if (!isGreen)
        {
            badAnim.Restart(); // 빨간 알림 띄우기
        }
        else
        {
            goodAnim.Restart(); // 초록 알림 띄우기
        }
    }
}
