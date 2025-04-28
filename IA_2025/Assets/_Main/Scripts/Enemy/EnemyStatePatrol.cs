using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePatrol : State<EnemyStatesEnum>
{
    IMove _move;
    Transform _self;
    List<Transform> _waypoints;
    float _speed;
    int _waypointThreshold;
    int _currentWaypointIndex;

    public override void Initialize(params object[] p)
    {
        _move = (IMove)p[0];
        _self = (Transform)p[1];
        _waypoints = (List<Transform>)p[2];
        _speed = (float)p[3];
        _waypointThreshold = (int)p[4];
    }

    public override void Enter()
    {
        Debug.Log("Entering Patrol State");
    }

    public override void Execute()
    {
        if (_waypoints.Count == 0) return;

        Vector3 targetPos = _waypoints[_currentWaypointIndex].position;
        Vector3 dir = (targetPos - _self.position).normalized;
        _move.Move(dir);

        if (Vector3.Distance(_self.position, targetPos) <= _waypointThreshold)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Count;
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Patrol State");
    }
}
