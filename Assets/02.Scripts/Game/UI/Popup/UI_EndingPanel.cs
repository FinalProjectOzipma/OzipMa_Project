using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_EndingPanel : UI_Scene
{
    enum Texts
    {
        RewordGold,
        RewordExp
    }

    enum Rectransforms
    {
        OverUI,
        ClearUI,
    }

    private void OnEnable()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<RectTransform>(typeof(Rectransforms));

        Managers.UI.SetSceneList<UI_EndingPanel>(this);
    }

    public void MoveEndingPanel(bool isClear)
    {
        int whatRect = isClear ? (int)Rectransforms.ClearUI : (int)Rectransforms.OverUI; 
        RectTransform rt = Get<RectTransform>(whatRect);
        Vector3 originalPos = rt.localPosition;

        Sequence seq = DOTween.Sequence();
        seq.Append(rt.DOLocalMoveY(originalPos.y - 500f, 1.0f).SetEase(Ease.OutQuad))
           .AppendInterval(2.0f)
           .Append(rt.DOLocalMoveY(originalPos.y, 1.0f).SetEase(Ease.InQuad));
    }
}
