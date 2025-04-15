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

    //public override void TakeRoot(UserObject Info)
    //{
    //    Managers.Resource.Instantiate(nameof(Info), go =>
    //    {
    //        MyUnit = Info as MyUnit;
    //        MyUnitStatus = MyUnit.Status;
    //        go.transform.SetParent(transform);
    //    });
    //}
}