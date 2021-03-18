using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : SimpleZombieFSM
{

    public enum ZombieState
    {
        None,
        Idle,
        Walk,
        Attack
    }

    public ZombieState curState;



    protected override void Initialize()
    {

    }

    //Update each frame
    protected override void FSMUpdate()
    {
        switch (curState)
        {
            case ZombieState.Idle: UpdateIdleState(); break;
            case ZombieState.Walk: UpdateWalkState(); break;
            case ZombieState.Attack: UpdateAttackState(); break;
        }

    }

    private void UpdateIdleState()
    {

    }

    private void UpdateWalkState()
    {

    }

    private void UpdateAttackState()
    {

    }
}
