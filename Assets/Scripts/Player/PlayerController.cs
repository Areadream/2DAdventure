using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("���")]
    public PlayerInputControl inputControl;
    public Vector2 inputDirection;
    public PhysicsCheck physicsCheck;
    public SpriteRenderer spriteRenderer;
    public CapsuleCollider2D coll;
    private Rigidbody2D rb;
    public PlayerAnimation playAnimation;

    [Header("��������")]
    public float speed;
    private float walkSpeed => speed / 2f;
    private float runSpeed;
    public float jumpForce; //��Ծ������Ծ˲��ʩ�ӵ���
    public float hurtForce;
    private Vector2 originalOffset;
    private Vector2 originalSize;
    [Header("״̬")]
    public bool isCrouch;
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    [Header("�������")]
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

        #region ǿ����·
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
        //����
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
        //�����ƶ�
        if(!isCrouch)
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);

        //���﷭ת
        //����1 �ı�transform.scale   
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0) 
            faceDir = -1;

        
        transform.localScale = new Vector3(faceDir, 1, 1);

        //����2 �ı�SpriteRenderer.flipX
        //bool flip = spriteRenderer.flipX;
        //if (inputDirection.x > 0)
        //    flip = false;
        //if (inputDirection.x < 0)
        //    flip = true;
        //spriteRenderer.flipX = flip;

        //����
        isCrouch = inputDirection.y < -0.5f && physicsCheck.isGround;
        if (isCrouch)
        {
            //�޸���ײ���
            coll.offset = new Vector2(-0.05f, 0.85f);
            coll.size = new Vector2(0.7f, 1.7f);
        }
        else
        {
            //��ԭ��ײ���
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
