using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Gem : UI_Scene
{
    [SerializeField] private RectTransform ZamPoint;

    public void Start()
    {
        Managers.UI.SetSceneList<UI_Gem>(this);
    }

    public RectTransform GetGemPoint()
    {
        return ZamPoint;
    }
}
