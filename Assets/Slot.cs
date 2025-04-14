using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : Poolable
{
    private string type;
    public int Index;

    public IGettable Gettable;

    private Sprite _sprite;
    private Image _stackGage;
    private TextMeshProUGUI _InfoMesh;
    private TextMeshProUGUI _stackMesh;



    public void SetData(IGettable gettable)
    {
        /*Gettable = gettable;


        _sprite = userObject.Sprite;
        _InfoMesh.text = $"LV.{userObject.Level}\r\nEV.{userObject.Grade}";
        _stackGage.fillAmount = userObject.Stack.GetValue() % userObject.MaxStack.GetValue();
        _stackMesh.text = $"{userObject.Stack.GetValue()}/{userObject.MaxStack.GetValue()}";*/
    }
}
