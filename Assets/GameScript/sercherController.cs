using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sercherController : MonoBehaviour
{
    Rigidbody C_rigid;
    Vector3 initPlace;
    Vector3 T_Vector;
    Vector3 self;
    GameObject enemy;
    GameObject target;
    enemyController e_Controlle;
    GameObject UImanager;
    int type = 1;
    float degree;
    float distance;
    float totalWalkSpeed;
    float walkSpeed = 20f;
    float speedScale = 1f;
    bool accident = false;
    bool finish = true;
    bool patrol;
    const float figureArea = 5f;//見つかる範囲の距離(値は座標での距離)
    const float maxWalkSpeed = 8f;

    void Start()
    {
        this.UImanager = GameObject.Find("GameDirector");
        this.degree = Random.Range(45.0f, 315.0f);
        this.self = transform.rotation.eulerAngles;//自身の方向を360°で取得
        this.target = GameObject.Find("SD_unitychan_humanoid");
        this.enemy = GameObject.Find("Misaki_win_humanoid");
        this.C_rigid = GetComponent<Rigidbody>();
        this.distance = (this.transform.position - target.transform.position).sqrMagnitude;//3平方の2乗部分(負荷軽減の為、√する前の値)
        e_Controlle = this.enemy.GetComponent<enemyController>();//ファイル名が名前の変数を作ってみた
        initPlace = new Vector3(60f, 1.5f, 15f);
        this.transform.position = initPlace;
        patrol = true;
        if (C_rigid.useGravity == true)
        {
            C_rigid.useGravity = false;
        }
    }
    void Update()//patroleがtrueの時こっち, falseの時はFixedUpdate
    {
        if (patrol == true && e_Controlle.chase == false)
        {
            switch (type)
            {
                case 0:
                    //定点巡回プログラム
                    if(accident == true)
                    {
                        transform.rotation = Quaternion.RotateTowards
                            (this.transform.rotation,
                            Quaternion.Euler(this.self.x, this.self.y - 180f, this.self.z),
                            100f * Time.deltaTime);//振り向き実装
                        StartCoroutine(switchAccident());
                    }
                    else
                    {
                        transform.Translate(0f, 0f, 2.3f * Time.deltaTime);
                    }
                    break;
                case 1:
                    //不規則巡回
                    if (accident == true)
                    {
                        transform.rotation = Quaternion.RotateTowards
                            (this.transform.rotation,
                            Quaternion.Euler(this.self.x, this.self.y - degree, this.self.z),
                            100f * Time.deltaTime);//振り向き実装
                        StartCoroutine(switchAccident());
                    }
                    else
                    {
                        transform.Translate(0f, 0f, 2.3f * Time.deltaTime);
                    }
                    break;
                case 2:
                    //未定
                    break;
                default:
                    Debug.Log("異常あり");
                    break;
            }
        }
        this.distance = (this.transform.position - target.transform.position).sqrMagnitude;
        if (distance < Mathf.Pow(figureArea,2))//3平方なので、figureArea^2
        {
            patrol = false;
        }
        else if(patrol == false)
        {
            patrol = true;
        }
        if (e_Controlle.chase == true)
        {
            this.transform.position = initPlace;
        }
        if(e_Controlle.chase == true)
        {
            C_rigid.isKinematic = true;
        }
        else
        {
            C_rigid.isKinematic = false;
        }
    }
    void FixedUpdate()
    {
        UImanager.GetComponent<gameDirector>().attachGravity(this.C_rigid);
        if (e_Controlle.chase == false && patrol == false)
        {
            T_Vector = target.transform.position - this.transform.position;
            Quaternion direction = Quaternion.LookRotation(T_Vector);//targetの向きを向く
            direction.x = 0f;
            direction.z = 0f;
            this.transform.rotation = direction;
            totalWalkSpeed = Mathf.Abs(this.C_rigid.velocity.x) + Mathf.Abs(this.C_rigid.velocity.z);
            if (totalWalkSpeed < maxWalkSpeed && e_Controlle.chase == false)
            {
                this.C_rigid.AddForce(this.transform.forward * walkSpeed * speedScale);
                //Debug.Log("加速中");
            }
            else
            {
                //this.C_rigid.AddForce(this.transform.forward * walkSpeed * 0.8f);
            }
            C_rigid.velocity *= 0.95f;
        }
    }
    void OnTriggerEnter(Collider objName)
    {
        if(objName.gameObject.name == "SD_unitychan_humanoid")
        {
            e_Controlle.chase = true;
            patrol = false;
        }
        accident = true;
        //Debug.Log("衝突判定"+objName.gameObject.name);
    }
    void OnCollisionEnter(Collision objName)
    {
        if (objName.gameObject.name != "SD_unitychan_humanoid")
        {
            //accident = true;
        }
    }
    void OnTriggerStay(Collider objName)
    {
        if (objName.gameObject.name != "SD_unitychan_humanoid" && this.finish == true)
        {
            accident = true;
        }
    }
    IEnumerator switchAccident()
    {
        this.finish = false;
        yield return new WaitForSeconds(1.3f);
        this.self = transform.rotation.eulerAngles;//自身の方向を360°で取得
        this.degree = Random.Range(45.0f, 315.0f);//次の振り向き角度を設定
        accident = false;
        yield return new WaitForSeconds(0.5f);
        this.finish = true;
    }
}
