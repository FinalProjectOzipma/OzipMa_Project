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
        AbilityDefaultValue,

        Count
    }

}

[GoogleSheet.Core.Type.UGS(typeof(RankType))]
public enum RankType
{
    Normal,
    Rare,
    Epic,
    Legend,
    Myth,
    Ancient,

    Count
}


[GoogleSheet.Core.Type.UGS(typeof(AtkType))]
public enum AtkType
{
    DirectHit,
    Projectile,
    VampiricAttack,
    ReflectDamage,
    Area,
    StatusInfliction,

    Count
}

[GoogleSheet.Core.Type.UGS(typeof(AbilityType))]
public enum AbilityType
{
    Physical,
    Psychic,
    Magic,
    Fire,
    Explosive,
    Dark,

    Count
}
