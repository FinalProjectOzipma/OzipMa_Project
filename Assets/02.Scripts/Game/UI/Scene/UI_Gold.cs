using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Gold : UI_Scene
{
    [SerializeField] private RectTransform GoldPoint;

    public void Start()
    {
        Managers.UI.SetSceneList<UI_Gold>(this);
    }

    public RectTransform GetGoldPoint()
    {
        return GoldPoint;
    }
}
