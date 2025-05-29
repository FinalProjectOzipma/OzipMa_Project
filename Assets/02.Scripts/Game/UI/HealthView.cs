using UnityEngine;

public class HealthView : MonoBehaviour
{
    public GameObject FillRect;
    public EntityHealth health;

    private EntityController ctrl;

    private void Start()
    {
        ctrl = GetComponentInParent<EntityController>();
    }

    private void Update()
    {
        if (ctrl != null)
        {
            transform.localScale = new Vector3(ctrl.FacDir, 1, 1);
        }

    }

    /// <summary>
    /// 체력바 업데이트
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="maxAmount"></param>
    public void SetHpBar(float amount, float maxAmount)
    {
        FillRect.transform.localScale = new Vector3(amount / maxAmount, 1, 1);
    }

    private void OnDisable()
    {
        FillRect.transform.localScale = Vector3.one;
    }
}
