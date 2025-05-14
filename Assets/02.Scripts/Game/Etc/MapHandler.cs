using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    public GameObject MainProps;
    public GameObject BossProps;
    public GameObject BossGate;

    private List<SpriteRenderer> curHighlights = new();
    private Sequence dotSeq; // DOTween 시퀀스
    private Vector3 gateUpPos = new Vector3(0f, 5f, 1f);
    private float gateDownOffset = -1f;

    // 배치 구역 표시 관련
    public List<Vector3Int> BuildHighlightList = new List<Vector3Int>(); // 맵마다 인스펙터창에서 설정해줄 애
    private string buildHighlightOrigin = "BuildHighlight";
    private int curHighlight_index = 0;
    private bool isHighlightOn = false;

    private void Start()
    {
        // 보스Gate 애니메이션 시퀀스 등록
        dotSeq = Util.RecyclableSequence();
        dotSeq.Append(BossGate.transform.DOLocalMoveY(gateDownOffset, 0.3f));
        dotSeq.Append(BossGate.transform.DOScale(1.1f, 0.6f));
        dotSeq.Append(BossGate.transform.DOScale(1f, 0.6f));

        dotSeq.Append(BossGate.transform.DOScale(5f, 0.4f));
        dotSeq.Append(BossGate.transform.DOScale(1f, 0f));
        dotSeq.Append(BossGate.transform.DOLocalMoveY(gateDownOffset * 10, 0f));

        // 배치 구역 Highlight 오브젝트 미리 생성해두기
        for (int i = 0; i < BuildHighlightList.Count; i++)
        {
            Managers.Resource.Instantiate(buildHighlightOrigin, obj =>
            {
                curHighlights.Add(obj.GetComponentInChildren<SpriteRenderer>());
                obj.SetActive(false);
            });
        }

        // 보스맵 전환 함수 구독시키기
        Managers.Wave.OnStartBossMap += StartBossMap;
        Managers.Wave.OnEndBossMap += EndBossMap;
    }

    /// <summary>
    /// 1개의 Highlight를 켜줌
    /// </summary>
    /// <param name="pos">Highlight할 위치</param>
    public void ShowBuildHighlight(Vector3 pos, bool isGreen = true)
    {
        if (curHighlights.Count <= curHighlight_index) return;

        curHighlights[curHighlight_index].transform.root.position = pos;
        curHighlights[curHighlight_index].transform.root.gameObject.SetActive(true);
        if(isGreen)
            curHighlights[curHighlight_index].color = Define.GlowGreen;
        else
            curHighlights[curHighlight_index].color = Define.GlowRed;

        curHighlight_index++;
        isHighlightOn = true;
    }

    /// <summary>
    /// 모든 Highlight 꺼줌
    /// </summary>
    public void HideAllHighlights()
    {
        if (isHighlightOn == false) return; // 중복 작업 방지

        foreach(SpriteRenderer obj in curHighlights)
        {
            obj.transform.root.gameObject.SetActive(false);
        }
        curHighlight_index = 0;
        isHighlightOn = false;
    }

    /// <summary>
    /// 보스맵 켜기 (연출 포함)
    /// </summary>
    public void StartBossMap()
    {
        BossGate.SetActive(true);
        dotSeq.Restart();
        MainProps.SetActive(false);
        BossProps.SetActive(true);
    }

    /// <summary>
    /// 보스맵 끄기
    /// </summary>
    public void EndBossMap()
    {
        BossGate.transform.position = gateUpPos;
        BossGate.transform.localScale = Vector3.one;
        BossGate.SetActive(false);

        MainProps.SetActive(true);
        BossProps.SetActive(false);
    }
}
