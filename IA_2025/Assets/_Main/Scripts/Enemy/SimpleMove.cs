using UnityEngine;

public class SimpleMove : MonoBehaviour, IMove
{
    public Vector3 Position => transform.position;

    public void Move(Vector3 dir)
    {
        transform.position += dir * Time.deltaTime;
    }
}