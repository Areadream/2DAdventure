using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPatrolState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
    }
    public override void LogicUpdate()
    {
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.wait = true;
            //�Լ���ӵģ���Ϊ�ٶ�̫���ͣ������ -> currentEnemy.rb.velocity = Vector3.zero;
            currentEnemy.rb.velocity = Vector3.zero;

            currentEnemy.anim.SetBool("walk", false);
            //currentEnemy.anim.SetBool("run", false);
        }
        //��Ƶ15:46 �����������������Ϊ����Ұ����Բ������ߵĶ���,�Ҹĵ�PhysicsUpdate������ԭ�����߼�
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
