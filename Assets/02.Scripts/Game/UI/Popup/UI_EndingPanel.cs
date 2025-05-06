using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_EndingPanel : UI_Scene
{
    [SerializeField] private TextMeshProUGUI RewordGold;
    [SerializeField] private TextMeshProUGUI RewordExp;

    [SerializeField] private RectTransform ClearUI;
    [SerializeField] private RectTransform OverUI;

    private void OnEnable()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        ClearUI.localPosition = new Vector3(0.0f, 1200.0f, 0.0f);
        OverUI.localPosition = new Vector3(0.0f, 1200.0f, 0.0f);
        Managers.UI.SetSceneList<UI_EndingPanel>(this);
    }

    public void MoveEndingPanel(bool isClear)
    {
        RectTransform whatRect = isClear ? ClearUI : OverUI; 
        Vector3 originalPos = whatRect.localPosition;

        Sequence seq = DOTween.Sequence();
        seq.Append(whatRect.DOLocalMoveY(originalPos.y - 500f, 1.0f).SetEase(Ease.OutQuad))
           .AppendInterval(2.0f)
           .Append(whatRect.DOLocalMoveY(originalPos.y, 1.0f).SetEase(Ease.InQuad));
    }
}
