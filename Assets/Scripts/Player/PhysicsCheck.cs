using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("检测参数")]
    public Vector2 bottomOffset;
    public float checkRadius;
    public LayerMask groundLayer;

    [Header("状态")]
    public bool isGround;

    private void Update()
    {
        check();
    }

    public void check()
    {
        //检测地面
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset,checkRadius,groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRadius);
    }
}
