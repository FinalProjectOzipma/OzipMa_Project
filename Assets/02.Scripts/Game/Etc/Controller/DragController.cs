using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragController : MonoBehaviour
{
    [HideInInspector] public Button DeleteButton;
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
    private TowerControlBase curTowerController;
    private TowerBodyBase curTowerBody;

    private void Start()
    {
        buildingSystem = GetComponent<BuildingSystem>();
        Managers.Resource.Instantiate("TowerMenu", go =>
        {
            towerMenu = go;
            towerMenu.SetActive(false);
            DeleteButton = towerMenu.GetComponentInChildren<Button>(true);
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

            if (IsSpecificUIButtonClicked(eventPosition, DeleteButton))
            {
                // 버튼을 눌렀을 때만 동작
                return;
            }

            GameObject detectedObj = buildingSystem.GetCurrentDragObject(eventPosition);

            if (detectedObj == null)
            {
                // 다른 곳 클릭했을 시 숨기기
                SetEditMode(false);
                return;
            }

            if (isEditMode)
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

    /// <summary>
    /// 오브젝트 드래그 시작
    /// </summary>
    /// <param name="detectedObj">클릭 감지된 오브젝트</param>
    public void BeginDrag(GameObject detectedObj)
    {
        uiTowerMenu.TargetTower = dragObject = detectedObj.transform.root.gameObject;
        curTowerController = dragObject.GetComponent<TowerControlBase>();
        curTowerBody = curTowerController.GetTowerBodyBase();
        spriteRenderer = curTowerBody.GetMainSpriteObj().GetComponent<SpriteRenderer>();

        curTowerController.TowerStop(); // 편집모드에서는 타워 작동 멈추기
        if (curTowerBody != null) curTowerBody.ShowRangeIndicator(); // 사거리 표시
        Util.Log("사거리 표시 켜는 곳");

        // 드래그 오브젝트 위치 업데이트
        if (dragObject != null)
        {
            dragObject.transform.position = buildingSystem.UpdatePosition(eventPosition);
            isEditDragging = true;
        }

    }

    /// <summary>
    /// 오브젝트 드래그 업데이트
    /// </summary>
    public void Drag()
    {
        // 편집메뉴UI, 타워 위치 업데이트
        towerMenu.transform.position = dragObject.transform.position = buildingSystem.UpdatePosition(Input.mousePosition);

        // 배치 가능/불가능 색상 표시
        if (buildingSystem.CanTowerBuild(Input.mousePosition))
        {
            spriteRenderer.color = Color.green;
            return;
        }
        spriteRenderer.color = Color.red;
    }

    /// <summary>
    /// 오브젝트 드래그 끝
    /// </summary>
    public void EndDrag()
    {
        isEditDragging = false;
        spriteRenderer.color = Color.white;
        if (curTowerController != null) curTowerController.TowerStart(); // 타워 작동 재개
        if (curTowerBody != null) curTowerBody.HideRangeIndicator(); // 사거리 표시 끄기

        // 드래그 종료 위치에 배치 완료 
        Vector2 inputPos = Input.mousePosition;
        if (isEditMode && buildingSystem.CanTowerBuild(inputPos))
        {
            dragObject.transform.position = buildingSystem.UpdatePosition(inputPos);
            int key = buildingSystem.RemovePlacedMapScreenPos(eventPosition);
            buildingSystem.AddPlacedMapScreenPos(inputPos, key);
            dragObject = null;
            return;
        }

        // 이전에 있던 위치에 되돌려놓기 
        uiTowerMenu.transform.position = dragObject.transform.position = buildingSystem.UpdatePosition(eventPosition);
        dragObject = null;
    }

    /// <summary>
    /// 편집모드 On/Off
    /// </summary>
    /// <param name="isOn">true:켜기, false:끄기</param>
    public void SetEditMode(bool isOn)
    {
        isEditMode = isOn;
        towerMenu.SetActive(isOn);
    }

    public bool IsSpecificUIButtonClicked(Vector2 screenPosition, Button targetButton)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            Button hitButton = result.gameObject.GetComponentInParent<Button>();
            if (hitButton == targetButton)
            {
                return true;
            }
        }

        return false; // 해당 버튼 아님
    }
}
