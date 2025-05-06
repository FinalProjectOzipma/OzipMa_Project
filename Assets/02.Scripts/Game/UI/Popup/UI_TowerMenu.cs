using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TowerMenu : UI_Base
{
    [SerializeField] private Button DeleteButton;
    [SerializeField] private Image ButtonImage;

    public GameObject TargetTower;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        DeleteButton.gameObject.BindEvent(OnClickDelete);
    }


    // 버튼 클릭하면 타켓타워 삭제
    public void OnClickDelete(PointerEventData data)
    {
        Managers.Resource.Destroy(TargetTower);
        BuildingSystem.Instance.RemovePlacedMapWorldPos(TargetTower.transform.position);
        BuildingSystem.Instance.DragController.SetEditMode(false);
    }
 
}
