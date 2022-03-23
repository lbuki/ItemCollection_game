using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemManager : MonoBehaviour
{
    EnemyController enemyScript;
    CharaController playerScript;
    GameDirector UImanager;
    ObjNameList nameList;

    GameObject player;
    Vector3 relDistance;
    float hight;
    float distance;

    void Awake()
    {
        nameList = GameObject.Find("GameDirector").GetComponent<ObjNameList>();
    }
    void Start()
    {
        player = nameList.player;
        hight = this.transform.position.y;
        enemyScript = nameList.enemyScript;
        playerScript = nameList.playerScript;
        UImanager = nameList.UImanager;
    }

    void Update()
    {
        relDistance = player.transform.position - this.transform.position;
        distance = Mathf.Pow(Mathf.Abs(relDistance.x), 2)
                                + Mathf.Pow(Mathf.Abs(relDistance.y), 2)
                                + Mathf.Pow(Mathf.Abs(relDistance.z), 2);
        transform.Rotate(50f * Time.deltaTime,
                         50f * Time.deltaTime,
                         50f * Time.deltaTime);
        transform.position =
            new Vector3(transform.position.x,
                        hight + Mathf.Sin(Time.time)/10f,
                        transform.position.z);
        if(this.distance < 1.3f)
        {
            collected();
        }
    }
    void collected()
    {
        if (enemyScript.chase == true)//アイテムを拾うと、追いかけられるのが止まる
        {
            enemyScript.chase = false;
        }
        if (UImanager.wasCollected < UImanager.itemAmount)
        {
            UImanager.wasCollected += 1;
        }
        UImanager.SE_collect();
        Destroy(this.gameObject);
    }
}
