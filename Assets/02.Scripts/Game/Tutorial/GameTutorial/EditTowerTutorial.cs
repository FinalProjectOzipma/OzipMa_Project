using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 타워 편집 튜토리얼
/// </summary>
public class EditTowerTutorial : TutorialBase
{
    public EditTowerTutorial(TutorialController _controller, Enums.TutorialStep step) : base(_controller, step)
    {
    }

    public override bool CheckCondition()
    {
        // 타워 편집이 이루어지면
        if(BuildingSystem.Instance.DragController.TutorialDragCheck == true)
        {
            return true;
        }
        return false;
    }

    public override void OnStart()
    {
        // 설치된 타워 위치 아무거나 start위치로 사용
        Vector3Int startPoint = BuildingSystem.Instance.GridObjectMap.Keys.First();
        Vector3 startPos = BuildingSystem.Instance.CellToWorldPos(startPoint);
        startPos = Camera.main.WorldToScreenPoint(startPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle
                    (
                        controller.CursorRect,
                        startPos,
                        null,
                        out Vector2 localPos
                    );
        startPos = localPos;

        // 아무 빈공간을 다음 end위치로 사용
        Vector3 endPos = Vector3.zero;
        foreach (Vector3Int emptyPoint in BuildingSystem.Instance.GetCurMapHandler().BuildHighlightList)
        {
            if (startPoint != emptyPoint && !BuildingSystem.Instance.GridObjectMap.ContainsKey(emptyPoint))
            {
                // 다른 빈공간을 endPos로 사용
                endPos = BuildingSystem.Instance.CellToWorldPos(emptyPoint);
                endPos = Camera.main.WorldToScreenPoint(endPos);
                RectTransformUtility.ScreenPointToLocalPointInRectangle
                    (
                        controller.CursorRect,
                        endPos,
                        null,
                        out Vector2 localPos2
                    );
                endPos = localPos2;
                break;
            }
        }
        controller.Cursor.Init(startPos, endPos, CursorType.ClickDrag);

        // 드래그 체크용 bool 초기화
        BuildingSystem.Instance.DragController.TutorialDragCheck = false;
        
        controller.Dialogue.EnQueueText("잘하셨어요!\n타워는 최대 2개까지 설치가 됩니다.");
        controller.Dialogue.EnQueueText("배치된 타워를 꾹- 누르면 <color=#f5a545>편집모드</color>를 활성화할 수 있어요.");
        controller.Dialogue.EnQueueText("편집모드에서 타워를 <color=#f5a545>드래그</color>로 옮겨보세요.");
        controller.Dialogue.EnQueueText("");

        controller.SetDialogueActive(true);
    }

    public override void OnEnd()
    {
        base.OnEnd();

        controller.OverlayOff();
    }
}
