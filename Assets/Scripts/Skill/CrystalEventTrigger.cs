using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalEventTrigger : MonoBehaviour
{
    [SerializeField] CrystalSkillController ckc;
    void ExploreFinish()
    {
        ckc.AnimationExplodeEvent();
    }
    void AnimationFinish()
    { 
        ckc.SelfDestroy();
    }
}
