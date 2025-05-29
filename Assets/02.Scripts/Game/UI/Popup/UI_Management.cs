using DG.Tweening;

public class UI_Management : UI_Scene
{
    bool isOpened;
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        //Bind

        //Get

        //BindEvent
    }

    public void SlideToggle()
    {
        if (isOpened)
        {
            transform.DOLocalMoveY(transform.localPosition.y - 500f, 0.5f).SetEase(Ease.OutCubic);
        }
        else
        {
            transform.DOLocalMoveY(transform.localPosition.y + 500f, 0.5f).SetEase(Ease.OutCubic);
        }
        isOpened = !isOpened;
    }
}
