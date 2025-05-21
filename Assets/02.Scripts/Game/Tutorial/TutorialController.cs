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
        //queue.Enqueue(new PlaceTowerTutorial(this));
        //queue.Enqueue(new EditTowerTutorial(this));
        //queue.Enqueue(new DeleteTowerTutorial(this));
        queue.Enqueue(new ResearchTutorial(this));

        // 튜토리얼 시작
        SetDialogueActive(true);
        SetCursorActive(false);
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
            Managers.Wave.GameStart();
            Managers.Resource.Destroy(this.gameObject, true); // 제거
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
}
