using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Cursor : MonoBehaviour
{
    [SerializeField]private Animator anim;

    private int towerHash;
    private int towerClick;
    private int towerBuild;
    private int towerDrag;
    private int towerDestroy;

    private int researchHash;
    private int researchClick;
    private int researchStart;

    private Tutorial tuto;
    private UI_Main main;

    private Action cursorTodo;

    private void OnValidate()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    private void Awake()
    {
        towerHash = Animator.StringToHash("Tower");
        towerClick = Animator.StringToHash("TowerClick");
        towerBuild = Animator.StringToHash("TowerBuild");
        towerDrag = Animator.StringToHash("TowerDrag");
        towerDestroy = Animator.StringToHash("TowerDestroy");

        researchHash = Animator.StringToHash("Research");
        researchClick = Animator.StringToHash("ResearchClick");
        researchStart = Animator.StringToHash("ResearchStart");

        cursorTodo += IsInvenClick;
    }

    private void Start()
    {
        main = FindObjectOfType<UI_Main>();
        tuto = FindObjectOfType<Tutorial>();
    }

    private void OnEnable()
    {
        Time.timeScale = 1.0f;
    }

    public void AnimTrigger()
    {
        cursorTodo.Invoke();
    }

    //클릭 확인용
    private void IsInvenClick()
    {
        //인벤토리 올라왔는지 확인
        if (main.isManagerOpen)
        {
            anim.SetBool(towerHash, false);
            anim.SetBool(towerClick, true);
            cursorTodo -= IsInvenClick;
            cursorTodo += IsTowerBuild;
        }
    }

    private void IsTowerBuild()
    {
        //타워 설치되었는지 확인
        if (BuildingSystem.Instance.CurrentTowerCount > 0)
        {
            anim.SetBool(towerClick, false);
            anim.SetBool(towerBuild, true);

            cursorTodo -= IsTowerBuild;
            cursorTodo += IsTowerDrag;


            tuto.BG.gameObject.SetActive(true);
            tuto.TxtOnClick();
            Time.timeScale = 0f;
            gameObject.SetActive(false);
        }
    }

    private void IsTowerDrag()
    {
        //타워드래그되었는지 확인
        if (BuildingSystem.Instance.DragController.TutorialIsDrag)
        {
            anim.SetBool(towerBuild, false);
            anim.SetBool(towerDrag, true);

            cursorTodo -= IsTowerBuild;
            cursorTodo += IsTowerDestroy;

            gameObject.SetActive(false);
            tuto.BG.gameObject.SetActive(true);
            tuto.TxtOnClick();
        }
    }

    private void IsTowerDestroy()
    {
        //타워드래그되었는지 확인
        if (BuildingSystem.Instance.DragController.TutorialIsDrag)
        {
            anim.SetBool(towerBuild, true);
            anim.SetBool(towerClick, false);

            cursorTodo -= IsTowerDestroy;
            cursorTodo += IsResearchClick;

            gameObject.SetActive(false);
            tuto.BG.gameObject.SetActive(true);
            tuto.TxtOnClick();
        }
    }

    private void IsResearchClick()
    {
        //연구 열렸는지 확인
        if (main.isResearchOpen)
        {
            anim.SetBool(researchClick, true);
            cursorTodo -= IsResearchClick;
            gameObject.SetActive(false);
        }
    }
}
