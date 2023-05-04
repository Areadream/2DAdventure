using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("组件")]
    public PlayerInputControl inputControl;
    public Vector2 inputDirection;
    public PhysicsCheck physicsCheck;
    public SpriteRenderer spriteRenderer;
    public CapsuleCollider2D coll;
    private Rigidbody2D rb;
    public PlayerAnimation playAnimation;

    [Header("基本参数")]
    public float speed;
    private float walkSpeed => speed / 2f;
    private float runSpeed;
    public float jumpForce; //跳跃力，跳跃瞬间施加的力
    public float hurtForce;
    private Vector2 originalOffset;
    private Vector2 originalSize;
    [Header("状态")]
    public bool isCrouch;
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    [Header("物理材质")]
    public PhysicsMaterial2D wall;
    public PhysicsMaterial2D normal;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputControl();
        physicsCheck = GetComponent<PhysicsCheck>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<CapsuleCollider2D>();
        playAnimation = GetComponent<PlayerAnimation>();

        originalOffset = coll.offset; 
        originalSize = coll.size;

        inputControl.Gameplay.Jump.started += Jump;

        #region 强制走路
        runSpeed = speed;
        inputControl.Gameplay.MoveButton.performed +=  ctx =>
        {
            if (physicsCheck.isGround)
                speed = walkSpeed;
        };
        inputControl.Gameplay.MoveButton.canceled += ctx =>
        {
            if (physicsCheck.isGround)
                speed = runSpeed;
        };
        #endregion
        //攻击
        inputControl.Gameplay.Attack.started += PlayerAttack;
    }



    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();

        CheckState();
    }

    private void FixedUpdate()
    {
        if(!isHurt && !isAttack)
            Move();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log(other.name);

    }

    public void Move()
    {
        //人物移动
        if(!isCrouch)
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);

        //人物翻转
        //方法1 改变transform.scale   
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0) 
            faceDir = -1;

        
        transform.localScale = new Vector3(faceDir, 1, 1);

        //方法2 改变SpriteRenderer.flipX
        //bool flip = spriteRenderer.flipX;
        //if (inputDirection.x > 0)
        //    flip = false;
        //if (inputDirection.x < 0)
        //    flip = true;
        //spriteRenderer.flipX = flip;

        //蹲下
        isCrouch = inputDirection.y < -0.5f && physicsCheck.isGround;
        if (isCrouch)
        {
            //修改碰撞体积
            coll.offset = new Vector2(-0.05f, 0.85f);
            coll.size = new Vector2(0.7f, 1.7f);
        }
        else
        {
            //还原碰撞体积
            coll.offset = originalOffset;
            coll.size = originalSize;
        }
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        if(physicsCheck.isGround)
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        if (physicsCheck.isGround)
        {
            isAttack = true;
            playAnimation.PlayAttack();
        }
    }

    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir =
            new Vector2((transform.position.x - attacker.transform.position.x), 0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }

    private void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;
    }

}
