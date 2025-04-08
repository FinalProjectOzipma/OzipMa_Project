using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TestButton : UI_Base
{
    enum Buttons
    {
        ToggleButton
    }
    enum GameObjects
    {
        UI_Management,
    }
    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        //Bind
        Bind<Button>(typeof(Buttons));
        Bind<UI_Management>(typeof(GameObjects));

        //BindEvent
        GetButton((int)Buttons.ToggleButton).gameObject.BindEvent(OnButtonClicked);
    }

    public void OnButtonClicked(PointerEventData data)
    {
        Get<UI_Management>((int)GameObjects.UI_Management).SlideToggle();
    }
}
