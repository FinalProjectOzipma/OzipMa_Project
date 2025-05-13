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

    private DefaultTable.AbilityDefaultValue explo; // 도트뎀 필요한 멤버변수
    public KnightBuff(EntityController ctrl)
    {
        this.ctrl = ctrl as EnemyController;

        DefaultTable.AbilityDefaultValue explo = 
            Util.TableConverter<DefaultTable.AbilityDefaultValue>(Managers.Data.Datas[Sheet.AbilityDefaultValue])[(int)AbilityType.Explosive];
    }

    public void Execute(float attackerDamage, AbilityDefaultValue values)
    {
        if (condiHandler == null)
            condiHandler = ctrl.ConditionHandlers[(int)AbilityType.Buff];

        StartBuff().Forget();
    }

    // 버프 할당
    async UniTaskVoid StartBuff()
    {
        // 버프 시간안에 반복
        int layerMask = (int)Enums.Layer.Core | (int)Enums.Layer.MyUnit;
        var cir = condiHandler.GameObj.GetComponent<CircleCollider2D>();
        Collider2D[] cols = Physics2D.OverlapCircleAll(cir.transform.position, cir.radius, layerMask);

        condiHandler.CurDuration = condiHandler.Duration;
        while (condiHandler.CurDuration >= 0f)
        {
            
            // 어차피 나이트 Idle에서 시간체크하고
            // BuffObject는 Buff 클립이 실행이 될때 나오는 이펙트니깐 상관없고
            // animationTrigger view Object를 그때 실행시켜주면 됨
            
            foreach(var col in cols)
            {
                if (col == null)
                    continue;

                if (col.TryGetComponent<IDamagable>(out var dmg))
                    dmg.ApplyDamage(0, AbilityType.Explosive, ctrl.gameObject, explo);
            }

            await UniTask.NextFrame();
            condiHandler.CurDuration -= Time.deltaTime;
        }

        condiHandler.ObjectActive(false);
    }
}
