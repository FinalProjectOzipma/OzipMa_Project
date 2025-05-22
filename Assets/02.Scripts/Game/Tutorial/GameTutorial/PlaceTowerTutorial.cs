using DefaultTable;
using UnityEngine;

/// <summary>
/// 타워 배치 튜토리얼
/// </summary>
public class PlaceTowerTutorial : TutorialBase
{
    private int dialogueNum = 0;
    private int prevTowerCount = 0;

    private UI_Main mainUI;

    public PlaceTowerTutorial(TutorialController _controller, Enums.TutorialStep step) : base(_controller, step)
    {
    }
    public override bool CheckCondition()
    {
        switch (dialogueNum)
        {
            case 0:
                // 관리탭 열렸는지 확인
                if (mainUI.isManagerOpen == true)
                {
                    dialogueNum++;
                    controller.ShowOnlyDialogue();

                    // 타워 배치 커서 세팅
                    Vector3 startPos = new Vector3(-315, -190, 0); // 인벤토리 첫번째 슬롯 위치
                    Vector3 endPos = new Vector3(-75, 393, 0);
                    controller.Cursor.Init(startPos, endPos, CursorType.Drag);
                }
                break;
            case 1:
                // 배치했는지 확인
                if (BuildingSystem.Instance.GridObjectMap.Count > prevTowerCount || prevTowerCount == 2)
                {
                    dialogueNum++;
                }
                break;
            default:
                // 모두 끝났으면 튜토리얼 끝
                return true;
        }
        return false;
    }

    public override void OnStart()
    {
        mainUI = Managers.UI.GetScene<UI_Main>();
        prevTowerCount = BuildingSystem.Instance.GridObjectMap.Count;

        Vector3 startPos = new Vector3(-370, -900, 0); // 관리탭 위치
        Vector3 endPos = new Vector3(-370, -900, 0);
        controller.Cursor.Init(startPos, endPos, CursorType.Click);

        controller.Dialogue.EnQueueText("주인님 반갑습니다.\n시작하기 앞서, 던전 관리를 간단히 알려드리겠습니다.");
        controller.Dialogue.EnQueueText("먼저 던전을 지키는 아군과 타워를 볼 수 있는 인벤토리를 알려드릴게요.");
        controller.Dialogue.EnQueueText("아래 탭의 첫번째 버튼을 눌러 인벤토리를 볼 수 있습니다.");
        controller.Dialogue.EnQueueText("");
        controller.Dialogue.EnQueueText("잘하셨어요!\n우리가 갖고있는 타워를 설치해서 던전을 지켜야해요.");
        controller.Dialogue.EnQueueText("인벤토리에서 타워를 드래그하여 배치해보세요.");
        controller.Dialogue.EnQueueText("몬스터는 드래그할 수 없으니까 타워인지 잘 확인해주셔야해요!!!!");
        controller.Dialogue.EnQueueText("");

        controller.SetDialogueActive(true);
    }
    public override void OnEnd()
    {
        base.OnEnd();

        mainUI.OFFSwipe();
    }
}
