using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : EntityController
{
    public Tower Tower {  get; private set; }
    public TowerStatus TowerStatus { get; private set; }

    private GameObject body;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
    }

    /// <summary>
    /// Tower 정보 넣어주는 함수
    /// </summary>
    /// <param name="Info">Tower 데이터</param>
    public override void TakeRoot(UserObject Info)
    {
        Tower = Info as Tower;
        TowerStatus = Tower.Status;

        Managers.Resource.Instantiate(Tower.Name, go => {
            body = go;
            body.transform.SetParent(transform);

            // TODO
            // 1. go에서 Animator, AnimData 받아오기
            // 2. AnimData.StateMachine.
        });
    }
}
