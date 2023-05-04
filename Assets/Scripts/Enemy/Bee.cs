using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : Enemy
{
    public float patrolRadius;
    public float CubeX,CubeY;
    protected override void Awake()
    {
        base.Awake();
        patrolState = new BeePatrolState();
        chaseState = new BeeChaseState();
    }

    public override bool FoundPlayer()
    {
        var obj = Physics2D.OverlapCircle(transform.position, checkDistance, attackLayer);
        if (obj)
        {
            attacker = obj.transform;
        }
        return obj;
    }

    public override void OnDrawGizmosSelected()
    {
        
        Gizmos.DrawWireSphere(transform.position, checkDistance);
        Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position, patrolRadius);
        //巡逻范围
        Gizmos.DrawWireCube(spawnPoint, new Vector3(patrolRadius*2, patrolRadius*2));
    }

    public override Vector3 GetNewPoint()
    {
        var targetX = Random.Range(-patrolRadius, patrolRadius);
        var targetY = Random.Range(-patrolRadius, patrolRadius);

        return spawnPoint +  new Vector3(targetX, targetY);
    }

    public override void Move()
    {

    }
}
