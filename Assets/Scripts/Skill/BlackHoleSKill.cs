using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSKill : Skill
{
    [SerializeField] int amountOfAttacks;
    [SerializeField] float cloneCooldowm;
    [Space]
    [SerializeField] GameObject blackHoleObj;
    [SerializeField] float maxSize;
    [SerializeField] float growSpeed;
    [SerializeField] float shrinkSpeed;


    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }
    public override void UseSkill()
    {
        base.UseSkill();
        GameObject newBlackHole = Instantiate(blackHoleObj, player.transform.position, 
            Quaternion.identity);
        newBlackHole.GetComponent<BlackHoleSkillController>().SetupBlackHole(maxSize,
            growSpeed, shrinkSpeed, amountOfAttacks, cloneCooldowm);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
