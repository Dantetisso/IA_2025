using UnityEngine;

public class NPCView : MonoBehaviour, ILook
{
    [SerializeField] private float speedRot = 10f;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rb;
    public void LookDir(Vector3 dir)
    {
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * speedRot);
    }
    
    private void Update()
    {
        OnMoveAnim();
    }

    private void OnMoveAnim()
    {
        anim.SetFloat("rbVel", rb.linearVelocity.magnitude);
    }
}
