using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSIdle<T> : NPCSBase<T>
{
    private NPCModel _model;
    private Coroutine _coroutine;
    private int _seconds;
    private Dictionary<WaitEnum, float> _waitTime = new Dictionary<WaitEnum, float>();

    public NPCSIdle(NPCModel model)
    {
        _model = model;
        _waitTime.Add(WaitEnum.Aggressive, 20);
        _waitTime.Add(WaitEnum.Proactive, 30);
        _waitTime.Add(WaitEnum.Lazy, 50);
        _waitTime.Add(WaitEnum.Sleepy, 75);
    }
    
    public override void Enter()
    {
        base.Enter();
        _move.Move(Vector3.zero);
        _coroutine = CoroutineExecutor.Instance.StartCoroutine(IdleCoroutine());
    }

    private IEnumerator IdleCoroutine()
    {
        yield return new WaitForSeconds(GetWaitTime());
        _model.ChangeWaypointIndex();
        _coroutine = null;
    }

    public override void Exit()
    {
        base.Exit();
        if (_coroutine != null)
        {
            _model.ChangeWaypointIndex();
            CoroutineExecutor.Instance.StopCoroutineExecution(_coroutine);
        }
    }

    private float GetWaitTime()
    {
        var wait = MyRandom.Roulette(_waitTime);
        switch (wait)
        {
            case WaitEnum.Sleepy:
                return 5;
            case WaitEnum.Lazy:
                return 3;
            case WaitEnum.Proactive:
                return 1.5f;
            case WaitEnum.Aggressive:
                return 0.5f;
            default:
                return 1;
        }
    }
    
}
