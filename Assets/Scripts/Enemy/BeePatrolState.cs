using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeePatrolState : BaseState
{
    public Vector3 target;
    public Vector3 moveDir;//朝向Player

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        target = enemy.GetNewPoint();

        //currentEnemy.anim.SetBool("walk", true);
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }

        //到了巡逻点等待
        if(Mathf.Abs(target.x - currentEnemy.transform.position.x)<0.1f &&
            Mathf.Abs(target.y - currentEnemy.transform.position.y) < 0.1f)
        {
            currentEnemy.wait = true;
            target = currentEnemy.GetNewPoint();
        }

        //转向
        moveDir = (target - currentEnemy.transform.position).normalized;
        if(moveDir.x > 0)
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        if(moveDir.x < 0)
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);
    }

    public override void PhysicsUpdate()
    {
        if (!currentEnemy.wait && !currentEnemy.isHurt && !currentEnemy.isDead)
        {
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed * Time.deltaTime;
        }
        else
        {
            if(!currentEnemy.isHurt)
                currentEnemy.rb.velocity = Vector2.zero;
        }

    }
    public override void OnExit()
    {

    }

}
