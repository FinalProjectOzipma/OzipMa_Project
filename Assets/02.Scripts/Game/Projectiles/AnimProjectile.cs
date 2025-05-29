using UnityEngine;

public class AnimProjectile : EntityProjectile
{
    private Animator anim;
    private string destroyString = "Destroy";
    private int destroyHash;

    private bool triggerCalled;

    private bool OnDestroy;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        destroyHash = Animator.StringToHash(destroyString);
    }

    public override void Init(GameObject owner, float ownerAttack, Vector2 targetPos)
    {
        base.Init(owner, ownerAttack, targetPos);
        OnDestroy = false;
    }

    private void Update()
    {
        if (!triggerCalled)
            return;

        if (gameObject.activeInHierarchy)
        {
            triggerCalled = false;
            anim.SetBool(destroyHash, false);
            Managers.Resource.Destroy(gameObject);
        }
    }

    protected override void OnPoolDestroy(int hitLayer)
    {
        if (hitLayer != (int)Enums.Layer.Map)
            return;

        anim.SetBool(destroyHash, true);
        spTrail.SetActive(false);
        OnDestroy = true;
    }

    protected override void Move()
    {
        if (!OnDestroy)
            base.Move();
    }

    public void AnimationTriggerCalled() => triggerCalled = true;
}
