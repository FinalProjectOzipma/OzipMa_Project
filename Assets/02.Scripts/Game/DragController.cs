using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    public bool IsSlotDragging = false;

    private BuildingSystem buildingSystem;
    private SpriteRenderer spriteRenderer;
    private Vector3 eventPosition;
    private GameObject dragObject;
    private bool isEditDragging = false;

    public float HoldTimeThreshold = 0.4f; // 홀드 지연 시간
    private GameObject towerMenu; // 불러온 타워메뉴창 프리팹 저장
    private UI_TowerMenu uiTowerMenu;
    private float pressTime = 0f; // 홀드 경과 시간
    private bool isEditMode = false; 

    private void Start()
    {
        buildingSystem = GetComponent<BuildingSystem>();
        Managers.Resource.Instantiate("TowerMenu", go =>
        {
            towerMenu = go;
            towerMenu.SetActive(false);
            uiTowerMenu = towerMenu.GetComponent<UI_TowerMenu>();
            SetEditMode(false);
        });
    }

    private void Update()
    {
        if (IsSlotDragging) return;

        // 드래그 Begin
        if(Input.GetMouseButton(0) && !isEditDragging)
        {
            eventPosition = Input.mousePosition;
            GameObject detectedObj = buildingSystem.GetCurrentDragObject(eventPosition);
            if (detectedObj == null)
            {
                // 다른 곳 클릭했을 시 숨기기
                SetEditMode(false);
                return;
            }

            if(isEditMode)
            {
                BeginDrag(detectedObj);
                return;
            }

            pressTime += Time.deltaTime;
            if (pressTime >= HoldTimeThreshold)
            {
                SetEditMode(true);
                BeginDrag(detectedObj);        
            }
        }

        // 드래그 Update
        if(isEditDragging)
        {
            Drag();
        }

        // 드래그 End
        if(Input.GetMouseButtonUp(0))
        {
            pressTime = 0f;

            if (isEditDragging)
            {
                EndDrag();
            }
        }
    }

    public void BeginDrag(GameObject detectedObj)
    {
        uiTowerMenu.TargetTower = dragObject = detectedObj.transform.root.gameObject;
        dragObject.GetComponent<TowerControlBase>().TowerStop();
        spriteRenderer = Util.FindComponent<SpriteRenderer>(detectedObj, "MainSprite");
        if (dragObject != null)
        {
            dragObject.transform.position = buildingSystem.UpdatePosition(eventPosition);
            isEditDragging = true;
        }
    }

    public void Drag()
    {
        // 편집메뉴UI, 타워 위치 조정
        towerMenu.transform.position = dragObject.transform.position = buildingSystem.UpdatePosition(Input.mousePosition);

        // 배치 가능/불가능 색상
        if (buildingSystem.CanTowerBuild(Input.mousePosition))
        {
            spriteRenderer.color = Color.green;
            return;
        }
        spriteRenderer.color = Color.red;
    }

    public void EndDrag()
    {
        isEditDragging = false;
        spriteRenderer.color = Color.white;
        dragObject.GetComponent<TowerControlBase>().TowerStart();

        // 드래그 종료 위치에 배치 완료 
        Vector2 inputPos = Input.mousePosition;
        if (isEditMode && buildingSystem.CanTowerBuild(inputPos))
        {
            dragObject.transform.position = buildingSystem.UpdatePosition(inputPos);
            int key = buildingSystem.RemovePlacedMapScreenPos(eventPosition);
            buildingSystem.AddPlacedMap(inputPos, key);
            dragObject = null;
            return;
        }

        // 이전에 있던 위치에 되돌려놓기 
        uiTowerMenu.transform.position = dragObject.transform.position = buildingSystem.UpdatePosition(eventPosition);
        dragObject = null;
    }

    public void SetEditMode(bool isOn)
    {
        isEditMode = isOn;
        towerMenu.SetActive(isOn);
    }
}
