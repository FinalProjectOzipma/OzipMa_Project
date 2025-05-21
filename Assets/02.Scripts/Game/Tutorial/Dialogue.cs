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
    private Queue<string> txt = new();

    /// <summary>
    /// 출력할 대사를 넣는 메서드(큐임)
    /// </summary>
    /// <param name="s">대사</param>
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

    private void OnEnable()
    {
        //초기화
        Init();
    }

    /// <summary>
    /// 대화 연출 나올때 기본 세팅
    /// </summary>
    public override void Init()
    {
        IsEnd = false;
        TutorialTxt.text = txt.Dequeue();
        TutorialTxt.maxVisibleCharacters = 0;
        Play = StartCoroutine(Typing());
        Managers.Audio.PlaySFX(SFXClipName.Guide);
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
            Managers.Audio.PlaySFX(SFXClipName.Guide);
            Play = null;
            return;
        }

        if (txt.Peek().Equals(""))
        {
            IsEnd = true;
            txt.Dequeue();
            return;
        }

        TutorialTxt.text = txt.Dequeue();
        TutorialTxt.maxVisibleCharacters = 0;
        Play = StartCoroutine(Typing());
        Managers.Audio.PlaySFX(SFXClipName.Guide);
    }

    /// <summary>
    /// 타이핑 애니메이션 연출
    /// </summary>
    /// <returns></returns>
    private IEnumerator Typing()
    {
        int len = TutorialTxt.text.Length;
        for (int i = 0; i<len; i++)
        {
            TutorialTxt.maxVisibleCharacters = i + 1;
            yield return time;
        }
        Play = null;
    }
}
