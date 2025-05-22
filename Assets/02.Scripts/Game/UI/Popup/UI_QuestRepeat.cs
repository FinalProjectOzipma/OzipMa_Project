using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestRepeat : UI_Scene
{
    [SerializeField] private Button QuestAlarm;

    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI Progress;
    [SerializeField] private TextMeshProUGUI RewardText;
    [SerializeField] private Image Icon;

    [SerializeField] private Image AlarmImage;

    private SpriteRenderer sprite;

    List<QuestData> repeatQuestList;

    public QuestData RepeatQuestData;

    private Tween blinkTween;


    private void Awake()
    {
        Init();
    }


    public override void Init()
    {
        Util.Log("반복퀘 데이터 세팅");

        Managers.UI.SetSceneList(this);
        repeatQuestList = Managers.Quest.GetQuestList(QuestType.Repeat);     
        QuestAlarm.onClick.AddListener(CompleteQuest);
        SetData();

        PlayMoveAnimation();
    }

    public void SetData()
    {
        int ranmoNumber = Random.Range(0, repeatQuestList.Count); 

        RepeatQuestData = repeatQuestList[ranmoNumber];

        RepeatQuestData.OnProgressChanged += UpdateProgress;
        RepeatQuestData.OnStateChanged += OnQuestStateChanged;

        OnQuestStateChanged(RepeatQuestData.State);
        UpdateProgress();

        RepeatQuestData.IsActive = 1;
        RepeatQuestData.Progress = 0;
        RepeatQuestData.State = QuestState.Doing;

        Description.text = RepeatQuestData.Description;
        RewardText.text = Util.FormatNumber((long)RepeatQuestData.RewardGem);
        RepeatQuestData.State = QuestState.Doing;
    }

    public void CompleteQuest()
    {
        if(RepeatQuestData.State != QuestState.Done)
        {
            Managers.UI.Notify("퀘스트 진행 중 입니다.", false);
            Util.Log("진행 상태 :" + RepeatQuestData.State.ToString());
            Util.Log("퀘스트 설명 :" + RepeatQuestData.Description);
            Util.Log("퀘스트 진행 상황 : " + RepeatQuestData.Progress.ToString());
            return;
        }


        if (blinkTween != null && blinkTween.IsActive())
        {
            blinkTween.Kill(); // 깜빡임 종료
            AlarmImage.DOFade(0f, 0.2f); // 알파값 복원
            blinkTween = null;
        }

        RepeatQuestData.OnProgressChanged -= UpdateProgress;
        RepeatQuestData.OnStateChanged -= OnQuestStateChanged;
        RepeatQuestData.Progress = 0;
        RepeatQuestData.State = QuestState.Complete;
        RepeatQuestData.IsActive = 0;
        Managers.Player.AddGem(RepeatQuestData.RewardGem);

        SetData();

        PlayMoveAnimation();

        uiSeq.Play();


    }

    public void UpdateProgress()
    {
        Progress.text = $"{RepeatQuestData.Progress} / {RepeatQuestData.Goal}";
    }


    void PlayMoveAnimation()
    {
        uiSeq = DOTween.Sequence();

        Vector3 originalPos = QuestAlarm.gameObject.transform.localPosition;
        float moveDistance = 350f; // 이동할 거리 (예: 오른쪽으로 350)
        float duration = 0.5f;     // 애니메이션 시간

        uiSeq.Append(QuestAlarm.gameObject.transform.DOLocalMoveX(originalPos.x + moveDistance, duration))
             .Append(QuestAlarm.gameObject.transform.DOLocalMoveX(originalPos.x, duration));
    }

    public void OnQuestStateChanged(QuestState newState)
    {
        if (newState == QuestState.Done)
        {
            uiSeq = DOTween.Sequence();
            if (blinkTween != null && blinkTween.IsActive()) return;

            blinkTween = AlarmImage.DOFade(0.5f, 0.7f)
                                   .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
