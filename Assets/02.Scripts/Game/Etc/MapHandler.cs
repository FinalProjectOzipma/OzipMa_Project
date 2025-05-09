using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    public GameObject MainProps;
    public GameObject BossProps;
    public GameObject BossGate;

    private Sequence dotSeq; // DOTween 시퀀스
    private Vector3 gateUpPos = new Vector3(0f, 5f, 1f);
    private float gateDownOffset = -1f;

    private void Start()
    {
        dotSeq = Util.RecyclableSequence();
        dotSeq.Append(BossGate.transform.DOLocalMoveY(gateDownOffset, 0.3f));
        dotSeq.Append(BossGate.transform.DOScale(1.1f, 0.6f));
        dotSeq.Append(BossGate.transform.DOScale(1f, 0.6f));

        dotSeq.Append(BossGate.transform.DOScale(5f, 0.4f));
        dotSeq.Append(BossGate.transform.DOScale(1f, 0f));
        dotSeq.Append(BossGate.transform.DOLocalMoveY(gateDownOffset * 10, 0f));

        Managers.Wave.OnStartBossMap += StartBossMap;
        Managers.Wave.OnEndBossMap += EndBossMap;
    }

    public void StartBossMap()
    {
        BossGate.SetActive(true);
        dotSeq.Restart();
        MainProps.SetActive(false);
        BossProps.SetActive(true);
    }
    public void EndBossMap()
    {
        BossGate.transform.position = gateUpPos;
        BossGate.transform.localScale = Vector3.one;
        BossGate.SetActive(false);

        MainProps.SetActive(true);
        BossProps.SetActive(false);
    }
}
