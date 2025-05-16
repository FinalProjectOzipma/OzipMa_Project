using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    private EntityController ctrl;
    private Collider2D atkCol;
    [SerializeField] private LayerMask hitLayer;

    private void Awake()
    {
        ctrl = GetComponent<EntityController>();
        atkCol = GetComponent<Collider2D>();
    }
    public void SetRotation(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer & hitLayer) != 0)
        {
            IDamagable dmg = collision.GetComponentInParent<IDamagable>();
            if (dmg != null) dmg.ApplyDamage(ctrl.Status.Attack.GetValue());
        }
    }
}
