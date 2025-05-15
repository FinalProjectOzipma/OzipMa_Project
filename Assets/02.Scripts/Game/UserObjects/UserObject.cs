using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserObject
{
    public int PrimaryKey;
    public string Name;
    public string Description;
    public RankType RankType;

    public Sprite Sprite { get; set; } // UI에 표시될 오브젝트 이미지
    public StatusBase Status { get; set; }
    public IntegerBase MaxGrade { get; private set; } = new IntegerBase();


    public virtual void Init(int primary, Sprite sprite)
    {
        PrimaryKey = primary;
        Sprite = sprite;
        MaxGrade.SetValue(5);
    }

    public T GetUpCasting<T>() where T : StatusBase => Status as T;

    /// <summary>
    /// 합성기능
    /// </summary>
    public void UpGrade()
    {
        while (true)
        {
            //스택이 맥스스택보다 낮다? 그레이드가 최대 그래이드다???
            if (Status.Stack.Value < Status.MaxStack.Value 
                || Status.Grade.Value == MaxGrade.Value)
                break;

            //스택 = 스택 - 맥스스택
            Status.Stack.Value -= Status.MaxStack.Value;

            //그레이드 올리기
            Status.Grade.AddValue(1);
            
            //맥스스텍값 5올리기
            Status.MaxStack.AddValue(5);

            ApplyUpgrade();
        }
    }

    /// <summary>
    /// 합성시 증가 스텟 적용하기
    /// </summary>
    private void ApplyUpgrade()
    {
        var result = Util.TableConverter<DefaultTable.InchentMultiplier>(Managers.Data.Datas[Enums.Sheet.InchentMultiplier]);
        float multiplier = result[Status.Grade.Value -1].Multiplier;
        Status.MaxLevel.AddValue(10);

        Status.Attack.AddMultiples(multiplier);
        Status.Defence.AddMultiples(multiplier);
        Status.Health.MaxValue += Status.Health.MaxValue * multiplier;
    }
}
