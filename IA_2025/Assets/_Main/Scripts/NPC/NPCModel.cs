using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCModel : PlayerModel
{
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private List<Transform> wayPointsList;
    public bool _isFinishPath;
    
    private int _modifier = 1;
    private ObstacleAvoidance _obs;
    private ILook _look;
    
    public int WaypointIndex { get; private set; } = 0;

    protected override void Awake()
    {
        _obs = GetComponent<ObstacleAvoidance>();
        _look = GetComponent<ILook>();
        base.Awake();
    }
    
    public float AttackRange => attackRange;
    public override void Attack()
    {
        var colls = Physics.OverlapSphere(Position, attackRange, enemyMask);
        for (int i = 0; i < colls.Length; i++)
        {
            GameObject.Destroy(colls[i].gameObject);
        }
        base.Attack();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Position,attackRange);
    }

    public void ChangeWaypointIndex()
    {
        if (WaypointIndex+_modifier >= wayPointsList.Count)
        {
            _modifier = -1;
        }
        else if (WaypointIndex + _modifier < 0)
        {
            _modifier = 1;       
        }
        WaypointIndex += _modifier;
    }

    public Transform GetWaypoint()
    {
        return wayPointsList[WaypointIndex];
    }

    public override void Move(Vector3 dir)
    {
        var obsDir = _obs.GetDir(dir);
        _look.LookDir(obsDir);
        base.Move(obsDir);
    }
}
