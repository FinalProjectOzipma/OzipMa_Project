using DefaultTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchTutorial : TutorialBase
{
    private UI_Main mainUI;
    private UI_ResearchScene researchUI;
    private int dialogueNum = 0;
    private bool skip = false;

    public ResearchTutorial(TutorialController _controller) : base(_controller)
    {
    }
    public override bool CheckCondition()
    {
        if(skip) return true;

        switch (dialogueNum)
        {
            case 0:
                // 연구탭 열렸는지 확인
                if(mainUI.isResearchOpen == true)
                {
                    dialogueNum++;
                    controller.ShowOnlyDialogue();

                    // 연구 시작 버튼 커서 세팅
                    Vector3 startPos = new Vector3(345, 315, 0);
                    Vector3 endPos = new Vector3(345, 315, 0);
                    controller.Cursor.Init(startPos, endPos, CursorType.Click);
                    researchUI = Managers.UI.GetPopup<UI_ResearchScene>();
                }
                break;
            case 1:
                // 연구버튼 눌렀는지 확인
                if(researchUI.AttackUpgrade.isResearching || researchUI.DefenceUpgrade.isResearching || researchUI.CoreUpgrade.isResearching)
                {
                    dialogueNum++;
                    controller.ShowOnlyDialogue();

                    // 시간 단축 커서 세팅
                    Vector3 startPos = new Vector3(-75, 242, 0);
                    Vector3 endPos = new Vector3(-75, 242, 0);
                    controller.Cursor.Init(startPos, endPos, CursorType.Click);
                    Managers.Player.AddGold(1000);
                }
                break;
            case 2:
                // 시간 단축 눌렀는지 확인
                if(researchUI.AttackUpgrade.isResearchGoldBtn || researchUI.DefenceUpgrade.isResearchGoldBtn || researchUI.CoreUpgrade.isResearchGoldBtn)
                {
                    dialogueNum++;
                    controller.ShowOnlyDialogue();

                    // 완료 버튼 커서 세팅
                    Vector3 startPos = new Vector3(345, 315, 0);
                    Vector3 endPos = new Vector3(345, 315, 0);
                    controller.Cursor.Init(startPos, endPos, CursorType.Click);
                }
                break;
            case 3:
                // 완료 버튼 눌렀는지 확인
                if (researchUI.AttackUpgrade.isResearchComplete || researchUI.DefenceUpgrade.isResearchComplete || researchUI.CoreUpgrade.isResearchComplete)
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
        if(Managers.Player.AttackResearchData != null || Managers.Player.DefenceResearchData != null || Managers.Player.CoreResearchData != null)
        {
            skip = true;
            return;
        }

        mainUI = Managers.UI.GetScene<UI_Main>();

        // 연구 탭 클릭 커서 세팅
        Vector3 startPos = new Vector3(140, -885, 0); 
        Vector3 endPos = new Vector3(140, -885, 0);
        controller.Cursor.Init(startPos, endPos, CursorType.Click);

        controller.Dialogue.EnQueueText("이번에 소개해드릴 기능은 연구입니다.\n먼저 연구 탭을 클릭해보시겠어요?");
        controller.Dialogue.EnQueueText("");
        controller.Dialogue.EnQueueText("좋아요!!\n연구를 통해 우리가 더 강해질 수 있어요.");
        controller.Dialogue.EnQueueText("연구 버튼을 클릭해보세요.");
        controller.Dialogue.EnQueueText("");
        controller.Dialogue.EnQueueText("좋아요!!\n그런데 연구가 시간이 좀 걸리네요?");
        controller.Dialogue.EnQueueText("이럴 땐 재화를 사용하여 연구시간을 단축시킬 수 있어요!");
        controller.Dialogue.EnQueueText("제 비상금..을 조금 드렸으니 골드로 시간을 단축시켜보시겠어요?");
        controller.Dialogue.EnQueueText("");
        controller.Dialogue.EnQueueText("마지막으로 완료 버튼을 클릭하면 연구가 완료됩니다!");
        controller.Dialogue.EnQueueText("");

        controller.SetDialogueActive(true);
    }
    public override void OnEnd()
    {
        base.OnEnd();

        // TODO :: 튜토리얼 보상 처리
    }
}
