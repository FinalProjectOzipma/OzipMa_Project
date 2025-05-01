using DefaultTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCondition<T> : IConditionable where T : EntityController
{
    private Coroutine dotCor;
    private EnemyController enemyCtrl;
    private MyUnitController myunitCtrl;

    private float attackerDamage;
    private float victimDefence;
    private int a = 0;

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
        if (typeof(T) == typeof(EnemyController))
            victimDefence = enemyCtrl.Status.Defence.GetValue();
        else
            victimDefence = myunitCtrl.MyUnitStatus.Defence.GetValue();

        ApplyDotDamage(values.AbilityValue, values.AbilityDuration, values.AbilityCooldown);
    }

    public void Init()
    {
        
    }

    private void ApplyDotDamage(float abilityValue, float abilityDuration, float abilityCooldown)
    {
        if (dotCor != null)
        {
            enemyCtrl.StopCoroutine(dotCor);
            dotCor = null;
        }

        dotCor = StartCoroutine(OnDotDamage(abilityValue, abilityDuration, abilityCooldown));
    }

    private IEnumerator OnDotDamage(float abilityValue, float abilityDuration, float abilityCooldown)
    {
        bool canHit = true;
        float coolDown = 0.0f;
        ++a;
        while (abilityDuration > 0)
        {
            abilityDuration -= Time.deltaTime;

            if (canHit)
            {
                AddHealth(-abilityValue);
                coolDown = abilityCooldown;
                canHit = false;
            }

            if (coolDown <= 0.0f)
            {
                canHit = true;
            }

            coolDown -= Time.deltaTime;
            yield return null;
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
