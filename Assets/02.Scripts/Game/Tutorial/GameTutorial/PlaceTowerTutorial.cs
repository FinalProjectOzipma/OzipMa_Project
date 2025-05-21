using UnityEngine;

/// <summary>
/// 타워 배치 튜토리얼
/// </summary>
public class PlaceTowerTutorial : TutorialBase
{
    private int prevTowerCount = 0;
    public PlaceTowerTutorial(TutorialController _controller) : base(_controller)
    {
    }
    public override bool CheckCondition()
    {
        if (BuildingSystem.Instance.GridObjectMap.Count > prevTowerCount || prevTowerCount == 2)
        {
            return true;
        }
        return false;
    }

    public override void OnStart()
    {
        prevTowerCount = BuildingSystem.Instance.GridObjectMap.Count;

        Vector3 startPos = new Vector3(-260, -110, 0); // 인벤토리 첫번째 슬롯 위치
        Vector3 endPos = new Vector3(-40, 270, 0); 
        controller.Cursor.Init(startPos, endPos, CursorType.Drag);

        controller.Dialogue.EnQueueText("주인님 반갑습니다. \r\n시작하기 앞서, 던전 관리를 간단히 알려드리겠습니다.");
        controller.Dialogue.EnQueueText("먼저 타워를 설치하는 방법을 알려드릴게요~");
        controller.Dialogue.EnQueueText("인벤토리에서 타워를 드래그하여 배치해보세요.");
        controller.Dialogue.EnQueueText("");

        controller.SetDialogueActive(true);
    }
    public override void OnEnd()
    {
        base.OnEnd();

        // TODO :: 튜토리얼 보상 처리
    }
}
