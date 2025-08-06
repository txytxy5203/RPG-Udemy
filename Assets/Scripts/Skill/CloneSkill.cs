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
        //偏移值为正的话 就把clone的翻转一下
        if (_offset.x > 0f)
            cloneObj.transform.Rotate(0,180,0);
    }
}
