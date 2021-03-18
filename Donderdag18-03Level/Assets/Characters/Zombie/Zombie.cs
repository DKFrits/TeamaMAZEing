using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : SimpleZombieFSM
{
    public NavMeshAgent agent;
    private Animator anim;

    public float maxAttackRange;
    public float maxWalkRange;
    public float maxRunRange;

    // animation number bindings
    private readonly string zombieAnimationVariable = "ZombieState";
    private readonly int idleAnim = 0;
    private readonly int walkAnim = 1;
    private readonly int attackAnim = 2;
    private readonly int runAnim = 3;

    // player information
    private Transform playerTransform;

    // attack/hit variables
    private bool setAttackTimer;
    private float attackTimer;
    private float damageTime = 0.7f;

    public enum ZombieState
    {
        None,
        Idle,
        Walk,
        Attack,
        Run
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
            case ZombieState.Run: UpdateRunState(); break;
        }

        // Debug.Log("curstate:: " + curState);

    }

    private void UpdateIdleState()
    {

        anim.SetInteger(zombieAnimationVariable, idleAnim);

        var closeEnough = (Vector3.Distance(transform.position, playerTransform.position) <= maxWalkRange);
        if (closeEnough)
        {
            curState = ZombieState.Walk;
        }
    }

    private void UpdateWalkState()
    {

        WalkTowardsPlayer();

        var outOfRange = (Vector3.Distance(transform.position, playerTransform.position) >= maxWalkRange);
        if (outOfRange)
        {
            curState = ZombieState.Idle;
        }

        var inAttackRange = (Vector3.Distance(transform.position, playerTransform.position) <= maxAttackRange);
        if (inAttackRange)
        {
            setAttackTimer = true;
            curState = ZombieState.Attack;
        }

        var inRunRange = (Vector3.Distance(transform.position, playerTransform.position) <= maxRunRange);
        if (inRunRange)
        {
            curState = ZombieState.Run;
        }
    }

    private void UpdateAttackState()
    {
        AttackPlayer();
        var outOfAttackRange = (Vector3.Distance(transform.position, playerTransform.position) >= maxAttackRange);
        if (outOfAttackRange)
        {
            if ((Vector3.Distance(transform.position, playerTransform.position) >= maxRunRange))
            {
                curState = ZombieState.Walk;
            } else
            {
                curState = ZombieState.Run;
            }
        }
    }

    private void UpdateRunState()
    {

        RunTowardsPlayer();

        var inAttackRange = (Vector3.Distance(transform.position, playerTransform.position) <= maxAttackRange);
        if (inAttackRange)
        {
            setAttackTimer = true;
            curState = ZombieState.Attack;
        }

        var outOfRunRange = (Vector3.Distance(transform.position, playerTransform.position) >= maxRunRange);
        if (outOfRunRange)
        {
            curState = ZombieState.Walk;
        }
    }

    private void WalkTowardsPlayer()
    {
        // set zombie speed
        agent.speed = 0.5f;

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

        DamagePlayerIfHit();
    }

    private void RunTowardsPlayer()
    {
        // set zombie speed
        agent.speed = 5f;

        // Debug.Log("afstand:: " + Vector3.Distance(transform.position, playerTransform.position));
        // set walking animation
        anim.SetInteger(zombieAnimationVariable, runAnim);

        // look at player
        transform.LookAt(playerTransform);

        // move to player (path based on navmesh)
        agent.destination = playerTransform.position;
    }

    private void DamagePlayerIfHit()
    {

        // if attack animation is on for 1 sec -> player is hit
        
        var hit = PlayerDamaged();
        if (hit)
        {
            GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
            playerTransform = objPlayer.transform;
            var script = objPlayer.GetComponent<SC_FPSController>();
            script.reduceHealthByOne();
            setAttackTimer = true;
        }
    }

    private bool PlayerDamaged()
    {
        if (setAttackTimer)
        {
            attackTimer = Time.time;
            setAttackTimer = false;
        }
        else
        {
            if (attackTimer + damageTime < Time.time)
            {
                return true;
            }
        }
        return false;
    }
}
