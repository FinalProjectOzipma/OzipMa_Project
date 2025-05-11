using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GachaSlot : UI_Scene
{
    [SerializeField] private Image icon;
    [SerializeField] private UserObject userObj;
    [SerializeField] private TextMeshProUGUI countTxt;
    private int count;


    //초기화함수
    public void Setup(UserObject userObj)
    {
        count = 1;
        this.userObj = userObj;
        icon.sprite = userObj.Sprite;
        countTxt.gameObject.SetActive(false);
    }

    public UserObject GetMyUnit()
    {
        return this.userObj; 
    }

    public void AddCount()
    {
        count++;
        countTxt.text = count.ToString();
        countTxt.gameObject.SetActive(true);
    }
}