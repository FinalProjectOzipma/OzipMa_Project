using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeleteTowerTutorial : TutorialBase
{
    private RectTransform rect;
    private int prevTowerCount;

    public DeleteTowerTutorial(TutorialController _controller) : base(_controller)
    {
    }

    public override bool CheckCondition()
    {
        // 타워 삭제가 이루어지면
        if (prevTowerCount > BuildingSystem.Instance.CurrentTowerCount || BuildingSystem.Instance.CurrentTowerCount == 0)
        {
            return true;
        }
        return false;
    }

    public override void OnStart()
    {
        rect = controller.Cursor.GetComponent<RectTransform>();

        // 설치된 타워 위치 아무거나 start위치로 사용
        Vector3Int startPoint = BuildingSystem.Instance.GridObjectMap.Keys.First();
        Vector3 startPos = BuildingSystem.Instance.CellToWorldPos(startPoint);
        startPos = Camera.main.WorldToScreenPoint(startPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle
                    (
                        rect.parent as RectTransform,
                        startPos,
                        null,
                        out Vector2 localPos
                    );
        startPos = localPos;

        Vector3 endPos = startPos;
        endPos.x += 30f;
        endPos.y -= 100f;
        controller.Cursor.Init(startPos, endPos, CursorType.ClickDragClick);

        
        controller.Dialogue.EnQueueText("잘하셨어요!");
        controller.Dialogue.EnQueueText("이번에는 편집모드에서 X버튼을 눌러 타워를 제거해보세요.");
        controller.Dialogue.EnQueueText("");

        controller.SetDialogueActive(true);
        prevTowerCount = BuildingSystem.Instance.CurrentTowerCount;
    }

    public override void OnEnd()
    {
        base.OnEnd();

        // TODO :: 보상
    }
}
