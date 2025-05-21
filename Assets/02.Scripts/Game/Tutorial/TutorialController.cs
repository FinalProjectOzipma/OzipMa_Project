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
        queue.Enqueue(new PlaceTowerTutorial(this));
        queue.Enqueue(new EditTowerTutorial(this));

        // 첫번째 튜토리얼부터 시작
        NextTutorial();
    }

    private void Update()
    {
        // 대화끝나면 커서 보여주기
        if(Dialogue.IsEnd == true)
        {
            SetCursorActive(true);
            SetDialogueActive(false);
        }

        // 튜토리얼 조건 만족시 다음 튜토리얼로 넘김
        if(currentTutorial.CheckCondition() == true)
        {
            NextTutorial();
        }
    }

    public void NextTutorial()
    {
        if (queue.Count > 0)
        {
            currentTutorial = queue.Dequeue();
            currentTutorial.OnStart();
        }
        else // 모든 튜토리얼 완료했으면
        {
            Managers.Wave.GameStart();
            Managers.Resource.Destroy(this.gameObject, true); // 제거
        }
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
