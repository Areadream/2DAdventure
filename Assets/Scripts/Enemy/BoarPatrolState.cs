using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPatrolState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;

    }
    public override void LogicUpdate()
    {
        //TODO:发现player切换到chaseState
        if(currentEnemy.FoundPlayer())
        {
            //切换
            currentEnemy.SwitchState(NPCState.Chase);
        }



        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {

            currentEnemy.wait = true;
            //自己添加的，因为速度太快会停不下来 -> currentEnemy.rb.velocity = Vector3.zero;
            currentEnemy.rb.velocity = Vector3.zero;

            currentEnemy.anim.SetBool("walk", false);
            //currentEnemy.anim.SetBool("run", false);
        }

        //视频15:46 多添加了下面这句命令，为了让野猪可以播放行走的动画,我改到PhysicsUpdate，符合原来的逻辑
        //else
        //{
        //    currentEnemy.anim.SetBool("walk", true);
        //}
    }
    public override void PhysicsUpdate()
    {
        if (!currentEnemy.isHurt && !currentEnemy.isDead && !currentEnemy.wait)
            currentEnemy.anim.SetBool("walk", true);
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("walk", false);
    }

}
