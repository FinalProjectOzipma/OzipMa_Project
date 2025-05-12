using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GachaSlot : UI_Base
{
    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI CountTxt;

    private int count;
    private Sprite spr;
    private string initStr;
    public UserObject userObj { get; private set; }

    //데이터 받는함수
    public void Setup(UserObject userObj)
    {
        spr = Icon.sprite;
        initStr = CountTxt.text;

        this.userObj = userObj;

        Icon.sprite = userObj.Sprite;
        CountTxt.enabled = false;
        count = 1;
    }

    //중복 되는거 더해주기
    public void AddCount()
    {
        CountTxt.enabled = true;
        count++;
        CountTxt.text = count.ToString();
    }

    //초기화 작업
    public override void Init()
    {
        userObj = null;
        Icon.sprite = spr;
        count = 0;
        CountTxt.text = initStr;
        CountTxt.enabled = false;
    }
}