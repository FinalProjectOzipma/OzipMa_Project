using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI_QuestSlot : UI_Base
{
    [SerializeField] private TextMeshProUGUI RewardText;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI GoalValueText;
    [SerializeField] private TextMeshProUGUI Name;

    [SerializeField] private Slider ProgressSlider;

    [SerializeField] private GameObject CompleteImage;
    [SerializeField] private Button CheckButton;

    public int Index;
    public QuestData questData;


    private void Start()
    {
        CheckButton.onClick.AddListener(OnClickGetReward);
    }

    private void OnDisable()
    {
        questData.OnProgressChanged -= UpdateUI;
        questData.OnStateChanged -= ChangedButton;
    }


    public void SetData(QuestData questData)
    {
        this.questData = questData;
        Name.text = questData.Name;
        RewardText.text = Util.FormatNumber(questData.RewardGem);
        Description.text = questData.Description;

        questData.OnProgressChanged += UpdateUI;
        questData.OnStateChanged += ChangedButton;

        UpdateUI();
        ChangedButton(questData.State);

    }

    public void OnClickGetReward()
    {
        if (questData.Progress != questData.Goal)
        {
            Managers.UI.Notify("퀘스트가 진행 중입니다.", false);
            return;
        }

        questData.State = QuestState.Complete;
        questData.OnStateChanged?.Invoke(questData.State);
        Managers.Player.AddGem(questData.RewardGem);

    }

    public void UpdateUI()
    {
        GoalValueText.text = $"{questData.Progress} / {questData.Goal}";
        ProgressSlider.value = (float)questData.Progress / questData.Goal;

    }

    public bool CheckIsComplete()
    {
        return CompleteImage.activeSelf;
    }

    public void ChangedButton(QuestState questState)
    {
        if (questData.State == QuestState.Done)
        {
            CheckButton.interactable = true;
            CompleteImage.SetActive(false);
        }
        else if (questData.State == QuestState.Doing)
        {
            CheckButton.interactable = false;
            CompleteImage.SetActive(false);
        }
        else
        {
            CheckButton.interactable = false;
            CompleteImage.SetActive(true);
        }
    }


}
