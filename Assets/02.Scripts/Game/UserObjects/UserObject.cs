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
}
