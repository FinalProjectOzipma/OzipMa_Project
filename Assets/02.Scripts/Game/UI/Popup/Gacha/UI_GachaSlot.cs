using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GachaSlot : UI_Base
{
    [SerializeField] private RectTransform AnimRoot;
    [SerializeField] private Image Icon;
    [SerializeField] private Image Cover;

    private Color coverColor;

    public UserObject userObj { get; private set; }

    private void Awake()
    {
        coverColor = Cover.color;
        coverColor.a = 1f;
    }

    /// <summary>
    /// 데이터 받는함수
    /// </summary>
    /// <param name="userObj"></param>
    public void Setup(UserObject userObj)
    {
        this.userObj = userObj;

        Icon.sprite = userObj.Sprite;
    }

    /// <summary>
    /// 초기화 작업
    /// </summary>
    public override void Init()
    {
        userObj = null;
        Cover.color = coverColor;

        // AnimRoot 스케일 초기화
        AnimRoot.localScale = Vector3.one;
    }

    /// <summary>
    /// 커버이미지 점점 투명해지게 함
    /// </summary>
    public void FadeOut()
    {
        Cover.DOFade(0f, 0.2f).SetEase(Ease.InOutCirc);
    }
}