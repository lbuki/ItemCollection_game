using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    GameObject target;
    GameObject attack;
    Rigidbody E_rigid;
    Vector3 T_Vector;
    bool nearBy = false;
    bool attackCheck = false;
    float totalWalkspeed;
    float walkSpeed　= 10f;//加える力
    float speedScale = 1f;//後で速さを変更できるように倍率を設定
    const float maxWalkSpeed = 2.5f;
    void Start()
    {
        
        this.attack = GameObject.Find("areaGenerator");
        this.E_rigid = GetComponent<Rigidbody>();
        this.target = GameObject.Find("SD_unitychan_humanoid");
    }

    // Update is called once per frame
    void Update()
    {
        if(attackCheck == true)
        {
            StartCoroutine(attackFunc());
            attackCheck = false;
        }
    }
    void FixedUpdate()
    {
        T_Vector = target.transform.position - this.transform.position;
        Quaternion direction = Quaternion.LookRotation(T_Vector);//targetの向きを向く
        direction.x = 0f;
        direction.z = 0f;
        this.transform.rotation = direction;
        totalWalkspeed = Mathf.Abs(this.E_rigid.velocity.x) + Mathf.Abs(this.E_rigid.velocity.z);
        if (nearBy == false && totalWalkspeed < maxWalkSpeed)
        {
            this.E_rigid.AddForce(transform.forward * walkSpeed * speedScale);
        }
        else
        {
            //this.E_rigid.velocity　= new Vector3(0.8f * this.E_rigid.velocity.x, this.E_rigid.velocity.y, 0.8f * this.E_rigid.velocity.z);
        }

        Debug.Log("あたっく"+attackCheck);
    }
    void OnTriggerEnter(Collider objName)
    {
        if(objName.gameObject.name == "SD_unitychan_humanoid")
        {
            attackCheck = true;
            nearBy = true;
            //speedScale = 0f;
        }
    }
    void OnTriggerStay(Collider objName)
    {
        if (objName.gameObject.name == "SD_unitychan_humanoid")
        {
            nearBy = true;
            this.E_rigid.velocity = new Vector3(0.5f * this.E_rigid.velocity.x, this.E_rigid.velocity.y, 0.5f * this.E_rigid.velocity.z);
        }
    }
    void OnTriggerExit(Collider objName)
    {
        if (objName.gameObject.name == "SD_unitychan_humanoid")
        {
            nearBy = false;
            //attackCheck = false;
            //speedScale = 1.0f;
        }
    }
    IEnumerator attackFunc()
    {
        Debug.Log("yonda");
        yield return new WaitForSeconds(1);
        attack.GetComponent<attackController>().generateAttackArea();
        yield return new WaitForSeconds(1);
        
    }
}
