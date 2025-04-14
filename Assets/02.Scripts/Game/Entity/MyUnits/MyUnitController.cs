using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnitController : EntityController
{
    public MyUnit MyUnit { get; private set; }
    public MyUnitStatus MyUnitStatus { get; private set; }

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        AnimData = new MyUnitAnimationData();
        AnimData.Init(this);
    }


    //public override void Init(MyUnit myUnit)
    //{
    //    TakeRoot(myUnit);
    //    base.Init();
    //    AnimData = new MyUnitAnimationData();
    //    AnimData.Init(this);
    //}

    //Root부분 생성해주는 파트
    public void TakeRoot(MyUnit myUnit)
    {
        Managers.Resource.Instantiate(nameof(myUnit), go =>
        {
            MyUnit = myUnit;
            MyUnitStatus = MyUnit.Status;
            go.transform.SetParent(transform);
            Debug.Log("자식 붙여드렸습니다");
        });
    }
}