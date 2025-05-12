using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GachaResult : MonoBehaviour
{
    [SerializeField] private GameObject resultSlots;
    [SerializeField] private Button offButton;

    private void OnEnable()
    {
        offButton.enabled = false;
    }

    public void ShowResult(List<IGettable> result, int num)
    {
        foreach (UserObject data in result)
        {
            Managers.Resource.Instantiate($"{data.RankType}_Slot", go =>
            go.GetComponent<UI_GachaSlot>().Setup(data as MyUnit));
        }

        offButton.enabled = true;
    }
}
