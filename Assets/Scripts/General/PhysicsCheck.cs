using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PhysicsCheck : MonoBehaviour
{
    private PlayerController playerController;
    [Header("组件")]
    public CapsuleCollider2D coll;
    public Rigidbody2D rb;
    [Header("检测参数")]
    public bool manual;
    public bool isPlayer;
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float checkRadius;
    public LayerMask groundLayer;

    [Header("状态")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;
    public bool onWall;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        if(!manual )
        {
            coll = GetComponent<CapsuleCollider2D>();
            if (!manual)
            {
                rightOffset = new Vector2((coll.bounds.size.x + coll.offset.x) / 2, coll.bounds.size.y / 2);
                leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
            }
        }
        if (isPlayer)
        {
            playerController = GetComponent<PlayerController>();
        }
    }

    private void Update()
    {
        check();
    }

    public void check()
    {
        //检测地面
        if(onWall)
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2( bottomOffset.x * transform.localScale.x, bottomOffset.y),checkRadius,groundLayer);
        else
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2( bottomOffset.x * transform.localScale.x, 0),checkRadius,groundLayer);

        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRadius, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRadius, groundLayer);

        //在墙壁上
        if(isPlayer)
        {
            onWall = (touchLeftWall && playerController.inputDirection.x < 0f  || touchRightWall && playerController.inputDirection.x > 0f ) && rb.velocity.y < 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRadius);

        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRadius);


        //Vector2 center = capsuleCollider.bounds.center;
        //float xLeft = capsuleCollider.size.x/2;
        //float xRight = -capsuleCollider.size.x/2;
        ////Vector2 center = (Vector2)transform.position + bottomOffset;
        //Gizmos.DrawWireSphere(new Vector2(center.x + xRight, center.y), checkRadius);
        //Gizmos.DrawWireSphere(new Vector2(center.x + xLeft, center.y), checkRadius);
    }
}
