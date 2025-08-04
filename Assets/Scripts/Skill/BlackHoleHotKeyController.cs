using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlackHoleHotKeyController : MonoBehaviour
{
    public InputAction killAction;
    public TextMeshProUGUI textTip;
    private void Awake()
    {
        // 随机选键（可扩展更多键）
        Key randomKey = RandomKey();

        //Update UI
        textTip.text = randomKey.ToString();

        // 创建并绑定
        killAction = new InputAction(name: "Kill_" + gameObject.GetInstanceID(),
                                     binding: "<Keyboard>/" + randomKey.ToString());
        killAction.performed += KillAction_performed;
        killAction.Enable();
    }

    void KillAction_performed(InputAction.CallbackContext obj)
    {
        //召唤分身 执行秒杀逻辑
    }
    Key RandomKey()
    {
        // 常用字母，可自由扩展
        Key[] keys = { Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G, 
            Key.H, Key.J, Key.K, Key.L, Key.Q, Key.W, Key.E, Key.R, Key.T, 
            Key.Y, Key.U, Key.I, Key.O, Key.P };
        return keys[Random.Range(0, keys.Length)];
    }
}
