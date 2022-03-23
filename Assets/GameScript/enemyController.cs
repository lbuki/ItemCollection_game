using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyController : MonoBehaviour
{
    [SerializeField]
    AudioClip[] clips;//[0]=笑い声1, [1]=笑い声2
    AudioSource source;
    Rigidbody rigid;
    internal Animator animator;

    AttackController attackScript;
    ObjNameList nameList;
    GameDirector UImanager;

    GameObject target;
    Vector3 T_Vector;

    [System.NonSerialized]
    bool nearBy = false;
    [System.NonSerialized]
    public bool chase = false;
    bool attackCheck = false;
    bool finished = true;

    float countTime = 0;
    float distance;
    float totalWalkspeed;
    float walkSpeed　= 15f;//加える力
    float speedScale = 1f;//後で速さを変更できるように倍率を設定

    const float maxWalkSpeed = 5.0f;
    const float figureArea = 1f;//止まる範囲の距離(値は座標での距離)

    void Awake()
    {
        nameList = GameObject.Find("GameDirector").GetComponent<ObjNameList>();
    }
    void Start()
    {
        UImanager = nameList.UImanager;
        attackScript = nameList.attackScript;
        rigid = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        animator.SetBool("is_running", false);
        animator.SetBool("attack", false);

        target = nameList.player;
        distance = this.distance = (this.transform.position - target.transform.position).sqrMagnitude;
    }

    // Update is called once per frame
    void Update()
    {
        if (chase == true　&& nearBy == false)
        {
            animator.SetBool("is_running", true);
        }
        else
        {
            animator.SetBool("is_running", false);
        }
        if(attackCheck == true && chase == true)
        {
            animator.SetBool("attack", true);
            StartCoroutine(attackFunc());
            attackCheck = false;
        }
        distance = (this.transform.position - target.transform.position).sqrMagnitude;
        if (distance < Mathf.Pow(figureArea, 2))//3平方なので、figureArea^2
        {
            this.rigid.velocity =
                new Vector3(0.5f * rigid.velocity.x,
                            this.rigid.velocity.y,
                            0.5f * rigid.velocity.z);
            if(chase == true && finished == true)
            {
                attackCheck = true;
            }
            else if(chase == true)
            {
                nearBy = true;
                animator.SetBool("is_running", false);
                //speedScale = 0f;
            }
        }
        else if (chase == true)
        {
                nearBy = false;
                animator.SetBool("is_running", true);
        }
        if(chase == true)
        {
            voiceOfEnemy();
        }
    }
    void FixedUpdate()
    {
        UImanager.attachGravity(this.rigid);
        if (chase == true)
        {
            T_Vector = target.transform.position - this.transform.position;
            Quaternion direction = Quaternion.LookRotation(T_Vector);//targetの向きを向く
            direction.x = 0f;
            direction.z = 0f;
            this.transform.rotation = direction;
            totalWalkspeed = Mathf.Abs(rigid.velocity.x) + Mathf.Abs(rigid.velocity.z);
            if (finished == true && nearBy == false && totalWalkspeed < maxWalkSpeed)
            {
                rigid.AddForce(transform.forward * walkSpeed * speedScale);
            }
            else
            {
                //this.E_rigid.velocity　= new Vector3(0.8f * this.E_rigid.velocity.x, this.E_rigid.velocity.y, 0.8f * this.E_rigid.velocity.z);
            }
            //Debug.Log("あたっく"+attackCheck);
        }
        else
        {
            animator.SetBool("is_running", false);
            animator.SetBool("attack", false);
        }
    }
    void voiceOfEnemy()
    {
        countTime += Time.deltaTime;
        if(countTime > Random.Range(3, 10))
        {
            countTime = 0;
            source.PlayOneShot(clips[Random.Range(0, clips.Length)], Random.Range(0.1f, 0.7f));
        }
    }
    IEnumerator attackFunc()
    {
        finished = false;
        yield return new WaitForSeconds(0.6f);
        attackScript.generateAttackArea();
        yield return new WaitForSeconds(1);
        animator.SetBool("attack", false);
        finished = true;
    }
}
