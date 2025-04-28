using UnityEngine;

public class EnemyStateChase : State<EnemyStatesEnum>
{
    IMove _move;
    Transform _self;
    Transform _player;
    float _speed;

    public override void Initialize(params object[] p)
    {
        _move = (IMove)p[0];
        _self = (Transform)p[1];
        _player = (Transform)p[2];
        _speed = (float)p[3];
    }

    public override void Enter()
    {
        Debug.Log("Entering Chase State");
    }

    public override void Execute()
    {
        if (_player == null) return;

        Vector3 dir = (_player.position - _self.position).normalized;
        _move.Move(dir);
    }

    public override void Exit()
    {
        Debug.Log("Exiting Chase State");
    }
}
