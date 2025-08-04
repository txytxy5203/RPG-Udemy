using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSKill : Skill
{
    public GameObject blackHoleObj;
    public void CreatBlackHole()
    {
        Instantiate(blackHoleObj, player.transform.position, Quaternion.identity);
    }
}
