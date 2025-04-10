using GoogleSheet.Core.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
    [UGS(typeof(EAtkType))]
    public enum EAtkType
    {
        Cry,
        Jelly,
        Work,
        입니다,
    }

    public enum RankType
    {
        Normal,
        Rear,
        Legend,
        Myth,

        Count
    }

    public enum UnitAtkType
    {
        Near,
        Far,

        Count
    }

    public enum TowerAtkType
    {
        Projectile,
        Floor,
        Range,

        Count
    }

    public enum TowerType
    {
        Dot,
        Slow,
        KnockBack,
        BonusCoin,

        Count
    }

}
