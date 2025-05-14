using Cysharp.Threading.Tasks;
using DefaultTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class KnightBuff : IConditionable
{
    private ConditionHandler condiHandler;
    private EnemyController ctrl;
    private float duration;

    private float hitTime;
    private float coolDown = 0.4f;
    public KnightBuff(EntityController ctrl)
    {
        this.ctrl = ctrl as EnemyController;
    }

    public void Execute(float attackerDamage, AbilityDefaultValue values)
    {
        if (condiHandler == null)
            condiHandler = ctrl.ConditionHandlers[(int)AbilityType.Buff];

        hitTime = 0.0f;
        StartBuff().Forget();
    }

    // 버프 할당
    async UniTaskVoid StartBuff()
    {
        // 버프 시간안에 반복
        int layerMask = (int)Enums.Layer.Core | (int)Enums.Layer.MyUnit;
        var cir = condiHandler.GameObj.GetComponent<CircleCollider2D>();
        Collider2D[] cols = Physics2D.OverlapCircleAll(cir.transform.position, cir.radius / ctrl.transform.lossyScale.x, layerMask);

        condiHandler.CurDuration = condiHandler.Duration;

        while (condiHandler.CurDuration >= 0f)
        {
            
            // 어차피 나이트 Idle에서 시간체크하고
            // BuffObject는 Buff 클립이 실행이 될때 나오는 이펙트니깐 상관없고
            // animationTrigger view Object를 그때 실행시켜주면 됨
            
            if(hitTime <= 0.0f)
            {
                foreach (var col in cols)
                {
                    if (col == null)
                        continue;

                    if (col.TryGetComponent<IDamagable>(out var dmg))
                    {
                        dmg.ApplyDamage(40f);
                    }
                }

                hitTime = coolDown;
            }

            await UniTask.NextFrame();

            if(hitTime >= 0.0f)
            {
                hitTime -= Time.deltaTime;
            }

            condiHandler.CurDuration -= Time.deltaTime;
        }

        condiHandler.ObjectActive(false);
    }
}
