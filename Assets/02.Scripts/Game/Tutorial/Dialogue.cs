using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public Image BG;
    [SerializeField] private TextMeshProUGUI TutorialTxt;
    [SerializeField] private Image Cursor;
    [SerializeField] private Button Bg;

    private Queue<string> txt = new Queue<string>();

    private void Awake()
    {
        txt.Enqueue("주인님 반갑습니다. \r\n시작하기 앞서, 던전 관리를 간단히 알려드리겠습니다.");
        txt.Enqueue("먼저 타워를 설치하는 방법을 알려드릴게요~");
        txt.Enqueue("");
        txt.Enqueue("타워는 최대 2개까지 설치가 됩니다. 이번에는 위치를 옮기는법을 알아볼까요?");
        txt.Enqueue("");
        txt.Enqueue("이번에 알려드릴 것은 연구입니다.\n 하단의 마우스를 클릭해주세요.");
        txt.Enqueue("");
        txt.Enqueue("골드나 잼을 통해 시간을 단축할 수 있고 그냥 기다리셔도 연구가 완료됩니다.");

        Bg.onClick.AddListener(TxtOnClick);
    }

    void Update()
    {
        
    }

    private void Start()
    {
        Time.timeScale = 0f;
        TutorialTxt.text = txt.Dequeue();
    }

    public void TxtOnClick()
    {
        if (txt.Peek() == "")
        {
            Time.timeScale = 1f;
            BG.gameObject.SetActive(false);
            Cursor.gameObject.SetActive(true);
        }
        TutorialTxt.text = txt.Dequeue();
    }

}
