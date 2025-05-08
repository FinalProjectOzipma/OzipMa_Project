using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossTitle : UI_Scene
{
    private Renderer[] renderers;

    Dictionary<string, Color> colorInfo = new Dictionary<string, Color>();

    [SerializeField] private Image Bg;
    [SerializeField] private Image Line;
    [SerializeField] private Image CenterBg;
    [SerializeField] private Image CenterIcon;
    [SerializeField] private TextMeshProUGUI Text;

    public override void Init()
    {
        base.Init();

        colorInfo.Add(nameof(Bg), Bg.color);
        colorInfo.Add(nameof(Line), Line.color);
        colorInfo.Add(nameof(CenterBg), CenterBg.color);
        colorInfo.Add(nameof(CenterIcon), CenterIcon.color);
        colorInfo.Add(nameof(Text), Text.color);

        uiSeq = Util.RecyclableSequence();

        uiSeq.Append(transform.DOScale(1f, 1f));
        uiSeq.Join(Bg.DOColor(GetColor(nameof(Bg)), 1f));
        uiSeq.Join(Bg.DOColor(GetColor(nameof(Line)), 1f));
        uiSeq.Join(Bg.DOColor(GetColor(nameof(CenterBg)), 1f));
        uiSeq.Join(Bg.DOColor(GetColor(nameof(CenterIcon)), 1f));
        uiSeq.Join(Bg.DOColor(GetColor(nameof(Text)), 1f));
    }

    private Color GetColor(string name)
    {
        if (colorInfo.TryGetValue(name, out var color)) return color;

        Util.LogWarning($"{gameObject.name}에서 컬러를 가져오질 못했습니다.");
        return Color.white;
    }
}
