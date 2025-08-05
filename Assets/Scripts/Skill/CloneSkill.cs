using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CloneSkill : Skill
{
    [SerializeField] bool canAttack;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] float cloneDuration;
    public override void UseSkill()
    {
        base.UseSkill();
    }
    public void CreateClone(Transform trans, Vector3 _offset)
    {
        //实例化的时候位置就设置好
        GameObject cloneObj = Instantiate(clonePrefab);
        cloneObj.GetComponent<CloneSkillController>().SetupClone(trans, 
            cloneDuration, canAttack, _offset);
        
    }
}
