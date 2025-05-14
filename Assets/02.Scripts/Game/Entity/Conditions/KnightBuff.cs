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
        var stage = Util.TableConverter<DefaultTable.Stage>(Managers.Data.Datas[Enums.Sheet.Stage]);
        int index = Mathf.Min(Managers.Player.CurrentStage, stage.Count - 1);

        condiHandler.CurDuration = condiHandler.Duration;

        while (condiHandler.CurDuration >= 0f)
        {
            
            // 어차피 나이트 Idle에서 시간체크하고
            // BuffObject는 Buff 클립이 실행이 될때 나오는 이펙트니깐 상관없고
            // animationTrigger view Object를 그때 실행시켜주면 됨
            
            if(hitTime <= 0.0f)
            {
                ctrl.Body.GetComponent<KnightBody>().OnSunFireCapeAttack(-10f * stage[index].ModifierRatio);

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
