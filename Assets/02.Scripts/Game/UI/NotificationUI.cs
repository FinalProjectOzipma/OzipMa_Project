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

    private float colorChangeTime = 0.5f;
    private float fadeoutTime = 3f;

    private void Awake()
    {
        // 토스트 메세지 애니메이션 설정
        goodAnim = Util.RecyclableSequence();
        badAnim = Util.RecyclableSequence();

        goodAnim.Append(NotificationTxt.DOColor(Color.green, colorChangeTime));
        goodAnim.Join(NotificationTxt.DOFade(255, colorChangeTime));
        goodAnim.Append(NotificationTxt.DOFade(0, fadeoutTime));
        goodAnim.Append(DOVirtual.DelayedCall(0f, () => { Managers.Resource.Destroy(this.gameObject); }));

        badAnim.Append(NotificationTxt.DOColor(Color.red, colorChangeTime));
        badAnim.Join(NotificationTxt.DOFade(255, colorChangeTime));
        badAnim.Append(NotificationTxt.DOFade(0, fadeoutTime));
        badAnim.Append(DOVirtual.DelayedCall(0f, () => { Managers.Resource.Destroy(this.gameObject); }));
    }

    /// <summary>
    /// 타워 설치 개수가 변경될 때마다 실행
    /// </summary>
    /// <param name="curCnt">현재 개수</param>
    /// <param name="maxCnt">최대 개수</param>
    public void SetLimit(int curCnt, int maxCnt)
    {
        NotificationTxt.color = Color.yellow;

        if (curCnt > maxCnt)
        {
            if (goodAnim.IsPlaying()) goodAnim.Pause();
            badAnim.Restart();

            NotificationTxt.text = $"타워 설치 제한 {curCnt} / {maxCnt}";
        }
        else
        {
            if (badAnim.IsPlaying()) badAnim.Pause();
            goodAnim.Restart();

            NotificationTxt.text = $"타워 설치됨 {curCnt} / {maxCnt}";
        }
    }

    public void SetMessage(string msg, bool isGreen = true)
    {
        NotificationTxt.color = Color.yellow;
        NotificationTxt.text = msg;

        if (!isGreen)
        {
            if (goodAnim.IsPlaying()) goodAnim.Pause();
            badAnim.Restart(); // 빨간 알림 띄우기
        }
        else
        {
            if (badAnim.IsPlaying()) badAnim.Pause();
            goodAnim.Restart(); // 초록 알림 띄우기
        }
    }
}
