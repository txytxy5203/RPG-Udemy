using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Entity : MonoBehaviour
{
    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockbackDirection;
    protected bool isKnocked;

    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }

    #region ��ײ���
    [Header("��ײ���")]
    [SerializeField] protected float groundCheckDis;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected float wallCheckDis;
    [SerializeField] protected Transform wallCheck;
    public Transform attackTrans;
    public float attackCheckRadius;
    #endregion

    //���������д�� ���� ����ʽ ��Ȼ����̳в��� �����ٿ���
    public int FaceDir { get; protected set; }
    public bool IsFaceRight { get; protected set; }

    #region ��ײ���
    public bool IsGrounded { get; private set; }
    public bool IsWalled { get; private set; }
    #endregion


    protected virtual void Awake()
    {
        
    }
    protected virtual void Start()
    {
        //��ʼ������
        IsFaceRight = true;
        FaceDir = 1;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        fx = GetComponent<EntityFX>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    protected virtual void Update()
    {
        //��ײ���
        IsGrounded = CheckGrounded();
        IsWalled = CheckWalled();
    }
    public virtual bool CheckGrounded() => Physics2D.Raycast(
    groundCheck.position, Vector2.down, groundCheckDis, groundLayer);
    public virtual bool CheckWalled() => Physics2D.Raycast(
        wallCheck.position, this.transform.right, wallCheckDis, groundLayer);
    

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x,
                               groundCheck.position.y - groundCheckDis));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x +
                        wallCheckDis * this.transform.right.x,
                               wallCheck.position.y));

        //������Χ
        Gizmos.DrawWireSphere(attackTrans.position, attackCheckRadius);
    }
    #region Flip
    public void Flip()
    {
        FaceDir = -FaceDir;
        IsFaceRight = !IsFaceRight;
        transform.Rotate(0, 180, 0);
    }
    protected void FlipController()
    {
        if (rb.velocity.x > 0 && !IsFaceRight)
        {
            Flip();
        }
        else if (rb.velocity.x < 0 && IsFaceRight)
        {
            Flip();
        }
    }
    #endregion
    #region Velocity
    public void SetVelocity(float x, float y)
    {
        //����������Ͳ��� �����ٶ�
        if (isKnocked)
        {
            return;        
        }

        rb.velocity = new Vector2(x, y);
        FlipController();
    }
    public void SetVelocity(Vector2 vector)
    {
        rb.velocity = vector;
    }
    #endregion

    public virtual void Damage()
    {
        Debug.Log("attack");
        fx.Hit();
        StartCoroutine(HitKnockBack());
    }
    protected virtual IEnumerator HitKnockBack()
    {
        //���ﲻʹ�� �����ٶȵ�api ��ΪFlip��������
        rb.velocity = new Vector2(knockbackDirection.x * -FaceDir, knockbackDirection.y);
        isKnocked = true;
        yield return new WaitForSeconds(.07f);
        isKnocked = false;
    }
    public void MakeTransprent(bool _transprent)
    {
        if (_transprent)
        {
            sr.color = Color.clear;
        }
        else 
        { 
            sr.color = Color.white;
        }
    }
}
