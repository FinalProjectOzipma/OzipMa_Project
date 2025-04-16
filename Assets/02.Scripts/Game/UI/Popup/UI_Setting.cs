using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Setting : UI_Base
{
    enum Buttons
    {
        SoundButton,
        LanguageButton,
        IDButton
    }

    enum GameObjects
    {
        SoundPopup,

    }


    private void Start()
    {
        Init();
        
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        Get<GameObject>((int)GameObjects.SoundPopup).gameObject.SetActive(false);

        GetButton((int)Buttons.SoundButton).gameObject.BindEvent(OnClickSoundPopUp);
    }


    public void OnClickSoundPopUp(PointerEventData data)
    {
        Get<GameObject>((int)GameObjects.SoundPopup).gameObject.SetActive(true);
    }
}
