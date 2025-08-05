using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlackHoleHotKeyController : MonoBehaviour
{
    SpriteRenderer sr;
    KeyCode myHotKey;
    TextMeshProUGUI myText;

    Transform myEnemy;
    BlackHoleSkillController blackHole;

    public void SetupHotKey(KeyCode _myHotKey, Transform _myEnemy, 
        BlackHoleSkillController _myBlackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy;
        blackHole = _myBlackHole;

        myHotKey = _myHotKey;
        myText.text = myHotKey.ToString();
    }
    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            blackHole.AddEnemyToList(myEnemy);
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
