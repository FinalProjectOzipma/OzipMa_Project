using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour 
{
    public Image BG;

    public bool IsEnd;
    [SerializeField] private TextMeshProUGUI TutorialTxt;
    [SerializeField] private Image Cursor;
    [SerializeField] private Button Bg;

    private Queue<string> txt;

    public void EnQueueText(string s)
    {
        txt.Enqueue(s);
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        Time.timeScale = 0f;
        TutorialTxt.text = txt.Dequeue();
    }

    public void TxtOnClick()
    {
        if (txt.Count == 0)
        {
            IsEnd = true;
            Time.timeScale = 1f;
            return;
        }

        TutorialTxt.text = txt.Dequeue();
    }
}
