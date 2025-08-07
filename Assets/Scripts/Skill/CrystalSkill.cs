using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] GameObject crystalPrefab;
    [SerializeField] float crystalDuration;

    [Header("Explore Info")]
    [SerializeField] bool canExplore;
    [Header("Move Info")]
    [SerializeField] bool canMove;
    [SerializeField] float moveSpeed;
    [Header("Multi Stacking Info")]
    [SerializeField] bool canUseMultiStacks;
    [SerializeField] int amountOfStacks;
    [SerializeField] float multiStackCool;
    [SerializeField] float useTimeWindow;
    [SerializeField] List<GameObject> crystaclLeft = new List<GameObject>();

    GameObject currentCrystal;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
        {
            return;
        }

        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab,
                Player.Instance.transform.position, Quaternion.identity);
            CrystalSkillController currentCrystalScript = currentCrystal.GetComponent<CrystalSkillController>();
            currentCrystalScript.SetupCrystal(crystalDuration, canExplore, canMove, 
                    moveSpeed, FindClosestEnemy(currentCrystal.transform));
        
        }
        else
        {
            if(canMove) return;
            Vector2 playerPos = Player.Instance.transform.position;

            //ª•ªªŒª÷√
            Player.Instance.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            currentCrystal.GetComponent<CrystalSkillController>()?.FinishCrystal();
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    bool CanUseMultiCrystal()
    {
        if(canUseMultiStacks)
        {
            if(crystaclLeft.Count > 0)
            {
                if (crystaclLeft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWindow);

                coolDown = 0;

                GameObject crystalToSpawn = crystaclLeft[crystaclLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, Player.Instance.transform.position, Quaternion.identity);
            
                crystaclLeft.Remove(crystalToSpawn);
                newCrystal.GetComponent<CrystalSkillController>().SetupCrystal(
                    crystalDuration,canExplore,canMove,moveSpeed,
                    FindClosestEnemy(newCrystal.transform));
                if (crystaclLeft.Count <= 0)
                {
                    //cooldown 
                    coolDown = multiStackCool;
                    RefilCrystacl();
                }
                return true;
            }
        }
        return false;
    }
    void RefilCrystacl()
    {
        int amountToAdd = amountOfStacks - crystaclLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystaclLeft.Add(crystalPrefab);
        }
    }
    void ResetAbility()
    {
        if(coolDownTimer > 0) return;
        coolDownTimer = multiStackCool;
        RefilCrystacl();
    }
}
