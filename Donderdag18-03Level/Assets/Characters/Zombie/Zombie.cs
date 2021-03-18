using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : SimpleZombieFSM
{
    public NavMeshAgent agent;
    private Animator anim;

    public float maxAttackRange;
    public float maxChaseRange;

    // animation number bindings
    private readonly string zombieAnimationVariable = "ZombieState";
    private readonly int idleAnim = 0;
    private readonly int walkAnim = 1;
    private readonly int attackAnim = 2;

    // player information
    private Transform playerTransform;

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

        curState = ZombieState.Idle;

        // get player transform
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        anim = GetComponent<Animator>();
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

        // Debug.Log("curstate:: " + curState);

    }

    private void UpdateIdleState()
    {

        anim.SetInteger(zombieAnimationVariable, idleAnim);

        var closeEnough = (Vector3.Distance(transform.position, playerTransform.position) <= maxChaseRange);
        if (closeEnough)
        {
            curState = ZombieState.Walk;
        }
    }

    private void UpdateWalkState()
    {

        WalkTowardsPlayer();

        var outOfRange = (Vector3.Distance(transform.position, playerTransform.position) >= maxChaseRange);
        if (outOfRange)
        {
            curState = ZombieState.Idle;
        }

        var inAttackRange = (Vector3.Distance(transform.position, playerTransform.position) <= maxAttackRange);
        if (inAttackRange)
        {
            curState = ZombieState.Attack;
        }
    }

    private void UpdateAttackState()
    {

        AttackPlayer();

        var outOfAttackRange = (Vector3.Distance(transform.position, playerTransform.position) >= maxAttackRange);
        if (outOfAttackRange)
        {
            curState = ZombieState.Walk;
        }
    }

    private void WalkTowardsPlayer()
    {
        // Debug.Log("afstand:: " + Vector3.Distance(transform.position, playerTransform.position));
        // set walking animation
        anim.SetInteger(zombieAnimationVariable, walkAnim);

        // look at player
        transform.LookAt(playerTransform);

        // move to player (path based on navmesh)
        agent.destination = playerTransform.position;
    }

    private void AttackPlayer()
    {
        anim.SetInteger(zombieAnimationVariable, attackAnim);
        agent.destination = transform.position;

        var hit = false;
        if (hit)
        {
            Debug.Log("player is hit");
        }
    }
}
