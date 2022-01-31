using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    GameObject target;
    GameObject attack;
    GameObject UImanager;
    Rigidbody E_rigid;
    Vector3 T_Vector;
    [System.NonSerialized]
    public Animator E_animator;
    bool nearBy = false;
    bool attackCheck = false;
    bool finished = true;
    [System.NonSerialized]
    public bool chase = false;
    float distance;
    float totalWalkspeed;
    float walkSpeed　= 15f;//加える力
    float speedScale = 1f;//後で速さを変更できるように倍率を設定
    const float maxWalkSpeed = 5.0f;
    const float figureArea = 1f;//止まる範囲の距離(値は座標での距離)
    void Start()
    {
        this.UImanager = GameObject.Find("GameDirector");
        this.attack = GameObject.Find("areaGenerator");
        this.E_rigid = GetComponent<Rigidbody>();
        this.target = GameObject.Find("SD_unitychan_humanoid");
        this.distance = this.distance = (this.transform.position - target.transform.position).sqrMagnitude;
        E_animator = GetComponent<Animator>();
        E_animator.SetBool("is_running", false);
        E_animator.SetBool("attack", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (chase == true　&& nearBy == false)
        {
            E_animator.SetBool("is_running", true);
        }
        else
        {
            E_animator.SetBool("is_running", false);
        }
        if(attackCheck == true && chase == true)
        {
            E_animator.SetBool("attack", true);
            StartCoroutine(attackFunc());
            attackCheck = false;
        }
        this.distance = (this.transform.position - target.transform.position).sqrMagnitude;
        if (distance < Mathf.Pow(figureArea, 2))//3平方なので、figureArea^2
        {
            this.E_rigid.velocity = new Vector3(0.5f * this.E_rigid.velocity.x, this.E_rigid.velocity.y, 0.5f * this.E_rigid.velocity.z);
            if(chase == true && finished == true)
            {
                attackCheck = true;
            }
            else if(chase == true)
            {
                nearBy = true;
                E_animator.SetBool("is_running", false);
                //speedScale = 0f;
            }
        }
        else if (chase == true)
        {
                nearBy = false;
                E_animator.SetBool("is_running", true);
        }
    }
    void FixedUpdate()
    {
        UImanager.GetComponent<gameDirector>().attachGravity(this.E_rigid);
        if (chase == true)
        {
            T_Vector = target.transform.position - this.transform.position;
            Quaternion direction = Quaternion.LookRotation(T_Vector);//targetの向きを向く
            direction.x = 0f;
            direction.z = 0f;
            this.transform.rotation = direction;
            totalWalkspeed = Mathf.Abs(this.E_rigid.velocity.x) + Mathf.Abs(this.E_rigid.velocity.z);
            if (finished == true && nearBy == false && totalWalkspeed < maxWalkSpeed)
            {
                this.E_rigid.AddForce(transform.forward * walkSpeed * speedScale);
            }
            else
            {
                //this.E_rigid.velocity　= new Vector3(0.8f * this.E_rigid.velocity.x, this.E_rigid.velocity.y, 0.8f * this.E_rigid.velocity.z);
            }
            //Debug.Log("あたっく"+attackCheck);
        }
        else
        {
            E_animator.SetBool("is_running", false);
            E_animator.SetBool("attack", false);
        }
    }
    IEnumerator attackFunc()
    {
        finished = false;
        yield return new WaitForSeconds(0.6f);
        attack.GetComponent<attackController>().generateAttackArea();
        yield return new WaitForSeconds(1);
        E_animator.SetBool("attack", false);
        finished = true;
    }
}
