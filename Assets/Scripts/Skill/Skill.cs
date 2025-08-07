using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float coolDown;
    protected float coolDownTimer;
    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }
    protected virtual void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }
    public virtual bool CanUseSkill()
    {
        if (coolDownTimer < 0)
        {
            UseSkill();
            coolDownTimer = coolDown;
            return true;
        }
        return false;
    }
    public virtual void UseSkill()
    {

    }
    protected virtual Transform FindClosestEnemy(Transform _checkTrans)
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTrans.position, 
                                 50, 1 << LayerMask.NameToLayer("Enemy"));
        Collider2D closestCollider = colliders.OrderBy(
            collider =>
                Vector2.Distance(_checkTrans.position, collider.transform.position))
                    .FirstOrDefault();
        return closestCollider.transform;
    }
}
