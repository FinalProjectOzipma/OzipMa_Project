using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : UI_Scene
{
    private enum Images
    {
        Icon,
        StackGageFill,
        Selected
    }

    private enum TextMeshs
    {
        ObjInfo,
        StackText
    }

    private Button button; // 이녀석은 현재 들고 있는 컴포넌트객체니깐 그냥 Get으로 불러드림

    public int Index { get; set; }
    public bool IsActive { get; private set; } = false;

    public IGettable Gettable;

    private Sprite _sprite;
    private Image _stackGage;

    private void Awake()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(TextMeshs));
        button = GetComponent<Button>();
        button.onClick.AddListener(SeletToggle);

        GetImage((int)Images.Selected).gameObject.SetActive(false);
    }

    private void SeletToggle()
    {
        GameObject imgObj = GetImage((int)Images.Selected).gameObject;
        imgObj.SetActive(!imgObj.activeSelf);
    }

    // 전체 선택될때 호출해야되는 메서드
    public void OnSelect()
    {
        IsActive = true;
        GetImage((int)Images.Selected).gameObject.SetActive(true);
    }

    public void DisSelect()
    {
        IsActive = false;
        GetImage((int)Images.Selected).gameObject.SetActive(false);
    }


    public void SetData<T>(IGettable gettable) where T : UserObject
    {
        
        Gettable = gettable;
        T obj = gettable.GetClassAddress<T>();
        var status = obj.Status;
        _sprite = obj.Sprite;
        GetImage((int)Images.Icon).sprite = _sprite;
        GetText((int)TextMeshs.ObjInfo).text = $"LV.{status.Level.GetValue()}\r\nEV.{status.Grade.GetValue()}";
        GetImage((int)Images.StackGageFill).fillAmount = status.Stack.GetValue() % status.MaxStack.GetValue();
        GetText((int)TextMeshs.StackText).text = $"{status.Stack.GetValue()}/{status.MaxStack.GetValue()}";
    }
}
