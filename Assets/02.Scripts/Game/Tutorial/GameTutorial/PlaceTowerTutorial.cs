using DefaultTable;
using System.Linq;
using UnityEngine;

/// <summary>
/// 타워 배치 튜토리얼
/// </summary>
public class PlaceTowerTutorial : TutorialBase
{
    private int dialogueNum = 0;
    private int prevTowerCount = 0;

    public PlaceTowerTutorial(TutorialController _controller, Enums.TutorialStep step) : base(_controller, step)
    {
    }
    public override bool CheckCondition()
    {
        switch (dialogueNum)
        {
            case 0:
                // 관리탭 열렸는지 확인
                if (controller.MainUI.isManagerOpen == true)
                {
                    dialogueNum++;
                    controller.ShowOnlyDialogue();

                    // 타워 배치 커서 세팅
                    Vector3 startPos = controller.GetObjPos(controller.SlotPosition); // 인벤토리 첫번째 슬롯 위치

                    Vector3Int endPoint = BuildingSystem.Instance.GetCurMapHandler().BuildHighlightList[0]; // 아무 칸 위치
                    Vector3 endPos = BuildingSystem.Instance.CellToWorldPos(endPoint);
                    endPos = Camera.main.WorldToScreenPoint(endPos);
                    RectTransform rect = controller.Cursor.GetComponent<RectTransform>();
                    RectTransformUtility.ScreenPointToLocalPointInRectangle
                                (
                                    rect.parent as RectTransform,
                                    endPos,
                                    null,
                                    out Vector2 localPos
                                );
                    endPos = localPos;
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
        prevTowerCount = BuildingSystem.Instance.GridObjectMap.Count;

        Vector3 startPos = controller.GetTabPosition(0); // 관리탭 위치
        Vector3 endPos = startPos;
        controller.Cursor.Init(startPos, endPos, CursorType.Click);

        controller.Dialogue.EnQueueText("주인님, 드디어 오셨군요!");
        controller.Dialogue.EnQueueText("요즘 모험가들이 자꾸 우리 집을 침입하려고 해요!");
        controller.Dialogue.EnQueueText("본격적으로 시작하기 전에,\n<color=#f5a545>던전 관리법</color>부터 간단히 알려드리겠습니다.");
        controller.Dialogue.EnQueueText("먼저 던전을 지키는 아군과 타워를 볼 수 있는 <color=#f5a545>인벤토리</color>를 알려드릴게요.");
        controller.Dialogue.EnQueueText("아래 탭의 첫번째 버튼을 눌러 인벤토리를 볼 수 있습니다.");
        controller.Dialogue.EnQueueText("");
        controller.Dialogue.EnQueueText("잘하셨어요!\n우리가 갖고있는 타워를 설치해서 던전을 지켜야해요.");
        controller.Dialogue.EnQueueText("인벤토리에서 타워를 <color=#f5a545>드래그</color>하여 배치해보세요.");
        controller.Dialogue.EnQueueText("몬스터는 드래그할 수 없으니까 타워인지 잘 확인해 주셔야 해요!!!!");
        controller.Dialogue.EnQueueText("");

        controller.SetDialogueActive(true);
    }
    public override void OnEnd()
    {
        base.OnEnd();

        controller.MainUI.OFFSwipe();
        controller.MainUI.OFFManagerMenu();
    }
}
