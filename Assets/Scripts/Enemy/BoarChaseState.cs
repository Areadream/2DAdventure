using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        Debug.Log("Enter Chase State.");
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("run", true);
    }

    public override void LogicUpdate()
    {
        if(currentEnemy.loseTimeCounter<=0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }

        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);
        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        currentEnemy.loseTimeCounter = currentEnemy.loseTime;
        currentEnemy.anim.SetBool("run", false);
    }
}
