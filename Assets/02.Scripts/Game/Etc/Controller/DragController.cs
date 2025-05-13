using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragController : MonoBehaviour
{
    [HideInInspector] public Button DeleteButton;
    public bool IsSlotDragging = false; // Slot쪽에서 드래그중인가?
    public float HoldTimeThreshold = 0.3f; // 홀드 지연 시간

    private BuildingSystem buildingSystem;
    private SpriteRenderer spriteRenderer;
    private Vector3 eventPosition;
    private GameObject dragObject;
    private bool isEditDragging = false; // 드래그 중인가?


    private GameObject towerMenu; // 불러온 타워메뉴창 프리팹 저장
    private UI_TowerMenu uiTowerMenu; // 타워메뉴 스크립트
    private float pressTime = 0f; // 홀드 경과 시간
    private bool isEditMode = false; // 편집모드인가?

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
        if (IsSlotDragging) return; // 슬롯에서 드래그중일 때는 오브젝트 드래그 막기

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
                if(isEditMode) SetEditMode(false);
                buildingSystem.HideBuildHighlight(); // 도감 클릭 버그 발생하면 안꺼져서 직접 꺼주기
                // 아무 클릭 감지된 것은 return;
                return;
            }

            // 이미 편집모드면 클릭만해도 드래그가능
            if (isEditMode)
            {
                BeginDrag(detectedObj);
                return;
            }

            // 첫 드래그는 HoldTimeThreshold초 이상 클릭해야 시작됨
            pressTime += Time.deltaTime;
            if (pressTime >= HoldTimeThreshold)
            {
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
        // 이전에 편집하던 타워는 사거리 표시 끄기
        if (isEditMode)
        {
            curTowerController?.HideRangeIndicator(); 
        }

        // 현재 감지된 타워로 새로운 작업
        SetEditMode(true);

        uiTowerMenu.TargetTower = dragObject = detectedObj.transform.root.gameObject;
        curTowerController = dragObject.GetComponent<TowerControlBase>();
        curTowerBody = curTowerController.GetTowerBodyBase();
        spriteRenderer = curTowerBody.GetMainSpriteObj().GetComponent<SpriteRenderer>();

        curTowerController.TowerStop(); // 편집모드에서는 타워 작동 멈추기

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
        if (curTowerController != null)
        {
            curTowerController.TowerStart(); // 타워 작동 재개
        }

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

        if (isOn)
        {
            buildingSystem.ShowBuildHighlight(); // 빌드 가능 구역 반짝이 키기
        }
        else if (!isOn)
        {
            buildingSystem.HideBuildHighlight(); // 빌드 가능 구역 반짝이 끄기
            curTowerController?.HideRangeIndicator(); // 편집모드를 끌 때 사거리 표시 끄기
        }
    }

    /// <summary>
    /// 특정 위치에 특정 버튼이 있는지 확인
    /// </summary>
    /// <param name="screenPosition">체크할 위치</param>
    /// <param name="targetButton">체크할 대상 버튼</param>
    /// <returns></returns>
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
