using UnityEngine;

public class NPCSIdle<T> : NPCSBase<T>
{
    private NPCModel _model;

    public NPCSIdle(NPCModel model)
    {
        _model = model;
    }
    
    public override void Enter()
    {
        base.Enter();
        _move.Move(Vector3.zero);
        _model.ChangeWaypointIndex();
    }
}
