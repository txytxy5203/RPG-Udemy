using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    #region Component
    Rigidbody2D rb;
    CircleCollider2D cd;
    Animator animator;
    #endregion

    #region Bounce
    float attackRadius;
    int maxAttackNumber;
    float speedOfBounce;
    int currentAtkNum = 0;
    List<Transform> attackList = new List<Transform>();
    #endregion

    #region Pierce
    int maxPierceNumber;
    #endregion

    #region Spin
    float spinDis;
    float spinAtkRadius;
    /// <summary>
    /// 旋转持续时间
    /// </summary>
    float spinDurationTime;
    /// <summary>
    /// 旋转时的攻击间隔时间
    /// </summary>
    float spinAtkIntervalTime;
    #endregion



    float swordGravity;
    E_SwordType swordType;

    //some tag
    bool isReturn;
    bool isAttacking;
    bool isRotate;
    [SerializeField] float returnSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
        animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if(isRotate) 
            transform.right = rb.velocity;
        if (isReturn)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                Player.Instance.transform.position, Time.deltaTime * returnSpeed);
            if (Vector2.Distance(transform.position, Player.Instance.transform.position) < .3f)
            {
                Player.Instance.CatchSword();
                
            }
        }

        if(isAttacking)
        {
            switch (swordType)
            {
                case E_SwordType.Bounce:
                    //Move to current enemy
                    transform.position = Vector2.MoveTowards(transform.position,
                        attackList[currentAtkNum].position, Time.deltaTime * speedOfBounce);

                    if (Vector2.Distance(transform.position, attackList[currentAtkNum].position) < .5f)
                    {
                        //攻击 Enemy
                        attackList[currentAtkNum].GetComponent<Enemy>()?.Damage();
                        maxAttackNumber--;
                        currentAtkNum++;

                        //条件判断
                        if(maxAttackNumber <= 0)
                        {
                            ReturnSword();
                        }
                        if (currentAtkNum >= attackList.Count)
                        { 
                            currentAtkNum = 0;
                        }
                    } 
                    break;
               
                case E_SwordType.Spin:
                    if (Vector2.Distance(transform.position, 
                        Player.Instance.transform.position) > spinDis)
                    {
                        // begin to spin atk check
                        InvokeRepeating("SpinAtk", 0, spinAtkIntervalTime);
                        // avoid repeat call Invoke
                        isAttacking = false;
                        DisableRb();
                        StartCoroutine(SpinTimer());
                    }
                    break;
                case E_SwordType.Regular:
                case E_SwordType.Pierce:
                default:
                    break;
            }
        }


    }
    public void SetupSwordBase(Vector2 _speed, float _gravity, E_SwordType _type)
    {
        if(_type == E_SwordType.Pierce)
            animator.SetBool("Rotation", false);
        else
            animator.SetBool("Rotation", true);
        rb.velocity = _speed;
        rb.gravityScale = _gravity;
        swordType = _type;
        isRotate = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturn) return;

        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            cd.enabled = false;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            transform.parent = collision.transform;
            animator.SetBool("Rotation", false);
            isRotate = false;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            switch (swordType)
            {
                case E_SwordType.Regular:
                    DisableRb();
                    animator.SetBool("Rotation", false);
                    transform.parent = collision.transform;
                    isRotate = false;
                    break;
                case E_SwordType.Bounce:
                    //attack first enemy
                    collision.GetComponent<Enemy>()?.Damage();
                    maxAttackNumber--;
                    currentAtkNum++;

                    //条件判断
                    if (maxAttackNumber <= 0)
                    {
                        ReturnSword();
                    }
                    if (currentAtkNum >= attackList.Count)
                    {
                        currentAtkNum = 0;
                    }

                    //disable some component
                    DisableRb();

                    //范围检测 初始化 attackList
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(
                        transform.position, attackRadius, 1 << LayerMask.NameToLayer("Enemy"));
                    foreach (var cd in colliders)
                    {
                        attackList.Add(cd.transform);
                    }                    

                    isAttacking = true;
                    isRotate = true;
                    break;
                case E_SwordType.Pierce:
                    collision.GetComponent<Enemy>()?.Damage();  

                    maxPierceNumber--;
                    if (maxPierceNumber <= 0)
                    {
                        //same with Regular Mode
                        DisableRb();
                        animator.SetBool("Rotation", false);
                        transform.parent = collision.transform;
                        isRotate = true;
                    }
                    break;
                case E_SwordType.Spin:
                    collision.GetComponent<Enemy>()?.Damage();
                    break;
                default:
                    break;
            }
        }
        
    }
    public void ReturnSword()
    {
        isRotate = false;
        isReturn = true;
        isAttacking = false;
        animator.SetBool("Rotation", false);
        transform.parent = null;

        DisableRb();
    }
    void DisableRb()
    {
        cd.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    #region Init
    public void InitBounce(float _attackRadius, int _maxAttackNumber, float _speedOfBounce)
    {
        this.attackRadius = _attackRadius;
        this.maxAttackNumber = _maxAttackNumber;
        this.speedOfBounce = _speedOfBounce;
    }
    public void InitPierce(int _maxPierceNumber)
    {
        this.maxPierceNumber = _maxPierceNumber;
    }
    public void InitSpin(float _spinDis, float _spinAtkRadius,float _spinDurationTime, 
        float _spinAtkIntervalTime)
    {
        isAttacking = true;
        spinDis = _spinDis;
        spinAtkRadius = _spinAtkRadius;
        spinDurationTime = _spinDurationTime;
        spinAtkIntervalTime = _spinAtkIntervalTime;
    }
    #endregion

    void SpinAtk()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position, spinAtkRadius, 1 << LayerMask.NameToLayer("Enemy"));
        foreach (var cd in colliders)
        {
            cd.GetComponent<Enemy>()?.Damage();
        }
    }
    IEnumerator SpinTimer()
    {
        yield return new WaitForSeconds(spinDurationTime);
        CancelInvoke("SpinAtk");
        ReturnSword();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(animator.gameObject.transform.position, spinAtkRadius);  
    }
}