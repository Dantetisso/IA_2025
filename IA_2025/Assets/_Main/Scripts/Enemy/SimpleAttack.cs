using System;
using UnityEngine;

public class SimpleAttack : MonoBehaviour, IAttack
{
    public float attackRange = 2f;

    public Action OnAttack { get; set; }  // IMPLEMENTACIÓN CORRECTA

    public float GetAttackRange => attackRange; // ESTABA BIEN, pero sacá ese "IAttack." raro que pusiste

    public void Attack()
    {
        Debug.Log("ATTACKING!");
        OnAttack?.Invoke();
    }
}
