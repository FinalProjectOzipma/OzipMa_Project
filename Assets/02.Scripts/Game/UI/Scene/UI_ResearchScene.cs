using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System.Numerics;

public class UI_ResearchScene : UI_Base
{
    enum Buttons
    {
        BackButton
    }

    enum Texts
    {
        GoldText,
        ZamText
    }

    enum Images
    {
        BackImage
    }

    enum UIObject
    {
        AttackUpgrade,
        DefenceUpgrade,
        RandomUpgrade
    }

    //public Sprite Sprite;


    Sequence sequence;

    private void Awake()
    {
        Init();


        //test

        //for (int i = 0; i < Managers.Data.Datas[Enums.Sheet.MyUnit].Count; i++)
        //{
        //    MyUnit unit = new();
        //    unit.Init(i, Sprite);
        //    Managers.Player.Inventory.Add(unit);
        //}

        //for (int i = 0; i < Managers.Data.Datas[Enums.Sheet.Tower].Count; i++)
        //{
        //    Tower tower = new();
        //    tower.Init(i, Sprite);
        //    Managers.Player.Inventory.Add(tower);
        //}


    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));


        GetTextMeshProUGUI((int)Texts.GoldText).text = Managers.Player.GetGold().ToString();
        GetTextMeshProUGUI((int)Texts.ZamText).text = Managers.Player.GetZam().ToString();
        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBack);

    }

    private TextMeshProUGUI GetTextMeshProUGUI(int idx) { return Get<TextMeshProUGUI>(idx); }

    private void OnEnable()
    {
        if (Managers.Player != null)
        {
            Managers.Player.OnGoldChanged += UpdateGoldUI;
            UpdateGoldUI(Managers.Player.GetGold());
        }
    }

    private void OnDisable()
    {
        if (Managers.Player != null)
        {
            Managers.Player.OnGoldChanged -= UpdateGoldUI;
        }

    }

    private void UpdateGoldUI(long gold)
    {
        GetTextMeshProUGUI((int)Texts.GoldText).text = Util.FormatNumber(Managers.Player.GetGold());
        GetTextMeshProUGUI((int)Texts.ZamText).text = Util.FormatNumber(Managers.Player.GetZam());
    }

    public void OnClickBack(PointerEventData data)
    {
        Util.OnClickButtonAnim(this.gameObject, GetImage((int)Images.BackImage), false);
        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick, this.transform.position);


    }


}
