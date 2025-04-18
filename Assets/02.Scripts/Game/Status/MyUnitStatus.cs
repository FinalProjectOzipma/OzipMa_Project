using System.Collections.Generic;

public class MyUnitStatus : StatusBase
{
    public EntityHealth Health = new();
    public float MaxHealth;

    public List<FloatBase> Defences = new();
    public FloatBase MoveSpeed = new();

    public MyUnitStatus(int PrimaryKey, List<DefaultTable.MyUnit> Row)
    {
        var result = Row[PrimaryKey];

        Health.SetValue(result.Health);
        MaxHealth = Health.GetValue();

        Attack.SetValue(result.Attack);
        for(int i = 0; i < result.Defence.Count; i++)
        {
            Defences[i] = new FloatBase();
            Defences[i].SetValue(result.Defence[i]);
        }

        MoveSpeed.SetValue(result.MoveSpeed);

        AttackCoolDown.SetValue(result.AttackCoolDown);
        AttackRange.SetValue(result.AttackRange);
    }
}
