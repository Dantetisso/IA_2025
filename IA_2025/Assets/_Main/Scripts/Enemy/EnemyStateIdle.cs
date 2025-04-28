using System;

public class EnemyStateIdle : State<EnemyStatesEnum>
{
    public override void Enter()
    {
        base.Enter();
        Console.WriteLine("Entrando en estado Idle");
    }

    public override void Execute()
    {
        base.Execute();
        // Aquí no hacemos nada en cada frame mientras está en Idle
    }

    public override void Exit()
    {
        base.Exit();
        Console.WriteLine("Saliendo del estado Idle");
    }
}
