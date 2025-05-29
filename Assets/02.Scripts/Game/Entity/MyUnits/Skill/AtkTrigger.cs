using UnityEngine;

public class AtkTrigger : Poolable
{
    private bool OnDestroy;

    [SerializeField] private CircleCollider2D trigger;

    #region ownerData
    private GameObject ownerObject;
    private float ownerAttack;
    private int ownerLayer;
    #endregion

    private int hitLayer;
    public void Init(GameObject ownerObject, float ownerAttack, GameObject target = null, bool isSingleAttack = true)
    {
        this.ownerObject = ownerObject;

        this.ownerAttack = ownerAttack;
        this.ownerLayer = 1 << ownerObject.layer;

        if (target == null)
        {
            Util.Log($"현재 target이 NULL값입니다. 게임오브젝트 정보 {gameObject.GetInstanceID()}");
            return;
        }

        hitLayer = (int)Enums.Layer.Enemy | (int)Enums.Layer.MyUnit | (int)Enums.Layer.Core;

        if (ownerLayer == (int)Enums.Layer.Enemy)
            hitLayer -= ownerLayer;
        else
            hitLayer -= ownerLayer | (int)Enums.Layer.Core;

        transform.position = target.transform.position;
    }

    private void Update()
    {
        if (OnDestroy)
            OnPoolDestroy();
    }

    public void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(trigger.transform.position, trigger.radius, hitLayer);

        foreach (var hit in colliders)
        {
            Util.Log(hit.name);
            IDamagable damagle = hit.GetComponent<IDamagable>();
            if (damagle != null)
            {
                damagle.ApplyDamage(ownerAttack);
            }
        }
    }

    public void AnimationTriggerCalled() => OnDestroy = true;

    protected void OnPoolDestroy()
    {
        if (gameObject.activeInHierarchy)
        {
            Managers.Resource.Destroy(gameObject);
            OnDestroy = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(trigger.transform.position, trigger.radius);
    }
}
