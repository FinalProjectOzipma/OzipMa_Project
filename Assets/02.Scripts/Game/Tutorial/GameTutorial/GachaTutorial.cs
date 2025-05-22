using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaTutorial : TutorialBase
{
    private int dialogueNum = 0;

    private UI_Main mainUI;
    private GachaUI gachaUI;
    private UI_Dictionary dictionaryUI;

    public GachaTutorial(TutorialController _controller, Enums.TutorialStep step) : base(_controller, step)
    {
    }

    public override bool CheckCondition()
    {
        switch (dialogueNum)
        {
            case 0:
                // 가챠탭 열렸는지 확인
                if (mainUI.isGachaOpne == true)
                {
                    dialogueNum++;
                    controller.ShowOnlyDialogue();

                    // 10개 뽑기 버튼 커서 세팅
                    Vector3 startPos = new Vector3(55, 150, 0); 
                    Vector3 endPos = new Vector3(55, 150, 0);
                    controller.Cursor.Init(startPos, endPos, CursorType.Click);

                    Managers.Player.AddGem(3000);

                    gachaUI = Managers.UI.GetPopup<GachaUI>();
                }
                break;
            case 1:
                // 가챠 여부 확인
                if (gachaUI.IsGachaInProgress == true)
                {
                    dialogueNum++;
                    controller.ShowOnlyDialogue();

                    // 도감 탭 위치로 커서 세팅
                    Vector3 startPos = new Vector3(-142, -871, 0);
                    Vector3 endPos = new Vector3(-142, -871, 0);
                    controller.Cursor.Init(startPos, endPos, CursorType.Click);
                }
                break;
            case 2:
                // 도감 탭 열기
                if (mainUI.isDictionaryOpne == true)
                {
                    dialogueNum++;
                    controller.ShowOnlyDialogue();

                    // 도감 정보창 위치로 커서 세팅
                    Vector3 startPos = new Vector3(-225, 130, 0);
                    Vector3 endPos = new Vector3(-225, 130, 0);
                    controller.Cursor.Init(startPos, endPos, CursorType.Click);

                    dictionaryUI = Managers.UI.GetPopup<UI_Dictionary>();
                }
                break;
            case 3:
                // 도감 정보창 열기
                if (dictionaryUI.IsSlotOpen() == true)
                {
                    dialogueNum++;
                    controller.ShowOnlyDialogue();

                    Managers.Player.AddGold(5000);
                    Managers.Player.AddGem(5000);
                }
                break;
            case 4:
                //마무리 멘트 모두 나왔는지 확인
                if (controller.Dialogue.IsEmpty == true)
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

    public override void OnEnd()
    {
        base.OnEnd();

        Managers.UI.CloseAllPopupUI();
    }

    public override void OnStart()
    {
        mainUI = Managers.UI.GetScene<UI_Main>();

        Vector3 startPos = new Vector3(470, -871, 0); // 가챠탭 위치
        Vector3 endPos = new Vector3(470, -871, 0);
        controller.Cursor.Init(startPos, endPos, CursorType.Click);

        controller.Dialogue.EnQueueText("마지막으로 아군과 타워를 추가로 소환할 수 있는 뽑기 시스템을 알려드리겠습니다.");
        controller.Dialogue.EnQueueText("아래 탭의 네 번째 버튼을 눌러 뽑기 탭을 열어보세요.");
        controller.Dialogue.EnQueueText("");
        controller.Dialogue.EnQueueText("잘하셨어요!\n진짜 마지막 비상금(ㅠㅠ)을 넣어드렸으니 아군을 소환해보세요.");
        controller.Dialogue.EnQueueText("");
        controller.Dialogue.EnQueueText("축하드립니다!\n중복으로 소환되는 아군과 타워는 나중에 강화 재료로 쓰인답니다!");
        controller.Dialogue.EnQueueText("소환된 아군과 타워들의 정보가 궁금하시죠!!!");
        controller.Dialogue.EnQueueText(".. 사실 안 궁금하다고 하셔도 알려드릴 거예욧!!!");
        controller.Dialogue.EnQueueText("아래 탭의 두 번째 버튼을 눌러서 도감창을 열어보세요.");
        controller.Dialogue.EnQueueText("");
        controller.Dialogue.EnQueueText("이곳에서 도감의 유닛들과 타워들을 선택하면 정보들을 볼 수 있어요. 한번 열어보시겠어요?");
        controller.Dialogue.EnQueueText("");
        controller.Dialogue.EnQueueText("정말 멋져요!\n모든 튜토리얼을 완료하였습니다.");
        controller.Dialogue.EnQueueText("진짜 진짜 마지막으로!\n제가 가진 모든 전재산(...)을 드릴게요!\n+5000Gold\n+5000Gem");
        controller.Dialogue.EnQueueText("앞으로 우리의 집을 멋지게 지켜주세요!!");
        controller.Dialogue.EnQueueText("");

        controller.SetDialogueActive(true);
    }
}
