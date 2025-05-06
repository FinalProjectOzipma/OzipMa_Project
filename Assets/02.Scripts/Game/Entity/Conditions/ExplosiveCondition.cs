using Cysharp.Threading.Tasks;
using DefaultTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCondition<T> : UniTaskHandler, IConditionable where T : EntityController
{
    private Coroutine dotCor;
    private EnemyController enemyCtrl;
    private MyUnitController myunitCtrl;

    private float attackerDamage;
    private float victimDefence;

    private bool canHit = true;
    private float coolDown = 0.0f;
    private float time = 0.0f;

    public ExplosiveCondition(T ctrl)
    {
        if (typeof(T) == typeof(EnemyController))
            this.enemyCtrl = ctrl as EnemyController;
        else
            this.myunitCtrl = ctrl as MyUnitController;
    }

    /// <summary>
    /// 나중에 저장을 추가하면, 참조 값이라 값타입이나 타워에서 디폴트 참조타입이 아닌 변하는 타입을 가져와야됨
    /// </summary>
    /// <param name="attackerDamage"></param>
    /// <param name="values"></param>
    public void Execute(float attackerDamage, AbilityDefaultValue values) 
    {
        TokenDisable();
        TokenEnable();

        time = values.AbilityDuration;
        coolDown = 0f;

        if (typeof(T) == typeof(EnemyController))
            victimDefence = enemyCtrl.Status.Defence.GetValue();
        else
            victimDefence = myunitCtrl.MyUnitStatus.Defence.GetValue();

        OnDotDamage(values.AbilityValue, values.AbilityDuration, values.AbilityCooldown).Forget();
    }

    public void Init()
    {
        Init();
    }


    async UniTaskVoid OnDotDamage(float abilityValue, float abilityDuration, float abilityCooldown)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;

            if (canHit)
            {
                AddHealth(-abilityValue);
                // 0초라서 수정해야됨 최소한 0.2초가 적당한듯?
                coolDown = 0.2f;
                canHit = false;
            }
            else if (coolDown <= 0.0f)
            {
                canHit = true;
            }

            coolDown -= Time.deltaTime;
            await UniTask.NextFrame(disableCancellation.Token);
        }

    }

    private void AddHealth(float damage)
    {
        if (typeof(T) == typeof(EnemyController))
        {
            enemyCtrl.Status.AddHealth(damage, enemyCtrl.ConditionHandlers[(int)AbilityType.Explosive].Attacker.gameObject);
            enemyCtrl.Fx.StartBlinkRed();
        }
/*        else
            myunitCtrl.MyUnitStatus.AddHealth(damage, enemyCtrl.ConditionHandlers[(int)AbilityType.Explosive].Attacker.gameObject);*/
    }

    // 나중에 공통적인건 묶어줄 필요가 있음
    // 공통적으로 묶어주면 기본 베이스에만 접근해두 되기때문에 if체크할 필요가 없어진다
    private Coroutine StartCoroutine(IEnumerator coroutine)
    {
        if (typeof(T) == typeof(EnemyController))
        {
            return enemyCtrl.StartCoroutine(coroutine);
        }

        return myunitCtrl.StartCoroutine(coroutine);
    }
}
