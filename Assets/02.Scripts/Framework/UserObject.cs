using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserObject
{
    public string Name;
    public string Description;
    public Enums.RankType RankType;

    public Sprite Sprite { get; set; } // UI에 표시될 오브젝트 이미지
    public IntegerBase Level { get; set; } = new IntegerBase();
    public IntegerBase Stack { get; set; } = new IntegerBase();
    public IntegerBase MaxStack { get; set; } = new IntegerBase();
    public IntegerBase MaxLevel { get; set; } = new IntegerBase();
    public IntegerBase Grade { get; set; } = new IntegerBase();
    public IntegerBase MaxGrade { get; private set; } = new IntegerBase();

    public virtual void Init(int maxStack, Sprite sprite)
    {
        Sprite = sprite;

        // 매개변수에 유닛의 키를 들고와서 저장된게 있는지 확인 체크 여기서 처리
        // TODO::
        Level.SetValue(1);
        Grade.SetValue(0);
        Stack.SetValue(0);
        MaxStack.SetValue(maxStack);


        MaxLevel.SetValue(20);
        MaxGrade.SetValue(5);
    }
}
