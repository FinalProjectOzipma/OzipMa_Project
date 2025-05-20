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

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        // 튜토리얼을 순서대로 넣기 
        queue.Enqueue(new PlaceTowerTutorial(this));
        //queue.Enqueue(new PlaceTowerTutorial());


        // 첫번째 튜토리얼부터 시작
        NextTutorial();
    }

    private void Update()
    {
        // TODO :: 대화끝나면 커서 보여주기 시작

        // TODO :: 튜토리얼 조건 만족시 넘어가
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
        else
        {
            Managers.Wave.GameStart();
            Managers.Resource.Destroy(this.gameObject, true); // 모든 튜토리얼 털면 제거
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
