using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform player;
    LineOfSightMono lineOfSight;
    FSM<EnemyStatesEnum> fsm;
    ITreeNode _root;
    Animator animator;
    IMove _move;
    IAttack _attack;
    public List<Transform> waypoints;
    public int currentWaypointIndex;
    public float moveSpeed;
    public int waypointThreshold = 1;
    
    void Start()
    {
        lineOfSight = GetComponent<LineOfSightMono>();
        animator = GetComponent<Animator>();
        _move = GetComponent<IMove>();
        _attack = GetComponent<IAttack>();

        StartFSM();
        InitializeTree();
    }

    void StartFSM()
    {
        var idle = new EnemyStateIdle();
        var patrol = new EnemyStatePatrol();
        var chase = new EnemyStateChase();
        var attack = new EnemyStateAttack();

        idle.AddTransition(EnemyStatesEnum.Patrol, patrol);
        patrol.AddTransition(EnemyStatesEnum.Chase, chase);
        patrol.AddTransition(EnemyStatesEnum.Idle, idle);
        chase.AddTransition(EnemyStatesEnum.Attack, attack);
        chase.AddTransition(EnemyStatesEnum.Idle, idle);
        attack.AddTransition(EnemyStatesEnum.Chase, chase);
        attack.AddTransition(EnemyStatesEnum.Idle, idle);

        idle.Initialize();
        patrol.Initialize(_move, transform, waypoints, moveSpeed, waypointThreshold);
        chase.Initialize(_move, transform, player, moveSpeed);
        attack.Initialize(_attack);

        fsm = new FSM<EnemyStatesEnum>(idle);
    }

    void InitializeTree()
    {
        var idle = new ActionNode(() => fsm.Transition(EnemyStatesEnum.Idle));
        var patrol = new ActionNode(() => fsm.Transition(EnemyStatesEnum.Patrol));
        var chase = new ActionNode(() => fsm.Transition(EnemyStatesEnum.Chase));
        var attack = new ActionNode(() => fsm.Transition(EnemyStatesEnum.Attack));

        var qInAttackRange = new QuestionNode(InAttackRange, attack, chase);
        var qSeePlayer = new QuestionNode(SeeTarget, qInAttackRange, patrol);
        var qPlayerExists = new QuestionNode(() => player != null, qSeePlayer, idle);

        _root = qPlayerExists;
    }

    public bool InAttackRange()
    {
        return Vector3.Distance(transform.position, player.position) <= _attack.GetAttackRange;
    }

    public bool SeeTarget()
    {
        return lineOfSight.LOS(player);
    }

    void Update()
    {
        fsm.OnExecute();
        _root.Execute();
    }

    void FixedUpdate()
    {
        fsm.OnFixExecute();
    }
}
