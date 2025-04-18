using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Start : UI_Base
{
    enum Buttons
    {
        StartButton
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickGameScene);
    }

    public void OnClickGameScene(PointerEventData data)
    {
        Managers.Scene.ChangeScene<GameScene>(Managers.Scene.GameScene);
    }
}
