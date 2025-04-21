using UnityEngine;

public class PlayerView : MonoBehaviour, ILook
{
    [SerializeField] private float speedRot = 10f;
    public void LookDir(Vector3 dir)
    {
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * speedRot);
    }
}
