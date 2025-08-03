using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    public Player player;

    public void AnimFinishEvent()
    {
        player.AnimatorFinish();
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
                player.attackTrans.position, player.attackCheckRadius, 1 << LayerMask.NameToLayer("Enemy"));
        foreach (Collider2D collider in colliders)
        {
            Debug.Log("attack enemy");
            collider.gameObject.GetComponent<Entity>()?.Damage();
        }
    }
    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
