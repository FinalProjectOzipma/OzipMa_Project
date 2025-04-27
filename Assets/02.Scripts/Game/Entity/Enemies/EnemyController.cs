using DefaultTable1;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : EntityController, IDamagable
{
    private Coroutine DotCor;
    private Coroutine SlowCor;

    public Rigidbody2D Rigid { get; private set; }
    public SpriteRenderer Spr { get; private set; }

    public Enemy Enemy { get; private set; }
    public EnemyStatus Status { get; private set; }
    public Stack<GameObject> Targets { get; set; } = new();


    public Sprite SpriteImage;
    public NavMeshAgent Agent;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Rigid = GetComponent<Rigidbody2D>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    protected override void Update()
    {
        if (AnimData != null)
            AnimData.StateMachine.CurrentState?.Update();
    }

    public override void Init(Vector2 position, GameObject gameObject = null)
    {
        base.Init(position);
        transform.position = position;
        Targets.Clear();
        Targets.Push(Managers.Wave.MainCore.gameObject);
    }

    private string _Body = nameof(_Body);
    public override void TakeRoot(int primaryKey, string name, Vector2 position)
    {
        Enemy = new Enemy(primaryKey, SpriteImage);
        Status = Enemy.Status;
        IsDead = false;
        Managers.Resource.Instantiate($"{name}{_Body}", go =>
        {
            go.transform.SetParent(transform);
            Fx = go.GetOrAddComponent<ObjectFlash>();
            Spr = go.GetOrAddComponent<SpriteRenderer>();
            Init(position);
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
        Rigid.AddForce(-dir * abilityValue, ForceMode2D.Impulse);
    }

    public void ApplyBonusCoin(float abilityValue)
    {
        Enemy.Reward += (int)abilityValue;
    }

    private IEnumerator OnDotDamage(float abilityValue, float abilityDuration, float abilityCooldown)
    {
        bool canHit = true;
        float coolDown = 0.0f;
        while (abilityDuration > 0)
        {
            abilityDuration -= Time.deltaTime;

            if(canHit)
            {
                float minus = Status.Defence.GetValue() - abilityValue;
                if(minus < 0.0f)
                {
                    Status.AddHealth(minus);
                    Fx.StartBlinkRed();
                }

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

    public virtual void AnimationFinishTrigger() => AnimData.StateMachine.CurrentState.AniamtionFinishTrigger();
    public virtual void AnimationFinishProjectileTrigger() => AnimData.StateMachine.CurrentState.AnimationFinishProjectileTrigger();

    /// <summary>
    /// go = 데미지 준 애(브레인부분)
    /// damage = 얼마나 줄지
    /// </summary>
    /// <param name="go"></param>
    /// <param name="damage"></param>
    public void ApplyDamage(float damage, AbilityType condition = AbilityType.None, GameObject go = null)
    {
        Util.Log(Status.AtkType.ToString());

        //반사타입 처리
        if (Status.AtkType == AtkType.ReflectDamage)
        {
            if (!go.TryGetComponent<MyUnitController>(out MyUnitController myunit))
            {
                Util.Log("마이유닛 아니네요?");
                return;
            }
            //float abilityRatio = Status.AbilityValue;
             float abilityRatio = 0.5f; // TODO: Test용 나중에 지워야함
            Util.Log("데미지 돌려드렸습니다");
            myunit.ReflectDamage(damage, abilityRatio);
            return;
        }

        //float minus = Status.Defences[0].GetValue() - attackPower;
        float minus = Status.Defence.GetValue() - damage;

        if (minus < 0.0f)
        {
            Status.AddHealth(minus);
            Fx.StartBlinkFlash();
        }

        int iCondition = (int)condition;
        if (Times.ContainsKey(iCondition) && Times[iCondition] < 0f)
        {
            CurrentCondition = condition;
            Times[iCondition] = Conditions[iCondition].CoolDown;
        }
    }

    
}
