using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class TutorialController : UI_Scene
{
    // 튜토리얼 요소 
    public Cursor Cursor;
    public Dialogue Dialogue;
    [SerializeField] private Button SkipButton;

    #region 버튼 위치를 위한 빈오브젝트 모음
    public GameObject[] MenuButtons; // 하단바 메뉴 버튼들
    public GameObject SlotPosition; // 인벤토리 슬롯 위치
    public GameObject ResearchStartPos; // 연구 시작 버튼
    public GameObject ResearchGoldPos; // 연구 골드가속 버튼
    public GameObject GachaStartPos; // 가챠 뽑기 버튼
    public GameObject DSlotPosition; // 도감 슬롯 위치
    #endregion

    [HideInInspector] public UI_Main MainUI;
    [HideInInspector] public RectTransform CursorRect; // 커서의 부모 Rect, 커서의 위치 기준으로 사용되기 때문에 CursorRect로 명명.

    private Queue<TutorialBase> queue = new();
    private TutorialBase currentTutorial;

    private List<GameObject> hiddenDuringTutorial; // 튜토리얼동안 숨겨둘 오브젝트들
    private InventoryUI inventoryUI;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        SkipButton.gameObject.BindEvent(OnClickSkip);

        MainUI = Managers.UI.GetScene<UI_Main>();
        CursorRect = gameObject.GetComponent<Canvas>().GetComponent<RectTransform>();
        inventoryUI = Managers.UI.GetScene<InventoryUI>();

        // 튜토리얼동안 막아둘 오브젝트들 수집
        hiddenDuringTutorial = new List<GameObject>(3);
        foreach (GameObject go in MainUI.GetTutorialHiddenObjects())
        {
            hiddenDuringTutorial.Add(go);
        }
        hiddenDuringTutorial.Add(inventoryUI.GetSwipeBtn());

        // 튜토리얼동안 봉인
        foreach(GameObject go in hiddenDuringTutorial)
        {
            go.SetActive(false);
        }


        // 튜토리얼을 순서대로 넣기 
        switch (Managers.Player.LastTutorialStep)
        {
            case Enums.TutorialStep.None:
                queue.Enqueue(new PlaceTowerTutorial(this, Enums.TutorialStep.PlaceTower));
                goto case Enums.TutorialStep.PlaceTower;
            case Enums.TutorialStep.PlaceTower:
                queue.Enqueue(new EditTowerTutorial(this, Enums.TutorialStep.EditTower));
                goto case Enums.TutorialStep.EditTower;
            case Enums.TutorialStep.EditTower:
                queue.Enqueue(new DeleteTowerTutorial(this, Enums.TutorialStep.DeleteTower));
                goto case Enums.TutorialStep.DeleteTower;
            case Enums.TutorialStep.DeleteTower:
                queue.Enqueue(new ResearchTutorial(this, Enums.TutorialStep.Research));
                goto case Enums.TutorialStep.Research;
            case Enums.TutorialStep.Research:
                queue.Enqueue(new GachaTutorial(this, Enums.TutorialStep.Gacha));
                goto case Enums.TutorialStep.Gacha;
            case Enums.TutorialStep.Gacha:
            default:
                break;
        }

        // 튜토리얼 시작
        NextTutorial();
    }

    private void Update()
    {
        // 대화 끝나면 커서 보여주기
        if (Dialogue.IsEnd == true)
        {
            ShowOnlyCursor();
        }

        // 튜토리얼 조건 만족시 다음 튜토리얼로 넘김
        if (currentTutorial.CheckCondition() == true)
        {
            Util.Log("튜토 하나 끗");
            NextTutorial();
        }
    }

    public void NextTutorial()
    {
        if (queue.Count > 0)
        {
            Dialogue.ClearQueue();
            currentTutorial?.OnEnd();
            currentTutorial = queue.Dequeue();
            currentTutorial.OnStart();
            ShowOnlyDialogue();
        }
        else // 모든 튜토리얼 완료했으면
        {
            currentTutorial?.OnEnd();
            TutorialEnd();
        }
    }

    public void OnClickSkip(PointerEventData data)
    {
       
        switch (currentTutorial.Step)
        {
            case Enums.TutorialStep.PlaceTower:
                queue.Dequeue();
                goto case Enums.TutorialStep.EditTower;
            case Enums.TutorialStep.EditTower:
                queue.Dequeue();
                goto case Enums.TutorialStep.DeleteTower;
            case Enums.TutorialStep.DeleteTower:
                NextTutorial();
                break; // 타워 튜토리얼 스킵
            case Enums.TutorialStep.Research:
                NextTutorial();
                break;
            case Enums.TutorialStep.Gacha:
                NextTutorial();
                break;
            default:
                Util.LogWarning("이상한 튜토리얼 타입이 존재");
                break;
        }
    }

    public void ShowOnlyDialogue()
    {
        SetCursorActive(false);
        SetDialogueActive(true);
    }
    public void ShowOnlyCursor()
    {
        SetCursorActive(true);
        SetDialogueActive(false);
    }

    public void SetCursorActive(bool active)
    {
        Cursor.gameObject.SetActive(active);
    }

    public void SetDialogueActive(bool active)
    {
        Dialogue.gameObject.SetActive(active);
    }

    private void TutorialEnd()
    {
        // 봉인된 오브젝트들 해제
        foreach (GameObject go in hiddenDuringTutorial)
        {
            go.SetActive(true);
        }

        Managers.Player.HasReceivedTutorialGold = true;
        Managers.Player.HasReceivedTutorialGem = true;
        Managers.Player.LastTutorialStep = Enums.TutorialStep.End; // 진행도 저장
        Managers.Wave.GameStart();
        Managers.Resource.Instantiate("QuestRepeatUI");
        Managers.Resource.Destroy(this.gameObject, true); // 제거
    }

    public void OverlayOff()
    {
        for (int i = 0; i < 4; i++)
        {
            MenuButtons[i].SetActive(false);
        }
    }

    public void OverlayOn(int index)
    {
        MenuButtons[index].SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            if (i == index) continue;
            MenuButtons[i].SetActive(true);
        }
    }

    #region 커서위치 구하는 메서드
    public Vector3 GetTabPosition(int index)
    {
        OverlayOn(index);
        Transform tabBtn = MenuButtons[index].transform;
        RectTransform rectTabBtn = tabBtn as RectTransform;
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, rectTabBtn.position); // 버튼의 화면 상 위치

        // screenPos를 커서의 앵커 기준 로컬 좌표로 변환
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle
                    (
                        CursorRect,
                        screenPos,
                        null,
                        out localPos
                    );
        return localPos;
    }

    public Vector3 GetObjPos(GameObject go)
    {
        Transform targetGO = go.transform;
        RectTransform rect = targetGO as RectTransform;
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, rect.position); // 버튼의 화면 상 위치

        // screenPos를 커서의 앵커 기준 로컬 좌표로 변환
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle
                    (
                        CursorRect,
                        screenPos,
                        null,
                        out localPos
                    );
        return localPos;
    }
    #endregion
}
