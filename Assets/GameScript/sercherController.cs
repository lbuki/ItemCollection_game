using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SercherController : MonoBehaviour
{
    struct PosRot
    {
        internal Vector3 pos;
        internal Quaternion rot;
    }
    PosRot initInfo;

    [SerializeField]
    AudioClip[] clips;//[0]=チリンチリン, [1]=シャンシャン
    AudioSource source;

    Rigidbody rigid;
    
    Vector3 T_Vector;
    Vector3 self;

    GameObject target;

    ObjNameList nameList;
    EnemyController enemyScript;
    GameDirector UImanager;

    [SerializeField,Tooltip("0で定点巡回, 1で不規則に巡回, 2で定点監視")]
    int type;
    int soundCount = 0;

    float degree;
    float distance;
    float totalWalkSpeed;
    float walkSpeed = 20f;
    float speedScale = 1f;

    const float figureArea = 5f;//見つかる範囲の距離(値は座標での距離)
    const float maxWalkSpeed = 8f;
    const float chaseSpeed = 2.2f;

    bool accident = false;
    bool finish = true;
    bool patrol;//trueで巡回, falseで追いかける

    
    void Awake()
    {
        nameList = GameObject.Find("GameDirector").GetComponent<ObjNameList>();
    }
    void Start()
    {
        degree = Random.Range(45.0f, 315.0f);
        self = transform.rotation.eulerAngles;//自身の方向を360°で取得
        target = nameList.player;
        rigid = GetComponent<Rigidbody>();
        distance = (transform.position - target.transform.position).sqrMagnitude;//3平方の2乗部分(負荷軽減の為、√する前の値)
        enemyScript = nameList.enemyScript;
        UImanager = nameList.UImanager;

        initInfo.pos = transform.position;
        initInfo.rot = transform.rotation;
        source = GetComponent<AudioSource>();
        transform.position = initInfo.pos;
        patrol = true;
        if (rigid.useGravity == true)
        {
            rigid.useGravity = false;
        }
        if(type == 0 || type == 1 || type == 2)
        {
            
        }
        else
        {
            type = Random.Range(0, 3);
        }
    }
    void Update()//patroleがtrueの時こっち, falseの時はFixedUpdate
    {
        //Debug.Log(transform.name + "の初期座標:" + initInfo.pos);
        if (target != null)
        {
            if (patrol == true && enemyScript.chase == false)
            {
                switch (type)
                {
                    case 0:
                        //定点巡回プログラム
                        if (accident == true)
                        {
                            transform.rotation = Quaternion.RotateTowards
                                (transform.rotation,
                                Quaternion.Euler(self.x, self.y - 180f, self.z),
                                100f * Time.deltaTime);//振り向き実装
                            StartCoroutine(switchAccident());
                        }
                        else
                        {
                            transform.Translate(0f, 0f, chaseSpeed * Time.deltaTime);
                        }
                        break;
                    case 1:
                        //不規則巡回
                        if (accident == true)
                        {
                            transform.rotation = Quaternion.RotateTowards
                                (transform.rotation,
                                Quaternion.Euler(self.x, self.y - degree, self.z),
                                100f * Time.deltaTime);//振り向き実装
                            StartCoroutine(switchAccident());
                        }
                        else
                        {
                            transform.Translate(0f, 0f, chaseSpeed * Time.deltaTime);
                        }
                        break;
                    case 2://近づいたら追跡, 離れると元の位置に戻る

                        break;
                    default:
                        Debug.Log("異常あり");
                        break;
                }
            }
            distance = (transform.position - target.transform.position).sqrMagnitude;
            if (distance < Mathf.Pow(figureArea, 2))//3平方なので、figureArea^2
            {
                patrol = false;
            }
            else if (patrol == false)
            {
                patrol = true;
                initPlace();
            }
            if (enemyScript.chase == true)
            {
                transform.position = initInfo.pos;
            }
            if (enemyScript.chase == true)
            {
                rigid.isKinematic = true;
            }
            else
            {
                rigid.isKinematic = false;
            }
            if (patrol == true)
            {
                soundCount = 0;
            }
            else
            {
                if (soundCount == 0)//追いかけ始めた時にチリンチリン鳴らす
                {
                    source.PlayOneShot(clips[Random.Range(0, clips.Length)], 1f);
                    soundCount += 1;
                    Debug.Log("soundCount = " + soundCount);
                }
            }
        }
    }
    void FixedUpdate()
    {
        UImanager.attachGravity(rigid);
        if (enemyScript.chase == false && patrol == false)
        {
            T_Vector = target.transform.position - transform.position;
            Quaternion direction = Quaternion.LookRotation(T_Vector);//targetの向きを向く
            direction.x = 0f;
            direction.z = 0f;
            transform.rotation = direction;
            totalWalkSpeed = Mathf.Abs(rigid.velocity.x) + Mathf.Abs(rigid.velocity.z);
            if (totalWalkSpeed < maxWalkSpeed && enemyScript.chase == false)
            {
                rigid.AddForce(transform.forward * walkSpeed * speedScale);
                //Debug.Log("加速中");
            }
            else
            {
                //this.C_rigid.AddForce(this.transform.forward * walkSpeed * 0.8f);
            }
            rigid.velocity *= 0.95f;
        }
    }
    void OnTriggerEnter(Collider objName)
    {
        if(objName.gameObject.name == "SD_unitychan_humanoid")
        {
            enemyScript.chase = true;
            patrol = false;
        }
        if (objName.gameObject.tag != "item" && objName.gameObject.tag != "enemy" && finish == true)
        {
            accident = true;
        }
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
        if (objName.gameObject.tag != "item" && objName.gameObject.tag != "enemy" && finish == true)
        {
            accident = true;
        }
    }
    void initPlace()
    {
        transform.position = initInfo.pos;
        transform.rotation = initInfo.rot;
    }
    IEnumerator switchAccident()
    {
        finish = false;
        yield return new WaitForSeconds(2.0f);
        self = transform.rotation.eulerAngles;//自身の方向を360°で取得
        degree = Random.Range(45.0f, 315.0f);//次の振り向き角度を設定
        accident = false;
        yield return new WaitForSeconds(0.5f);
        finish = true;
    }
}
