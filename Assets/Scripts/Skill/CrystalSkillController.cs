using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    Animator animator => GetComponentInChildren<Animator>();

    float crystalExistTimer;

    bool canExplore;
    bool canMove;
    float moveSpeed;
    [SerializeField] float attackCheckRadius;

    bool canGrow;
    [SerializeField] float growSpeed;

    Transform closestTarget;

    public void SetupCrystal(float _crystalDuration, bool _canExplore, 
        bool _canMove, float _moveSpeed, Transform _closestTarget)
    {
        crystalExistTimer = _crystalDuration;
        canExplore = _canExplore;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
    }
    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }
        if (canGrow) 
        { 
            transform.localScale = Vector2.Lerp(transform.localScale, 
                Vector2.one * 3, growSpeed * Time.deltaTime);   
        }

        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                closestTarget.position, Time.deltaTime * moveSpeed);
            if(Vector2.Distance(transform.position, closestTarget.position) < 1f)
            {
                FinishCrystal();
                canMove = false;
            }
        }
    }

    public void FinishCrystal()
    {
        if (canExplore)
        {
            canGrow = true;
            animator.SetTrigger("Explore");
        }
        else
        {
            SelfDestroy();
        }
    }

    public void SelfDestroy() => Destroy(gameObject);
    public void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
                transform.position, attackCheckRadius,
                1 << LayerMask.NameToLayer("Enemy"));
        foreach (Collider2D collider in colliders)
        {
            collider.gameObject.GetComponent<Entity>()?.Damage();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackCheckRadius);
    }
}
