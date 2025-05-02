using UnityEngine;

public class NPCSPatrol<T> : NPCSBase<T>
{
    private Transform _target;
    private NPCModel _model;
    
    public NPCSPatrol(NPCModel model)
    {
        _model = model;
    }

    public override void Enter()
    {
        _target = _model.GetWaypoint();
    }

    public override void Execute()
    {
        base.Execute();
        var dir = _target.transform.position - _move.Position;
        _move.Move(dir.normalized);
        _look.LookDir(dir.normalized);
    }
}
