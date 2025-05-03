using System;
using UnityEngine;

public class PlayerModel : MonoBehaviour, IMove, IAttack
{
    [SerializeField] private float speed;
    private Rigidbody _rb;
    private Action _onAttack = delegate { };

    public Action OnAttack { get => _onAttack; set => _onAttack = value; }

    public Vector3 Position => transform.position;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public virtual void Attack()
    {
        _onAttack();
    }
    public virtual void Move(Vector3 dir)
    {
        dir *= speed;
        dir.y = _rb.linearVelocity.y;
        _rb.linearVelocity = dir;
    }
}
