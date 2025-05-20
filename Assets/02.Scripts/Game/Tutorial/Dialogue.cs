using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : UI_Base
{
    // 타이핑 간격시간
    public float timegap;
    //대화 종료 bool값
    public bool IsEnd;

    //타이핑 애니메이션 코루틴
    private Coroutine Play;
    private WaitForSeconds time;

    //튜토리얼 대사
    [SerializeField] private TextMeshProUGUI TutorialTxt;
    //대사 넘기는 버튼
    [SerializeField] private Button Box;

    //대사 발사대
    private Queue<string> txt;

    /// <summary>
    /// 출력할 대사를 넣는 메서드(큐임)
    /// </summary>
    /// <param name="s"></param>
    public void EnQueueText(string s)
    {
        txt.Enqueue(s);
    }

    private void Awake()
    {
        //시간 세팅
        time = new WaitForSeconds(timegap);
        //버튼메서드 등록
        Box.onClick.AddListener(TxtOnClick);
    }

    private void Start()
    {
        //초기화
        Init();
    }

    /// <summary>
    /// 대화 연출 나올때 기본 세팅
    /// </summary>
    public void Init()
    {
        Time.timeScale = 0f;
        TutorialTxt.text = txt.Dequeue();
    }

    /// <summary>
    /// 클릭 했을때 대화 메서드
    /// </summary>
    public void TxtOnClick()
    {
        //누르면 텍스트연출 스킵시키기
        if (Play != null)
        {
            StopCoroutine(Play);
            TutorialTxt.maxVisibleCharacters = TutorialTxt.text.Length;
            Play = null;
            return;
        }

        if (txt.Count == 0)
        {
            IsEnd = true;
            Time.timeScale = 1f;
            return;
        }

        TutorialTxt.text = txt.Dequeue();
        TutorialTxt.maxVisibleCharacters = 0;
        Play = StartCoroutine(Typing());
    }

    //타이핑 애니메이션 연출
    private IEnumerator Typing()
    {
        int len = TutorialTxt.text.Length;
        for (int i = 0; i<len; i++)
        {
            TutorialTxt.maxVisibleCharacters = i + 1;
            yield return time;
        }
    }
}
