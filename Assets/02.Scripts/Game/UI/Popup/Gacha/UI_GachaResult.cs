using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GachaResult : UI_Popup
{
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
        foreach(UI_GachaSlot tf in slots)
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
        Bg.enabled = true;
    }

    /// <summary>
    /// 중복 오브젝트 검사
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private bool FindUO(UserObject data, out int index)
    {
        for (int i = 0; i< slots.Count; i++)
        {
            if (slots[i].userObj.Name == data.Name)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }
}
