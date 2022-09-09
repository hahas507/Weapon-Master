using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagbeetleFSM : EnemyState
{
    protected Idle idleState;
    protected Wander wanderState;

    protected override void Awake()
    {
        idleState = new Idle();
        wanderState = new Wander();
    }
}
