using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CloneSkillController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sr;

    [SerializeField] float colorLoosingSpeed;
    float cloneTimer;

    [SerializeField] Transform attackCheck;
    [SerializeField] float attackCheckRadius;
    

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sr.color = new Color(1,1,1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));
            if (sr.color.a < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
    public void SetupClone(Transform trans, float cloneDuration, bool canAttack)
    {

        if (canAttack) 
        {
            animator.SetInteger("AttackNumber", Random.Range(1, 4));
        }
        transform.position = trans.position;
        cloneTimer = cloneDuration;
    }

    private void AnimFinishEvent()
    {
        cloneTimer = -1f;
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
                attackCheck.position, attackCheckRadius, 
                1 << LayerMask.NameToLayer("Enemy"));
        foreach (Collider2D collider in colliders)
        {
            Debug.Log("attack enemy");
            collider.gameObject.GetComponent<Entity>()?.Damage();
        }
    }
    /// <summary>
    /// 自动索敌 暂时先不添加 后续可以再加上
    /// </summary>
    void FaceClosestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, 25);
        Collider2D closestCollider = colliders.OrderBy(
            collider => 
                Vector2.Distance(this.transform.position, collider.transform.position))
                    .FirstOrDefault();

    }
}
