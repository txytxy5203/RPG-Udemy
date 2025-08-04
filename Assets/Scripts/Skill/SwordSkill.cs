using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.InputSystem;

public enum E_SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}
public class SwordSkill : Skill
{
    public E_SwordType swordType = E_SwordType.Regular;

    [Header("Spine Info")]
    [SerializeField] float maxSpineDis;
    [SerializeField] float maxSpineTime;
    [SerializeField] float attackIntervalTime;

    [Header("Pierce Info")]
    [SerializeField] int maxPierceNumber;
    [SerializeField] float pierceGravity;

    [Header("Bounce Info")]
    [SerializeField] int maxAttackNumber;
    [SerializeField] float speedOfBounce;
    [SerializeField] float attackRadius;


    [Header("Skill Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private float swordGravity;
    [SerializeField] private float swordSpeed;

    [Header("Aim dots")]
    [SerializeField] private int numberofDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        dots = new GameObject[numberofDots];
    }
    public void CreateSword()
    {
        //先不改 后续看看手感再想想要不要加上launchDir
        Vector2 launchDir = GetAimInput();

        GameObject swordObj = Instantiate(swordPrefab);

        swordObj.transform.position = player.transform.position;
        switch (swordType)
        {
            case E_SwordType.Regular:
                swordObj.GetComponent<SwordSkillController>().
                    SetupSwordBase(launchDir * swordSpeed, swordGravity, swordType);
                break;

            case E_SwordType.Bounce:
                swordObj.GetComponent<SwordSkillController>().
                    SetupSwordBase(launchDir * swordSpeed, swordGravity, swordType);

                swordObj.GetComponent<SwordSkillController>().
                    InitBounce(attackRadius, maxAttackNumber, speedOfBounce);
                break;

            case E_SwordType.Pierce:
                swordObj.GetComponent<SwordSkillController>().
                    SetupSwordBase(launchDir * swordSpeed, pierceGravity, swordType);
                swordObj.GetComponent<SwordSkillController>().
                    InitPierce(maxPierceNumber);
                break;

            case E_SwordType.Spin:
                swordObj.GetComponent<SwordSkillController>().
                    SetupSwordBase(launchDir * swordSpeed, pierceGravity, swordType);
                swordObj.GetComponent<SwordSkillController>().
                    InitSpin(maxSpineDis, 1f, maxSpineTime, attackIntervalTime);
                break;
            default:
                break;
        }
        player.currentSword = swordObj;
    }
    public void AimVisual()
    {
        ClearAimVisualObj();

        float gravity = 0;
        switch (swordType)
        {
            case E_SwordType.Regular:
            case E_SwordType.Bounce:
                gravity = swordGravity;
                break;
            case E_SwordType.Pierce:
            case E_SwordType.Spin:
                gravity = pierceGravity;
                break;         
            default:
                break;
        }

        Vector2 launchDir = GetAimInput();
        //这里从 1 开始
        for (int i = 1; i <= numberofDots; i++)
        {
            //注意这里的斜抛公式 还有重力加速度
            Vector3 aimPos = new Vector3(launchDir.x * swordSpeed * spaceBetweenDots * i,
                            launchDir.y * swordSpeed * spaceBetweenDots * i
                            + gravity * Physics2D.gravity.y * Mathf.Pow(spaceBetweenDots * i, 2) / 2f);
            GameObject aimObj = Instantiate(dotPrefab, aimPos + player.transform.position, Quaternion.identity);
            dots[i - 1] = aimObj;
        }
    }
    public void ClearAimVisualObj()
    {
        foreach (var obj in dots)
        {
            Destroy(obj);
        }
    }

    public Vector2 GetAimInput()
    {
        Debug.Log(Player.Instance.playerInput.actions["AimStick"].ReadValue<Vector2>());
        if (Player.Instance.playerInput.actions["AimStick"].ReadValue<Vector2>().x != 0f)
        {
            Vector2 stick = Player.Instance.playerInput.actions["AimStick"].ReadValue<Vector2>();
            return stick.normalized;
        }
        // 2. 鼠标屏幕坐标
        return (Camera.main.ScreenToWorldPoint(
        Player.Instance.playerInput.actions["AimMouse"].ReadValue<Vector2>())
                        - player.transform.position).normalized;
    }
}
