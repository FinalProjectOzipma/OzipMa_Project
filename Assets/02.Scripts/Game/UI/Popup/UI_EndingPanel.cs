using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_EndingPanel : UI_Scene
{
    [SerializeField] private TextMeshProUGUI RewordGold;
    [SerializeField] private TextMeshProUGUI RewordGem;

    [SerializeField] private GameObject ClearUI;
    [SerializeField] private GameObject OverUI;

    private void OnEnable()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Managers.UI.SetSceneList<UI_EndingPanel>(this);
        ClearUI.SetActive(false);
        OverUI.SetActive(false);

    }

    public void SetRewardText(long gold, long gem)
    {
        RewordGold.text = $"{Util.FormatNumber(gold)}";
        RewordGem.text = $"{Util.FormatNumber(gem)}";
    }

    public void MoveEndingPanel(bool isClear)
    {
        GameObject whatgo = isClear ? ClearUI : OverUI;

        EndPanelAnime(whatgo);
    }

    public void EndPanelAnime(GameObject go, float showDuration = 2.0f)
    {
        if (uiSeq != null && uiSeq.IsActive())
            uiSeq.Kill();

        // 패널 활성화 + 초기 크기 설정
        go.SetActive(true);
        go.transform.localScale = Vector3.zero;

        uiSeq = Util.RecyclableSequence();

        uiSeq.Append(go.transform.DOScale(1.1f, 0.1f).SetEase(Ease.OutBack))
             .Append(go.transform.DOScale(1.0f, 0.1f))
             .AppendInterval(showDuration)
             .Append(go.transform.DOScale(1.1f, 0.1f))
             .Append(go.transform.DOScale(0.2f, 0.1f))
             .AppendCallback(() => go.SetActive(false))
             .Play();
    }
}
