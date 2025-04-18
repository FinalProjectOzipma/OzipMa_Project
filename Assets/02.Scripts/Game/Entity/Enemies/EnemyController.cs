using DefaultTable1;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : EntityController
{
    public EnemyStatus EnemyStatus { get; private set; }

    public Rigidbody2D Rigid { get; private set; }

    public Enemy Enemy { get; private set; }
    public EnemyStatus Status { get; private set; }


    public Sprite SpriteImage;
    public NavMeshAgent Agent;
    public GameObject Target;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    public override void Init(int primaryKey, string name, Vector2 position, GameObject gameObject = null)
    {
        base.Init(primaryKey, name, position);
        //EnemyStatus = new EnemyStatus(Row);
        AnimData = new EnemyAnimationData();
        AnimData.Init(this);
    }

    private string _Body = nameof(_Body);
    public override void TakeRoot(int primaryKey, string name, Vector2 position)
    {
        Enemy = new Enemy(primaryKey, SpriteImage);
        Status = Enemy.Status;
        Managers.Resource.Instantiate($"{name}{_Body}", go =>
        {
            go.transform.SetParent(transform);
            Rigid = go.GetOrAddComponent<Rigidbody2D>();
            Init(primaryKey, name, position, go);
        });
    }
}
