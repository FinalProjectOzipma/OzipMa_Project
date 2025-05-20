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

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        // 튜토리얼을 순서대로 넣기 
        queue.Enqueue(new PlaceTowerTutorial(this));
        //queue.Enqueue(new PlaceTowerTutorial());


        // 첫번째 튜토리얼부터 시작
        NextTutorial();
    }

    public void NextTutorial()
    {
        if (queue.Count > 0)
            queue.Dequeue().OnStart();
        else
            Destroy(this.gameObject); // 모든 튜토리얼 털면 제거
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
