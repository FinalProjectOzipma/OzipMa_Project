using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{ }

[GoogleSheet.Core.Type.UGS(typeof(RankType))]
public enum RankType
{
    일반,
    고급,
    신화,
    Myth,

    Count
}


[GoogleSheet.Core.Type.UGS(typeof(AtkType))]
public enum AtkType
{
    마법,
    물리,
    번개,

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
    공격형,
    방어형,
    화염형,
    화염,
    Dot,
    Slow,
    KnockBack,
    BonusCoin,

    Count
}
