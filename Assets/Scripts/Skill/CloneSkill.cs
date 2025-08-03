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
    public void CreateClone(Transform trans)
    {
        //ʵ������ʱ��λ�þ����ú�
        GameObject cloneObj = Instantiate(clonePrefab);
        cloneObj.GetComponent<CloneSkillController>().SetupClone(trans, 
            cloneDuration, canAttack);
        
    }
}
