using Cysharp.Threading.Tasks;
using DefaultTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCondition<T> : UniTaskHandler, IConditionable where T : EntityController
{
    private EntityController ctrl;

    private bool canHit = true;
    private float coolDown = 0.0f;
    private float time = 0.0f;

    public ExplosiveCondition(EntityController ctrl)
    {
        this.ctrl = ctrl;
    }

    /// <summary>
    /// 나중에 저장을 추가하면, 참조 값이라 값타입이나 타워에서 디폴트 참조타입이 아닌 변하는 타입을 가져와야됨
    /// </summary>
    /// <param name="attackerDamage"></param>
    /// <param name="values"></param>
    public void Execute(float attackerDamage, AbilityDefaultValue values) 
    {
        canHit = true;
        time = values.AbilityDuration;
        coolDown = 0f;

        TokenDisable();
        TokenEnable();
        OnDotDamage(values.AbilityValue, values.AbilityDuration, values.AbilityCooldown).Forget();
    }


    async UniTaskVoid OnDotDamage(float abilityValue, float abilityDuration, float abilityCooldown)
    {
        try
        {
            while (time > 0)
            {
                if (disableCancellation == null || disableCancellation.IsCancellationRequested || !ctrl.gameObject.activeInHierarchy)
                {
                    TokenDisable();
                    break;
                }

                if (canHit)
                {
                    AddHealth(-abilityValue);
                    // 0초라서 수정해야됨 최소한 0.2초가 적당한듯?
                    coolDown = 0.2f;
                    canHit = false;
                }
                else if (coolDown > 0.0f)
                {
                    coolDown -= Time.deltaTime;
                }
                else
                {
                    canHit = true;
                }

                time -= Time.deltaTime;
                await UniTask.NextFrame(disableCancellation.Token);
            }
            ctrl.Body.GetComponent<EntityBodyBase>().Spr.color = Color.white;
            ctrl.ConditionHandlers[(int)AbilityType.Explosive].IsExit = true;
        }
        catch(OperationCanceledException)
        {
            Util.Log("OnDamage 취소됨");
        }
        catch(ObjectDisposedException)
        {
            Util.Log("OnDamage: Token이 Dispose됨");
        }
        catch (Exception ex)
        {
            Util.Log($"OnDamage 에러 발생 : {ex}");
        }
    }

    private void AddHealth(float damage)
    {
        ctrl.Status.AddHealth(damage, ctrl.ConditionHandlers[(int)AbilityType.Explosive].Attacker.gameObject);
        ctrl.Fx.StartBlinkRed();
    }
}
