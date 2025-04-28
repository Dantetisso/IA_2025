using UnityEngine;

public class EnemyStateFlee : State<EnemyStatesEnum>
{
    private IMove _move;
    private Transform _selfTransform;
    private Transform _playerTransform;
    private float _moveSpeed;
    private float _safeDistance;

    public override void Initialize(params object[] p)
    {
        _move = (IMove)p[0];
        _selfTransform = (Transform)p[1];
        _playerTransform = (Transform)p[2];
        _moveSpeed = (float)p[3];
        _safeDistance = (float)p[4];
    }

    public override void Enter()
    {
        Debug.Log("Entering Flee State");
    }

    public override void Execute()
    {
        if (_playerTransform != null && _move != null)
        {
            float distance = Vector3.Distance(_selfTransform.position, _playerTransform.position);

            if (distance < _safeDistance)
            {
                // Escapar del jugador
                Vector3 dirAwayFromPlayer = (_selfTransform.position - _playerTransform.position).normalized;
                _move.Move(dirAwayFromPlayer * _moveSpeed);
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Flee State");
    }
}
