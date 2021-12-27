using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    GameObject target;
    Rigidbody E_rigid;
    Vector3 T_Vector;
    bool attackCheck = false;
    float totalWalkspeed;
    float walkSpeed　= 50f;//加える力
    float speedScale = 1f;//後で速さを変更できるように倍率を設定
    const float maxWalkSpeed = 10f;
    void Start()
    {
        this.E_rigid = GetComponent<Rigidbody>();
        this.target = GameObject.Find("SD_unitychan_humanoid");
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    void FixedUpdate()
    {
        T_Vector = target.transform.position - this.transform.position;
        Quaternion direction = Quaternion.LookRotation(T_Vector);//targetの向きを向く
        direction.x = 0f;
        direction.z = 0f;
        this.transform.rotation = direction;
        totalWalkspeed = Mathf.Abs(this.E_rigid.velocity.x) + Mathf.Abs(this.E_rigid.velocity.z);
        if (attackCheck == false && totalWalkspeed < maxWalkSpeed)
        {
            this.E_rigid.AddForce(transform.forward * this.walkSpeed * speedScale);
        }
        else
        {
            speedScale = 0;
        }

        speedScale = 1f;
        Debug.Log("あたっく"+attackCheck);
    }
    void OnTriggerStay(Collider objName)
    {
        if(objName.gameObject.name == "SD_unitychan_humanoid")
        {
            attackCheck = true;
            speedScale = 0;
        }
    }
    void OnTriggerExit(Collider objName)
    {
        if (objName.gameObject.name == "SD_unitychan_humanoid")
        {
            attackCheck = false;
        }
    }
}
