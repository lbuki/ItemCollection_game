using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemManager : MonoBehaviour
{
    enemyController e_Controlle;
    charaController c_Controlle;
    GameObject chara;
    float hight;
    float distance;
    Vector3 relDistance;

    void Start()
    {
        this.chara = GameObject.Find("SD_unitychan_humanoid");
        this.hight = this.transform.position.y;
        this.e_Controlle = GameObject.Find("Misaki_win_humanoid").GetComponent<enemyController>();
        this.c_Controlle = GameObject.Find("SD_unitychan_humanoid").GetComponent<charaController>();
    }

    void Update()
    {
        this.relDistance = chara.transform.position - this.transform.position;
        this.distance = Mathf.Pow(Mathf.Abs(relDistance.x),2)
            + Mathf.Pow(Mathf.Abs(relDistance.y), 2)
            + Mathf.Pow(Mathf.Abs(relDistance.z), 2);
        this.transform.Rotate(50f * Time.deltaTime, 50f * Time.deltaTime, 50f * Time.deltaTime);
        this.transform.position =
            new Vector3(this.transform.position.x,
                        hight + Mathf.Sin(Time.time)/10f,
                        this.transform.position.z);
        if(this.distance < 1.3f)
        {
            collected();
        }
    }
    void collected()
    {
        if (e_Controlle.chase == true)//アイテムを拾うと、追いかけられるのが止まる
        {
            e_Controlle.chase = false;
        }
        Destroy(this.gameObject);
        Debug.Log("でりーと");
    }
}
