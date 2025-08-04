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
        // ���ѡ��������չ�������
        Key randomKey = RandomKey();

        //Update UI
        textTip.text = randomKey.ToString();

        // ��������
        killAction = new InputAction(name: "Kill_" + gameObject.GetInstanceID(),
                                     binding: "<Keyboard>/" + randomKey.ToString());
        killAction.performed += KillAction_performed;
        killAction.Enable();
    }

    void KillAction_performed(InputAction.CallbackContext obj)
    {
        //�ٻ����� ִ����ɱ�߼�
    }
    Key RandomKey()
    {
        // ������ĸ����������չ
        Key[] keys = { Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G, 
            Key.H, Key.J, Key.K, Key.L, Key.Q, Key.W, Key.E, Key.R, Key.T, 
            Key.Y, Key.U, Key.I, Key.O, Key.P };
        return keys[Random.Range(0, keys.Length)];
    }
}
