using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    private Material originalMat;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }
    public void Hit()
    {
        StartCoroutine(FlashFX(.2f));
    }
    IEnumerator FlashFX(float value)
    {
        sr.material = hitMat;
        yield return new WaitForSeconds(value);
        sr.material = originalMat;
    }
    private void RedColorBlink()
    {
        Debug.Log("enter falsh");
        if (sr.color != Color.white)
        { 
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }
    private void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }
}
