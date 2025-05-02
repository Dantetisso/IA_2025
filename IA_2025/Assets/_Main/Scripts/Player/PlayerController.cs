using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
        private FSM<StateEnum> _fsm;
        private void Awake()
        {
               InitializeFSM();
        }
        private void InitializeFSM()
        {
                _fsm = new FSM<StateEnum>();
                var move = GetComponent<IMove>();
                var look = GetComponent<ILook>();
                var attack = GetComponent<IAttack>();

                var stateList = new List<PSBase<StateEnum>>();

                var idle = new PSIdle<StateEnum>(StateEnum.Walk);
                var walk = new PSWalk<StateEnum>(StateEnum.Idle);
                
                idle.AddTransition(StateEnum.Walk, walk);
                walk.AddTransition(StateEnum.Idle, idle);
                
                stateList.Add(idle);
                stateList.Add(walk);
                
                for (int i = 0; i < stateList.Count; i++)
                {
                        stateList[i].Initialize(move, look, attack);
                }

                _fsm.SetInit(idle);
        }

        private void Update()
        {
                _fsm.OnExecute();
        }
}
