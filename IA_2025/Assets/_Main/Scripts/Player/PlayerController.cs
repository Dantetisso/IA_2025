using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
        private IMove _move;
        private ILook _look;
        private void Awake()
        {
                _move = GetComponent<IMove>();
                _look = GetComponent<ILook>();
        }

        private void Update()
        {
                var dir = new Vector3(InputManager.GetMove().x, 0, InputManager.GetMove().y);
                _move.Move(dir);
                if (dir != Vector3.zero)
                {
                        _look.LookDir(dir);
                }
        }
}
