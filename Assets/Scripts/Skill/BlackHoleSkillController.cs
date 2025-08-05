using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    public float maxSize;
    public float growSpeed;
    public float shrinkSpeed;

    public bool canGrow = true;
    public bool canShrink;
    bool canCreateHotKeys = true;
    bool cloneAttackReleased;

    int amountOfAttacks = 4;
    float cloneAttackCooldown = .3f;
    float cloneAttackTimer;

    List<Transform> targets = new List<Transform>();
    List<GameObject> createdHotKey = new List<GameObject>();

    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed,
        int _amountOfAttacks, float _cloneAttackCooldown)
    {
        maxSize = _maxSize; 
        growSpeed = _growSpeed; 
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;

    }
    void Update()
    {
        cloneAttackTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale,
                Vector2.one * maxSize, Time.deltaTime * growSpeed);
        }
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale,
                new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        DestoryHotKeys();
        cloneAttackReleased = true;
        canCreateHotKeys = false;
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0f && cloneAttackReleased)
        {
            int randomIndex = Random.Range(0, targets.Count);
            cloneAttackTimer = cloneAttackCooldown;

            float xOffset;
            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;

            SkillManager.instance.clone.CreateClone(targets[randomIndex], Vector3.right * xOffset);

            amountOfAttacks--;
            if (amountOfAttacks <= 0)
            { 
                Player.Instance.ExitBlackHoleAbility();
                canShrink = true;
                cloneAttackReleased = false;
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {

            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        { 
            collision.GetComponent<Enemy>().FreezeTime(false);
        }
    }
    private void CreateHotKey(Collider2D collision)
    {
        if(keyCodeList.Count <= 0)
        {
            return;
        }
        if (!canCreateHotKeys)
            return;


        int r = Random.Range(0, keyCodeList.Count);
        KeyCode choosenKey = keyCodeList[r];
        keyCodeList.Remove(choosenKey);

        //当黑洞碰到Enemy后 执行的逻辑
        GameObject newHotKey = Instantiate(hotKeyPrefab,
            collision.transform.position + Vector3.up * 2, Quaternion.identity);
        createdHotKey.Add(newHotKey);
        newHotKey.GetComponent<BlackHoleHotKeyController>().SetupHotKey(choosenKey, collision.transform, this);
    }
    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);    

    void DestoryHotKeys()
    {
        if(createdHotKey.Count <= 0)
            return;
        for(int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }
}
