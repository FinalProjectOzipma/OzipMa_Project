using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class TutorialController : UI_Scene
{
    // 튜토리얼 요소 
    public Cursor Cursor;
    public Dialogue Dialogue;

    private Queue<TutorialBase> queue = new();
    private TutorialBase currentTutorial;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        // 튜토리얼을 순서대로 넣기 
        switch(Managers.Player.LastTutorialStep)
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
        Managers.Player.HasReceivedTutorialGold = true;
        Managers.Player.HasReceivedTutorialGem = true;
        Managers.Player.LastTutorialStep = Enums.TutorialStep.End; // 진행도 저장
        Managers.Wave.GameStart();
        Managers.Resource.Destroy(this.gameObject, true); // 제거
    }
}
