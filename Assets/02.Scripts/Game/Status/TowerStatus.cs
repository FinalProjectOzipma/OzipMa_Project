using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Enums;

public class TowerStatus : StatusBase
{
    public TowerStatus(int id)
    {
        Init();
        // TODO :: 타워 속성 받기

        //name = ;
        //description = ;
        //rankType = ;

        //attack.SetStat();
        //TowerattackType =
        //attackCooldown.SetStat();
        //attackRange.SetStat();
        //level =

        // TODO :: 진짜 데이터테이블 생기면 속성테이블 받아두기
        //Abilities = Util.TableConverter<데이터테이블.TowerAbilityDefaultValue>(Managers.Data.Datas["TowerAbilityDefaultValue"]);
    }
}
