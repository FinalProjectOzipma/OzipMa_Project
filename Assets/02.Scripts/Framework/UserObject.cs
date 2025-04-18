using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserObject
{
    public string Name;
    public string Description;
    public RankType RankType;

    public Sprite Sprite { get; set; } // UI에 표시될 오브젝트 이미지
    public IntegerBase MaxGrade { get; private set; } = new IntegerBase();


    public virtual void Init(int primary, Sprite sprite)
    {
        Sprite = sprite;

        // 매개변수에 유닛의 키를 들고와서 저장된게 있는지 확인 체크 여기서 처리
        // TODO::
        MaxGrade.SetValue(5);
    }
}
