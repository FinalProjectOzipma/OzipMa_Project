using DefaultTable1;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : EntityController
{

    private Coroutine DotCor;
    private Coroutine SlowCor;


    public EnemyStatus EnemyStatus { get; private set; }

    public Rigidbody2D Rigid { get; private set; }

    public Enemy Enemy { get; private set; }
    public EnemyStatus Status { get; private set; }


    public Sprite SpriteImage;
    public NavMeshAgent Agent;
    public GameObject Target;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    public override void Init(int primaryKey, string name, Vector2 position, GameObject gameObject = null)
    {
        base.Init(primaryKey, name, position);
        //EnemyStatus = new EnemyStatus(Row);
        AnimData = new EnemyAnimationData();
        AnimData.Init(this);
    }

    private string _Body = nameof(_Body);
    public override void TakeRoot(int primaryKey, string name, Vector2 position)
    {
        Enemy = new Enemy(primaryKey, SpriteImage);
        Status = Enemy.Status;
        Managers.Resource.Instantiate($"{name}{_Body}", go =>
        {
            go.transform.SetParent(transform);
            Rigid = go.GetOrAddComponent<Rigidbody2D>();
            Init(primaryKey, name, position, go);
        });
    }

    public void ApplyDotDamage(float abilityValue, float abilityDuration, float abilityCooldown)
    {
        if(DotCor != null)
        {
            StopCoroutine(DotCor);
            DotCor = null;
        }
             
        DotCor = StartCoroutine(OnDotDamage(abilityValue, abilityDuration, abilityCooldown));
    }

    public void ApplySlow(float abilityValue, float abilityDuration)
    {
        if (SlowCor != null)
        {
            StopCoroutine(SlowCor);
            SlowCor = null;
        }

        SlowCor = StartCoroutine(OnSlow(abilityValue, abilityDuration));
    }

    public void ApplyKnockBack(float abilityValue, Vector2 dir)
    {
        Rigid.AddForce(dir * abilityValue, ForceMode2D.Impulse);
    }

    public void ApplyBonusCoin(float abilityValue)
    {
        Enemy.Reward += (int)abilityValue;
    }

    private IEnumerator OnDotDamage(float abilityValue, float abilityDuration, float abilityCooldown)
    {
        bool canHit = true;
        float coolDown = 0.0f;
        while (abilityDuration < 0)
        {
            abilityDuration -= Time.deltaTime;

            if(canHit)
            {
                Status.AddHealth(-abilityValue);
                coolDown = abilityCooldown;
                canHit = false;
            }

            if(coolDown <= 0.0f)
            {
                canHit = true;
            }

            coolDown -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator OnSlow(float abilityValue, float abilityDuration)
    {
        while (abilityDuration < 0)
        {
            abilityDuration -= Time.deltaTime;

            Status.MoveSpeed.SetValueMultiples(abilityValue);

            yield return null;
        }

        Status.MoveSpeed.SetValueMultiples(1f);
    }
}
