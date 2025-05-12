using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class LongPressPopupTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public float holdTime = 0.5f;

    private bool isPointerDown = false;
    private float pressTime = 0f;
    private UI_InfoPopup objectInfo;
    private Slot slot;


    void Update()
    {
        if (isPointerDown)
        {
            pressTime += Time.deltaTime;
            if (pressTime >= holdTime)
            {
                slot = GetComponent<Slot>();

                Managers.UI.ShowPopupUI<UI_InfoPopup>("InfoUI");
                objectInfo = Managers.UI.GetPopup<UI_InfoPopup>();

                if (slot.Gettable is MyUnit myUnit)
                {
                    objectInfo.SelectedInfo(myUnit);
                }
                else if (slot.Gettable is Tower tower)
                {
                    objectInfo.SelectedInfo(tower);
                }

                Reset();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        pressTime = 0f;
    }

    public void OnPointerUp(PointerEventData eventData) => Reset();
    public void OnPointerExit(PointerEventData eventData) => Reset();

    private void Reset()
    {
        isPointerDown = false;
        pressTime = 0f;
    }
}
