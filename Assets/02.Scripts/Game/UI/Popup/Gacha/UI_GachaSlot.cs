using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GachaSlot : UI_Scene
{
    [SerializeField] private Image Icon;
    [SerializeField] private UserObject userObj;

    //초기화함수
    public void Setup(UserObject userObj)
    {
        this.userObj = userObj;
        Icon.sprite = userObj.Sprite;
    }
}