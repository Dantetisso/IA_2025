using UnityEngine;

public class EnemyStateAttack : State<EnemyStatesEnum>
{
    IAttack _attack;

    public override void Initialize(params object[] p)
    {
        _attack = (IAttack)p[0];
    }

    public override void Enter()
    {
        Debug.Log("Entering Attack State");
    }

    public override void Execute()
    {
        Debug.Log("Attack!");
        // Podrías llamar aquí a _attack.Attack() si tienes lógica real de ataque
    }

    public override void Exit()
    {
        Debug.Log("Exiting Attack State");
    }
}
