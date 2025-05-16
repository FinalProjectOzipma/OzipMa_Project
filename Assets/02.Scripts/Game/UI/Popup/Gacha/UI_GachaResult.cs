using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GachaResult : UI_Popup
{
    [SerializeField] private ScrollRect ScrollRect;
    [SerializeField] private Transform ResultSlots;
    [SerializeField] private Button Bg;
    private List<UI_GachaSlot> slots;

    private void Awake()
    {
        slots = new();
        Bg.onClick.AddListener(CloseResult);
    }

    /// <summary>
    /// 결과창 닫기
    /// </summary>
    private void CloseResult()
    {
        foreach (UI_GachaSlot tf in slots)
        {
            tf.Init();
            Managers.Resource.Destroy(tf.gameObject);
        }
        slots.Clear();
        Bg.enabled = false;
        Managers.Resource.Destroy(gameObject);
    }

    /// <summary>
    /// 결과 보여주기
    /// </summary>
    /// <param name="result"></param>
    public void ShowResult(List<IGettable> result)
    {
        
        ScrollRect.velocity = Vector2.zero;

        foreach (UserObject data in result)
        {
            Managers.Resource.Instantiate($"{data.RankType}_Slot", go =>
            {
                var component = go.GetComponent<UI_GachaSlot>();
                slots.Add(component);
                go.transform.SetParent(ResultSlots);
                component.Setup(data);
                go.transform.localScale = Vector3.one * 10f;
                go.transform.DOScale(1f, 0.15f).SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    component.FadeOut();
                });
                Managers.Audio.PlaySFX(SFXClipName.Card);
            });
        }
        ScrollRect.verticalNormalizedPosition = 1f;
        Bg.enabled = true;
    }
}
