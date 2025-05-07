using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_OFFLinePopup : UI_Popup
{
    [SerializeField] private Button CheckButton;
    [SerializeField] private Button DoubleButton;

    [SerializeField] private TextMeshProUGUI CheckText;
    [SerializeField] private TextMeshProUGUI DoubleText;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private TextMeshProUGUI RewordText;
    [SerializeField] private TextMeshProUGUI StageClearText;

    [SerializeField] private Image CheckImage;
    [SerializeField] private Image DoubleImage;

    [SerializeField] private GameObject UIOffLine;

    public override void Init()
    {
        
    }

}
