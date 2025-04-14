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
        StackGageFill
    }

    private enum TextMeshs
    {
        ObjInfo,
        StackText
    }

    public int Index;

    public IGettable Gettable;

    private Sprite _sprite;
    private Image _stackGage;

    private void Awake()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(TextMeshs));
    }

    public void SetData(IGettable gettable)
    {
        Gettable = gettable;
        UserObject obj = gettable.GetClassAddress<UserObject>();
        _sprite = obj.Sprite;
        GetImage((int)Images.Icon).sprite = _sprite;
        GetText((int)TextMeshs.ObjInfo).text = $"LV.{obj.Level}\r\nEV.{obj.Grade}";
        GetImage((int)Images.StackGageFill).fillAmount = obj.Stack.GetValue() % obj.MaxStack.GetValue();
        GetText((int)TextMeshs.StackText).text = $"{obj.Stack.GetValue()}/{obj.MaxStack.GetValue()}";
    }
}
