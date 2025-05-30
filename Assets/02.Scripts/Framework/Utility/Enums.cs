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
        Gacha,
        LoadingTip,
        InchentMultiplier,
        LevelUpValue,
        Research,
        AttackDefault,
        QuestDataList,
        Count
    }

    public enum Layer
    {
        Map = 1 << 6,
        Enemy = 1 << 8,
        MyUnit = 1 << 9,
        Core = 1 << 10,
        Tower = 1 << 11,
    }

    public enum WaveState
    {
        None,
        Start,
        Playing,
        Reward,
        End,
    }

    public enum TutorialStep
    {
        None,
        PlaceTower,
        EditTower,
        DeleteTower,
        Research,
        Gacha,
        End,

        Count,
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
    None,
    Physical,
    Psychic,
    Magic,
    Fire,
    Explosive,
    Dark,
    Buff,

    Count
}


[GoogleSheet.Core.Type.UGS(typeof(QuestType))]
public enum QuestType
{
    Daily,
    Achivement,
    Repeat,
    Count
}


[GoogleSheet.Core.Type.UGS(typeof(ConditionType))]
public enum ConditionType
{
    Connection,
    EnemyKill,
    BossKill,
    MyUnitCollect,
    TowerCollect,
    Reach,
    StageClear,
    WaveClear,
    MyUnitInchen,
    TowerInchen,
    Count
}

