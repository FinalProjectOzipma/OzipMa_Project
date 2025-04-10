using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TestButton : UI_Base
{
    enum Buttons
    {
        ToggleButton,
    }
    enum UI_Managements
    {
        UI_Management,
    }

    public bool IsPopupOpened;

    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        //Bind
        Bind<Button>(typeof(Buttons));
        Bind<UI_Management>(typeof(UI_Managements));

        //BindEvent
        GetButton((int)Buttons.ToggleButton).gameObject.BindEvent(OnButtonClicked, Define.UIEvent.Click);
    }

    public void OnButtonClicked(PointerEventData data)
    {
        Get<UI_Management>((int)UI_Managements.UI_Management).SlideToggle();

        if(IsPopupOpened)
        {
            Managers.UI.ClosePopupUI();
        }
        else
        {
            Managers.UI.ShowPopupUI<UI_TestPopup>("TestPopup");
        }
        IsPopupOpened = !IsPopupOpened;
    }
}
