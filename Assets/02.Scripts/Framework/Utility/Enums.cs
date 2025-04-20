using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
    public enum Sheet
    {
        Stage,
        Wave,
        MyUnit,
        Enemy,
        Tower,
        TowerAbilityDefaultValue,

        Count
    }

}

[GoogleSheet.Core.Type.UGS(typeof(RankType))]
public enum RankType
{
    일반,
    고급,
    신화,

    Normal,
    Rare,
    Myth,

    Count
}


[GoogleSheet.Core.Type.UGS(typeof(AtkType))]
public enum AtkType
{
    마법,
    물리,
    번개,

    Magical,
    Physical,
    Lightning,

    Count
}

[GoogleSheet.Core.Type.UGS(typeof(TowerAtkType))]
public enum TowerAtkType
{
    물리,

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
