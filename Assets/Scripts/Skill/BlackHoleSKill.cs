using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSKill : Skill
{
    [SerializeField] int amountOfAttacks;
    [SerializeField] float cloneCooldowm;
    [SerializeField] float blackHoleDuration;
    [Space]
    [SerializeField] GameObject blackHoleObj;
    [SerializeField] float maxSize;
    [SerializeField] float growSpeed;
    [SerializeField] float shrinkSpeed;

    BlackHoleSkillController currentBlackHole;
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }
    public override void UseSkill()
    {
        base.UseSkill();
        GameObject newBlackHole = Instantiate(blackHoleObj, player.transform.position, 
            Quaternion.identity);
        currentBlackHole = newBlackHole.GetComponent<BlackHoleSkillController>();
        currentBlackHole.SetupBlackHole(maxSize,
            growSpeed, shrinkSpeed, amountOfAttacks, cloneCooldowm, blackHoleDuration);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
    public bool SkillCompleted()
    {
        if(!currentBlackHole) return false;
        if (currentBlackHole.playerCanExitState)
        {
            currentBlackHole = null;
            return true;
        }
        return false;
    }
}
