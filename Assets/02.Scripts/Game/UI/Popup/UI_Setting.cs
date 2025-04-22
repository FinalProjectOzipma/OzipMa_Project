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
        SettingButton
    }

    enum Images
    {
        SettingImage
    }

    enum GameObjects
    {
        UI_Sound

    }


    private void Start()
    {
        Init();
        
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));


        GetButton((int)Buttons.SettingButton).gameObject.BindEvent(OnClickSoundPopUp);
    }


    public void OnClickSoundPopUp(PointerEventData data)
    {
        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick, transform.position);
        Get<GameObject>((int)GameObjects.UI_Sound).SetActive(true);
    }
}
