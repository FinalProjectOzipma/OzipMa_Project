using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

[Serializable]
public class TutorialButton: MonoBehaviour
{
    [SerializeField] private RectTransform rect;

    [SerializeField] private bool hasClickCondition;
    [SerializeField] private bool hasReleaseCondition;
    [SerializeField] private bool hasMouseOverCondition;
    [SerializeField] private float endTime;

    public event Action onEnd;

    private bool isGoal = false;
    private bool isTimeOver = false;
    private bool isMouseUp = false;
    private bool isMouseDown = false;

    private float startTime = 0f;

    private Cursor mouseInfo;
    public void Setup(Cursor mouseInfo)
    {
        this.mouseInfo = mouseInfo;
        isGoal = false;
        isTimeOver = false;
        isMouseUp = false;
        isMouseDown = false;
    }

    void Update()
    {
        if (GoalCheck() && TimeCheck() && isMouseUp && isMouseDown)
        {
            onEnd?.Invoke();
        }

    }

    private bool GoalCheck()
    {
        if (isGoal)
            return true;
        //부모 기준으로 얼마나 떨어져있는지 계산함
        Vector3 mousePos = rect.parent.InverseTransformPoint(mouseInfo.transform.position);
        if (Vector3.Distance(mousePos, Vector3.zero) < 0.1f)
        { 
            isGoal = true;
            startTime = Time.time;
            return true;
        }

        return false;
    }

    private bool TimeCheck()
    {
        if (isTimeOver)
            return true;

        if (!isGoal)
            return false;

        if (Time.time - startTime > endTime)
        {
            isTimeOver = true;
            return true;
        }

        return false;
    }



    public void OnPointerUp(PointerEventData eventData)
    {
        isMouseUp = true;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isMouseDown = true;
    }
}

