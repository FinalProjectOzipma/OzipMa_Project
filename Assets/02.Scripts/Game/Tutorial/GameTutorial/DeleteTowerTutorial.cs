using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeleteTowerTutorial : TutorialBase
{
    public DeleteTowerTutorial(TutorialController _controller) : base(_controller)
    {
    }

    public override bool CheckCondition()
    {
        // 타워 편집이 이루어지면
        if (BuildingSystem.Instance.DragController.TutorialDragCheck == true)
        {
            return true;
        }
        return false;
    }

    public override void OnStart()
    {
        // 설치된 타워 위치 아무거나
        Vector3 startPos = BuildingSystem.Instance.GridObjectMap.Keys.First();
        //controller.Cursor.Init(startPos)

        //
        controller.Dialogue.EnQueueText("잘하셨어요!");
        controller.Dialogue.EnQueueText("이번에는 편집모드에서 X버튼을 눌러 타워를 제거해보세요.");
        controller.Dialogue.EnQueueText("");

        controller.SetDialogueActive(true);
    }

    public override void OnEnd()
    {
        base.OnEnd();

        // TODO :: 보상
    }
}
