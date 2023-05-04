using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SnailPatrolState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;

    }
    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Skill);
        }
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {

            currentEnemy.wait = true;
            //自己添加的，因为速度太快会停不下来 -> currentEnemy.rb.velocity = Vector3.zero;
            currentEnemy.rb.velocity = Vector3.zero;

            currentEnemy.anim.SetBool("walk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("walk", true);
        }
    }
    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }

}
