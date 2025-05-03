using System.Collections.Generic;
using UnityEngine;

public class NPCModel : PlayerModel
{
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private List<Transform> wayPointsList;
    
    private int _modifier = 1;
    
    public int WaypointIndex { get; private set; } = 0;

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
}
