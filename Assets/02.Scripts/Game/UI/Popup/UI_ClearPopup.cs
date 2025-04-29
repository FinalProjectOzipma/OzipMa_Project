using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_ClearPopup : UI_Popup
{
    enum Texts
    {
        RewordGold,
        RewordExp
    }

    enum Rectransforms
    {
        Popup
    }

    private void Start()
    {
        Init();
        MoveClearPopup();
    }

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<RectTransform>(typeof(Rectransforms));

        Get<RectTransform>((int)Rectransforms.Popup).localPosition = new Vector3(0.0f, 1200.0f, 0.0f);

    }

    private void MoveClearPopup()
    {
        RectTransform rt = Get<RectTransform>((int)Rectransforms.Popup);
        Vector3 originalPos = rt.localPosition;

        Sequence seq = DOTween.Sequence();
        seq.Append(rt.DOLocalMoveY(originalPos.y - 500f, 1.0f).SetEase(Ease.OutQuad))
           .AppendInterval(2.0f)
           .Append(rt.DOLocalMoveY(originalPos.y, 1.0f).SetEase(Ease.InQuad));
    }

}
