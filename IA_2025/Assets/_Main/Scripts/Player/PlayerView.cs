using System;
using UnityEngine;

public class PlayerView : MonoBehaviour, ILook
{
    [SerializeField] private float speedRot = 10f;
    [SerializeField] private Animator _anim;
    [SerializeField] private Rigidbody _rb;
    public void LookDir(Vector3 dir)
    {
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * speedRot);
    }

    private void Update()
    {
        Debug.Log($"{_rb.linearVelocity.magnitude}");
        
        OnMoveAnim();
    }

    private void OnMoveAnim()
    {
        _anim.SetFloat("rbVel", _rb.linearVelocity.magnitude);
    }
}
