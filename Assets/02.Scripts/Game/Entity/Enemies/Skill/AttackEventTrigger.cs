using UnityEngine;

public class AttackEventTrigger : Poolable
{
    private bool OnDestroy;

    [SerializeField] private BoxCollider2D trigger;

    #region ownerData
    private GameObject ownerObject;
    private float ownerAttack;
    private int ownerLayer;
    #endregion

    private GameObject target = null;
    private bool isSingleAttack;

    private int hitLayer;


    public void Init(GameObject ownerObject, float ownerAttack, GameObject target, bool isSingleAttack = true)
    {
        this.ownerObject = ownerObject;

        this.ownerAttack = ownerAttack;
        this.ownerLayer = 1 << ownerObject.layer;

        this.isSingleAttack = isSingleAttack;
        this.target = target;

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

        Collider2D[] colliders = Physics2D.OverlapBoxAll(trigger.transform.position, trigger.bounds.size, 0f, hitLayer);

        foreach (var hit in colliders)
        {
            if (isSingleAttack && target != hit.gameObject) continue;

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
}
