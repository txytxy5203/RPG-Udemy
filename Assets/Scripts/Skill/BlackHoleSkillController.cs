using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkillController : MonoBehaviour
{
    public float maxSize;
    public float growSpeed;
    public bool canGrow;
    public List<Transform> targets;
    public GameObject uiObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, 
                Vector2.one * maxSize, Time.deltaTime * growSpeed);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            //当黑洞碰到Enemy后 执行的逻辑
            collision.GetComponent<Enemy>().FreezeTime(true);
            GameObject killUIObj = Instantiate(uiObj, 
                collision.transform.position, Quaternion.identity); 
            killUIObj.transform.parent = collision.transform;
        }    
    }
}
