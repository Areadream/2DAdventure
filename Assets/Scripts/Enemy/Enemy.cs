using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("���")]
    public Rigidbody2D rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public PhysicsCheck physicsCheck;


    [Header("��������")]
    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public Vector3 faceDir;
    public Transform attacker;
    public float hurtForce;



    [Header("��ʱ��")]
    public bool wait;
    public float waitTime;
    public float waitTimeCount;
    public float loseTime;
    public float loseTimeCount;

    [Header("״̬")]
    public bool isHurt;
    public bool isDead;

    protected BaseState patrolState;
    protected BaseState chaseState;
    private BaseState currentState;

    [Header("���")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;




    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
        //currentSpeed = chaseSpeed;

        waitTimeCount = waitTime;
        loseTimeCount = loseTime;
    }

    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        //if((physicsCheck.touchLeftWall && faceDir.x < 0) || (physicsCheck.touchRightWall && faceDir.x > 0))
        //{
        //    wait = true;
        //    anim.SetBool("walk", false);
        //    anim.SetBool("run", false);
        //}

        currentState.LogicUpdate();
        TimeCounter();
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate();
        if(!isHurt && !isDead && !wait)
            Move();
    }


    private void OnDisable()
    {
        currentState.OnExit();
    }

    public virtual void Move()
    {
        
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime,rb.velocity.y);
    }

    public void TimeCounter()
    {
        if(wait)
        {
            waitTimeCount -= Time.deltaTime;
            if(waitTimeCount<0)
            {
                wait = false;
                waitTimeCount = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }
        //����Ϊ�ĸ���
        if (!FoundPlayer())
        {
            if(loseTimeCount > 0)
                loseTimeCount -= Time.deltaTime;
        }
        else
        {
            loseTimeCount = loseTime;
        }

    }


    #region �¼�ִ�з���
    public void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;
        if (attackTrans.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (attackTrans.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);

        isHurt = true;
        //�ҵĲ���
        wait = false;
        waitTimeCount = waitTime;
        //

        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x,0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);
        StartCoroutine(OnHurt(dir));
    }

    public void OnDie()
    {
        gameObject.layer = 2;
        anim.SetBool("dead", true);
        isDead = true;
    }

    IEnumerator OnHurt(Vector2 dir)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f);
        isHurt = false;
    }

    public void DestoryAfterAnimation()
    {
        Destroy(this.gameObject);
    }

    public bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position+(Vector3)centerOffset,checkSize,0,faceDir,checkDistance,attackLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance * -transform.localScale.x, 0), 0.2f);
    }
    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _ => null
        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);

    }

    #endregion
}
