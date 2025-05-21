using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestRepeat : UI_Popup
{
    [SerializeField] private Button QuestAlarm;

    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI Progress;
    [SerializeField] private TextMeshProUGUI RewardText;

    List<QuestData> reppeatQuestList;

    public QuestData RepeatQuestData;


    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        SetData();
    }

    public override void Init()
    {
        reppeatQuestList = Managers.Quest.GetQuestList(QuestType.Repeat);
        QuestAlarm.onClick.AddListener(CompleteQuest);

        SetData();
    }


    public void SetData()
    {
        int ranmoNumber = Random.Range(0, reppeatQuestList.Count);

        RepeatQuestData = reppeatQuestList[ranmoNumber];
        Description.text = RepeatQuestData.Description;
        Progress.text = $"{RepeatQuestData.Progress} / {RepeatQuestData.Goal}";
        RewardText.text = Util.FormatNumber((long)RepeatQuestData.RewardGem);
        RepeatQuestData.State = QuestState.Done;

    }


    public void CompleteQuest()
    {
        if(RepeatQuestData.State == QuestState.Doing)
        {
            Managers.UI.Notify("퀘스트 진행 중 입니다.", false);
            return;
        }

        RepeatQuestData.Progress = 0;
        RepeatQuestData.Goal *= 2;
        RepeatQuestData.RewardGem *= 2;
        RepeatQuestData.State = QuestState.Complete;

        Managers.Player.AddGem(RepeatQuestData.RewardGem);

        SetData();
    }

}
