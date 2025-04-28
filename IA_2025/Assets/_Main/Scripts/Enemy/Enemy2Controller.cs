using System.Collections.Generic;
using UnityEngine;

public class Enemy2Controller : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float fleeDistance = 5f;
    [SerializeField] private float safeDistance = 10f;

    private LineOfSightMono lineOfSight;
    private FSM<EnemyStatesEnum> fsm;
    private ITreeNode _root;
    private IMove _move;

    public List<Transform> waypoints;
    public float moveSpeed;
    public int waypointThreshold = 1;

    private bool isFleeing = false; 

    void Start()
    {
        lineOfSight = GetComponent<LineOfSightMono>();
        _move = GetComponent<IMove>();

        StartFSM();
        InitializeTree();
    }

    void StartFSM()
    {
        var patrol = new EnemyStatePatrol();
        var flee = new EnemyStateFlee();

        patrol.AddTransition(EnemyStatesEnum.Flee, flee);
        flee.AddTransition(EnemyStatesEnum.Patrol, patrol);

        patrol.Initialize(_move, transform, waypoints, moveSpeed, waypointThreshold);
        flee.Initialize(_move, transform, player, moveSpeed, safeDistance);

        fsm = new FSM<EnemyStatesEnum>(patrol);
    }

    void InitializeTree()
    {
        var patrol = new ActionNode(() => 
        {
            if (!isFleeing) 
                fsm.Transition(EnemyStatesEnum.Patrol);
        });

        var flee = new ActionNode(() => 
        {
            isFleeing = true; 
            fsm.Transition(EnemyStatesEnum.Flee);
        });

        var qIsSafeDistance = new QuestionNode(IsSafeDistance, patrol, flee);
        var qIsPlayerClose = new QuestionNode(IsPlayerClose, flee, qIsSafeDistance);
        var qSeePlayer = new QuestionNode(SeeTarget, qIsPlayerClose, patrol);
        var qPlayerExists = new QuestionNode(() => player != null, qSeePlayer, patrol);

        _root = qPlayerExists;
    }

    bool SeeTarget()
    {
        return lineOfSight.LOS(player);
    }

    bool IsPlayerClose()
    {
        if (player == null) return false;
        return Vector3.Distance(transform.position, player.position) <= fleeDistance;
    }

    bool IsSafeDistance()
    {
        if (player == null) return true;

        bool safe = Vector3.Distance(transform.position, player.position) >= safeDistance;
        
        if (safe && isFleeing)
        {
            isFleeing = false; 
        }

        return safe;
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
