using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void ShowResult(List<IGettable> result)
    {
        foreach (UserObject data in result)
        {
            if (FindUO(data, out int index))
            {
                slots[index].AddCount();
            }
            else
            {
                Managers.Resource.Instantiate($"{data.RankType}_Slot", go =>
                {
                    var component = go.GetComponent<UI_GachaSlot>();
                    slots.Add(component);
                    go.transform.SetParent(ResultSlots);
                    component.Setup(data);
                });
            }
        }

        Bg.enabled = true;
    }

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
