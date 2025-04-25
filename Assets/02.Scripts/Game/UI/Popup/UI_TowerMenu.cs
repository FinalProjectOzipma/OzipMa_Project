using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TowerMenu : UI_Base
{
    enum Buttons
    {
        DeleteButton
    }

    enum Images
    {
        ButtonImage
    }

    public GameObject targetTower;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        Get<Button>((int)Buttons.DeleteButton).gameObject.BindEvent(OnClickDelete);
    }


    // 버튼 클릭하면 타켓타워 삭제
    public void OnClickDelete(PointerEventData data)
    {
        Managers.Resource.Destroy(targetTower);
        gameObject.SetActive(false);
    }
 
}
