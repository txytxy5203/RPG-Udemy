using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvent : MonoBehaviour
{
    public Enemy enemy;
    public void AnimFinishEvent()
    {
        enemy.AnimatorFinish();
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
                enemy.attackTrans.position, enemy.attackCheckRadius, 
                1 << LayerMask.NameToLayer("Player"));
        foreach (Collider2D collider in colliders)
        {
            collider.gameObject.GetComponent<Entity>()?.Damage();
        }
    }
    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
