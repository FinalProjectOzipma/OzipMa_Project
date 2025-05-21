using UnityEngine;

/// <summary>
/// 타워 배치 튜토리얼
/// </summary>
public class PlaceTowerTutorial : TutorialBase
{
    public PlaceTowerTutorial(TutorialController _controller) : base(_controller)
    {
    }
    public override bool CheckCondition()
    {
        if (BuildingSystem.Instance.GridObjectMap.Count > 0)
        {
            return true;
        }
        return false;
    }

    public override void OnStart()
    {
        Vector3 startPos = new Vector3(-260, -110, 0); // 인벤토리 첫번째 슬롯 위치
        Vector3 endPos = new Vector3(-40, 270, 0); ;
        controller.Cursor.Init(startPos, endPos);

        controller.Dialogue.EnQueueText("주인님 반갑습니다. \r\n시작하기 앞서, 던전 관리를 간단히 알려드리겠습니다.");
        controller.Dialogue.EnQueueText("먼저 타워를 설치하는 방법을 알려드릴게요~");
        controller.Dialogue.EnQueueText("인벤토리에서 타워를 드래그하여 배치해보세요.");
        controller.Dialogue.EnQueueText("");

        // 이 아래는 다른 튜토리어들에 붙일거야 
        //controller.Dialogue.EnQueueText("이번에 알려드릴 것은 연구입니다.\n 하단의 마우스를 클릭해주세요.");
        //controller.Dialogue.EnQueueText("");
        //controller.Dialogue.EnQueueText("골드나 잼을 통해 시간을 단축할 수 있고 그냥 기다리셔도 연구가 완료됩니다.");

        controller.SetDialogueActive(true);
    }
    public override void OnEnd()
    {
        base.OnEnd();

        // TODO :: 튜토리얼 보상 처리
    }
}
