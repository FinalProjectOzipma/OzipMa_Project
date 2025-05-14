using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerEditUI : UI_Base
{
    [SerializeField] private TextMeshProUGUI TowerLimitTxt;

    private Sequence goodAnim; 
    private Sequence badAnim;

    private float colorChangeTime = 0.5f;
    private float fadeoutTime = 3f;

    private void Start()
    {
        // 토스트 메세지 애니메이션 설정
        goodAnim = Util.RecyclableSequence();
        badAnim = Util.RecyclableSequence();

        goodAnim.Append(TowerLimitTxt.DOColor(Color.green, colorChangeTime));
        goodAnim.Join(TowerLimitTxt.DOFade(255, colorChangeTime));
        goodAnim.Append(TowerLimitTxt.DOFade(0, fadeoutTime));

        badAnim.Append(TowerLimitTxt.DOColor(Color.red, colorChangeTime));
        badAnim.Join(TowerLimitTxt.DOFade(255, colorChangeTime));
        badAnim.Append(TowerLimitTxt.DOFade(0, fadeoutTime));


        // 설치 개수 메세지 UI 구독
        BuildingSystem.Instance.OnTowerCountChanged += SetLimit;
    }

    /// <summary>
    /// 타워 설치 개수가 변경될 때마다 실행
    /// </summary>
    /// <param name="curCnt">현재 개수</param>
    /// <param name="maxCnt">최대 개수</param>
    public void SetLimit(int curCnt, int maxCnt)
    {
        TowerLimitTxt.color = Color.yellow;

        if (curCnt > maxCnt)
        {
            if(goodAnim.IsPlaying()) goodAnim.Pause();
            badAnim.Restart();
            
            TowerLimitTxt.text = $"타워 설치 제한 {curCnt} / {maxCnt}";
        }
        else
        {
            if (badAnim.IsPlaying()) badAnim.Pause();
            goodAnim.Restart();

            TowerLimitTxt.text = $"타워 설치됨 {curCnt} / {maxCnt}";
        }
    }
}
