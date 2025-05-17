using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : EntityController, IDamagable
{
    private Coroutine SlowCor;

    public Rigidbody2D Rigid { get; private set; }
    public SpriteRenderer Spr { get; private set; }
    public SpriteTrail SpTrail { get; private set; }


    public Enemy Enemy { get; private set; }
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

    public override void Init(Vector2 position)
    {
        base.Init(position);
        transform.position = position;
        Targets.Clear();
        Targets.Push(Managers.Wave.MainCore.gameObject);

        Body.GetComponent<EntityBodyBase>().Enable(); // 죽었을때 Disable 처리해줘야됨
    }

    private string _Body = nameof(_Body);
    public override void TakeRoot(int primaryKey, string name, Vector2 position)
    {
        entityName = name;

        if (Enemy == null)
            Enemy = new Enemy(primaryKey, SpriteImage);
        else
            Enemy.Init(primaryKey, SpriteImage);

        Enemy.Status.InitHealth();
        Status = Enemy.Status;

        if (Body == null)
        {
            Managers.Resource.Instantiate($"{name}{_Body}", go =>
            {
                go.transform.SetParent(transform);
                Fx = go.GetOrAddComponent<ObjectFlash>();
                Spr = go.GetOrAddComponent<SpriteRenderer>();
                SpTrail = go.GetComponentInChildren<SpriteTrail>();
                Body = go;
                Init(position);
            });
        }
        else
        {
            Init(position);
        }
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

    public virtual void AnimationFinishTrigger() => AnimData.StateMachine.CurrentState.AnimationFinishTrigger();
    public virtual void AnimationFinishProjectileTrigger() => AnimData.StateMachine.CurrentState.AnimationFinishProjectileTrigger();

    /// <summary>
    /// go = 데미지 준 애(브레인부분)
    /// damage = 얼마나 줄지
    /// </summary>
    /// <param name="go"></param>
    /// <param name="damage"></param>
    public void ApplyDamage(float incomingDamage, AbilityType condition = AbilityType.None, GameObject go = null, DefaultTable.AbilityDefaultValue values = null)
    {
        //반사타입 처리
        if (go != null && go.TryGetComponent<MyUnitController>(out MyUnitController myunit))
        {
            if (Enemy.AtkType == AtkType.ReflectDamage)
            {
                //float abilityRatio = Status.AbilityValue;
                float abilityRatio = 0.5f; // TODO: Test용 나중에 지워야함
                myunit.ReflectDamage(incomingDamage, abilityRatio);
                Util.Log("반사해드렸습니다");
                Managers.Audio.PlaySFX(SFXClipName.Reflect);
            }
            else if (myunit.MyUnit.AbilityType == AbilityType.Psychic)
            {
                Status.Attack.SetValueMultiples(0.7f);
            }
        }

        float defence = Mathf.Max(0f, Status.Defence.GetValue());

        // 부드러운 비율 스케일링
        float damageScale = incomingDamage / (incomingDamage + defence);

        float finalDamage = incomingDamage * damageScale;

        finalDamage = Mathf.Max(finalDamage, 1f); // 최소 1 보장 (선택사항)



        Status.AddHealth(-finalDamage, gameObject);
        Fx.StartBlinkFlash();
        ApplyCondition(condition, incomingDamage, go, values);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
