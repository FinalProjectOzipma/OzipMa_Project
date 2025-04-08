using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Management : UI_Base
{
    enum GameObjects
    {
        UI_Management,
    }

    bool isOpened;
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        //Bind
        Bind<GameObject>(typeof(GameObjects));

        //Get

        //BindEvent
    }

    public void SlideToggle()
    {
        if(isOpened)
        {
            Debug.Log("닫힘");
            transform.DOLocalMoveY(transform.localPosition.y - 500f, 0.5f).SetEase(Ease.OutCubic);
        }
        else
        {
            Debug.Log("열림");
            transform.DOLocalMoveY(transform.localPosition.y + 500f, 0.5f).SetEase(Ease.OutCubic);
        }
        isOpened = !isOpened;
    }
}
