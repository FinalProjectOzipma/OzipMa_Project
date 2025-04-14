using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
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
}

[GoogleSheet.Core.Type.UGS(typeof(TowerAtkType))]
public enum TowerAtkType
{
    Projectile,
    Floor,
    Range,

    Count
}

[GoogleSheet.Core.Type.UGS(typeof(TowerType))]
public enum TowerType
{
    Dot,
    Slow,
    KnockBack,
    BonusCoin,

    Count
}
