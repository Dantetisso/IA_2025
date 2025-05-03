using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distanceCheck = 0.05f;
    private FSM<StateEnum> _fsm; 
    private NPCModel _model;
    private LineOfSightMono _los;
    private ITreeNode _root;
    
    private void Awake()
    {
        _model = GetComponent<NPCModel>();
        _los = GetComponent<LineOfSightMono>();
    }
    void Start()
    {
        InitializedFSM();
        InitializedTree();
    }
    void Update()
    {
        _fsm.OnExecute();
        print(_fsm.CurrentState);
        _root.Execute();
    }
    private void FixedUpdate()
    {
        _fsm.OnFixExecute();
    }
    void InitializedFSM()
    {
        _fsm = new FSM<StateEnum>();
        var look = GetComponent<ILook>();

        var idle = new NPCSIdle<StateEnum>(_model);
        var attack = new NPCSAttack<StateEnum>();
        var chase = new NPCSChase<StateEnum>(target);
        var goZone = new NPCSPatrol<StateEnum>(_model);

        var stateList = new List<PSBase<StateEnum>>();
        stateList.Add(idle);
        stateList.Add(attack);
        stateList.Add(chase);
        stateList.Add(goZone);

        idle.AddTransition(StateEnum.Chase, chase);
        idle.AddTransition(StateEnum.Spin, attack);
        idle.AddTransition(StateEnum.GoZone, goZone);

        attack.AddTransition(StateEnum.Idle, idle);
        attack.AddTransition(StateEnum.Chase, chase);
        attack.AddTransition(StateEnum.GoZone, goZone);

        chase.AddTransition(StateEnum.Idle, idle);
        chase.AddTransition(StateEnum.Spin, attack);
        chase.AddTransition(StateEnum.GoZone, goZone);

        goZone.AddTransition(StateEnum.Chase, chase);
        goZone.AddTransition(StateEnum.Spin, attack);
        goZone.AddTransition(StateEnum.Idle, idle);

        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(_model, look, _model);
        }

        _fsm.SetInit(idle);
    }

    void InitializedTree()
    {
        var idle = new ActionNode(() => _fsm.Transition(StateEnum.Idle));
        var attack = new ActionNode(() => _fsm.Transition(StateEnum.Spin));
        var chase = new ActionNode(() => _fsm.Transition(StateEnum.Chase));
        var goZone = new ActionNode(() => _fsm.Transition(StateEnum.GoZone));

        var qCanAttack = new QuestionNode(QuestionCanAttack, attack, chase);
        var qGoToZone = new QuestionNode(QuestionGoToZone, goZone, idle);
        var qTargetInView = new QuestionNode(QuestionTargetInView, qCanAttack, qGoToZone);

        _root = qTargetInView;
    }
    private bool QuestionCanAttack()
    {
        return Vector3.Distance(_model.Position, target.position) <= _model.AttackRange;
    }
    private bool QuestionGoToZone()
    {
        var modelPos = new Vector3(_model.Position.x, 0, _model.Position.z);
        var wayPos = new Vector3(_model.GetWaypoint().position.x, 0, _model.GetWaypoint().position.z);
        var distance = Vector3.Distance(modelPos, wayPos );
        return distance > distanceCheck;
    }
    private bool QuestionTargetInView()
    {
        if (target == null) return false;
        return _los.LOS(target);
    }
}
