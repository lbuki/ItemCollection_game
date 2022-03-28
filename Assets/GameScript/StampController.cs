using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampController : MonoBehaviour
{
    [SerializeField]
    GameObject outStamp;
    [SerializeField]
    GameObject clearStamp;
    GameObject player;

    ObjNameList nameList;
    CharaController playerScript;
    GameDirector UImanagaer;
    Vector3 iconVolume;

    [SerializeField]
    float goalScale;
    float scale = 3f;
    float decreaseSpeed = 15f;

    void Awake()
    {
        nameList = GameObject.Find("GameDirector").GetComponent<ObjNameList>();
    }

    void Start()
    {
        UImanagaer = nameList.UImanager;
        playerScript = nameList.playerScript;
        player = nameList.player;
        iconVolume = new Vector3(scale, scale, 1f);
        outStamp.SetActive(false);
        clearStamp.SetActive(false);
        outStamp.transform.localScale = iconVolume;
        clearStamp.transform.localScale = iconVolume;
        if(goalScale == 0)
        {
            goalScale = 0.4f;
        }
    }
    void Update()
    {
       if(UImanagaer.wasCollected == UImanagaer.itemAmount)
        {
            clearStamp.SetActive(true);
            if(scale > goalScale)
            {
                scale -= decreaseSpeed * Time.deltaTime;
                iconVolume = new Vector3(scale, scale, 1f);
                clearStamp.transform.localScale = iconVolume;
            }
            else
            {
                iconVolume = new Vector3(goalScale, goalScale, 1f);
                clearStamp.transform.localScale = iconVolume;
                playerScript.asClear = true;
                UImanagaer.moveTitle();
            }
        }
       if(player.activeSelf == false)
        {
            outStamp.SetActive(true);
            if (scale > goalScale)
            {
                scale -= decreaseSpeed * Time.deltaTime;
                iconVolume = new Vector3(scale, scale, 1f);
                outStamp.transform.localScale = iconVolume;
            }
            else
            {
                iconVolume = new Vector3(goalScale, goalScale, 1f);
                clearStamp.transform.localScale = iconVolume;
                playerScript.asClear = true;
                UImanagaer.moveTitle();
            }
        }
    }
    
}
