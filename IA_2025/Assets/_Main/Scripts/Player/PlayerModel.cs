using System;
using UnityEngine;

public class PlayerModel : MonoBehaviour, IMove
{
    [SerializeField] private float speed = 5f;
    private Rigidbody _rb;
    
    public Vector3 Position => transform.position;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    public virtual void Move(Vector3 dir)
    {
        dir *= speed;
        dir.y = _rb.linearVelocity.y;
        _rb.linearVelocity = dir;
    }
}
