using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class charaController : MonoBehaviour
{
    GameObject camera;//デストロイ用
    GameObject effect;//デストロイ用
    GameObject enemy;//デストロイ用
    GameObject UImanager;
    Vector3 cameraRot;
    Vector3 dropPoint;
    Ray ray;
    Rigidbody rigid;
    float walkSpeed;
    float jumpForce = 7f;
    float speedx;
    float speedz;
    float speedy;
    float TotalWalkspeed;
    float speedScale;
    float dropTime;
    [System.NonSerialized]
    public float stamina;
    bool asDash = true;
    bool jumpAble = true;

    const string areaName = "attackAreaPrefab(Clone)";
    const float maxWalkSpeed = 5;
    const float addSpeed = 100f;//aaaaaaaaaaaaaaaaa
    const float jumpTime = 0.3f;
    private Animator animator;
    // Use this for initialization
    void Start()
    {
        this.stamina = 100f;
        this.UImanager = GameObject.Find("GameDirector");
        this.camera = GameObject.Find("Main Camera");
        this.enemy = GameObject.Find("Misaki_win_humanoid");
        animator = GetComponent<Animator>();
        this.rigid = GetComponent<Rigidbody>();
        walkSpeed = 0;
    }

    void Update() //毎フレーム実行される(FPS毎)
    {
        speedx = Mathf.Abs(this.rigid.velocity.x);
        speedz = Mathf.Abs(this.rigid.velocity.z);
        speedy = Mathf.Abs(this.rigid.velocity.y);
        TotalWalkspeed = speedx + speedz;
        cameraRot = camera.transform.rotation.eulerAngles;  //カメラの角度（オイラー角）
        cameraRot.x = 0;
        cameraRot.z = 0;                                    //キャラにx,z軸の変更は不要
        Debug.Log("スタミナ残量:"+stamina);
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))           //↗︎
        {
            this.transform.rotation = Quaternion.Euler(cameraRot);
            this.transform.Rotate(0, 45f, 0);
            walkSpeed = addSpeed;
            animator.SetBool("isWalking", true);
        }
        else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))  //↘︎
        {
            this.transform.rotation = Quaternion.Euler(cameraRot);
            this.transform.Rotate(0, 135f, 0);
            walkSpeed = addSpeed;
            animator.SetBool("isWalking", true);
        }
        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))  //↙︎
        {
            this.transform.rotation = Quaternion.Euler(cameraRot);
            this.transform.Rotate(0, 225f, 0);
            walkSpeed = addSpeed;
            animator.SetBool("isWalking", true);
        }
        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))  //↖︎
        {
            this.transform.rotation = Quaternion.Euler(cameraRot);
            this.transform.Rotate(0, 315f, 0);
            walkSpeed = addSpeed;
            animator.SetBool("isWalking", true);
        }
        else if (Input.GetKey(KeyCode.W))                             //↑
        {
            this.transform.rotation = Quaternion.Euler(cameraRot);
            walkSpeed = addSpeed;
            animator.SetBool("isWalking", true);
        }
        else if (Input.GetKey(KeyCode.S))                              //↓
        {
            this.transform.rotation = Quaternion.Euler(cameraRot);
            this.transform.Rotate(0, 180f, 0);
            walkSpeed = addSpeed;
            animator.SetBool("isWalking", true);
        }
        else if (Input.GetKey(KeyCode.A))                               //←
        {
            this.transform.rotation = Quaternion.Euler(cameraRot);
            walkSpeed = addSpeed;
            this.transform.Rotate(0, 270f, 0);
            animator.SetBool("isWalking", true);
        }
        else if (Input.GetKey(KeyCode.D))                              //→
        {
            this.transform.rotation = Quaternion.Euler(cameraRot);
            this.transform.Rotate(0, 90f, 0);
            walkSpeed = addSpeed;
            animator.SetBool("isWalking", true);
        }
        else
        {
            walkSpeed *= 0.95f;
            animator.SetBool("isWalking", false);
        }                                             //キャラ移動コード

        if (Input.GetKey(KeyCode.R) && asDash == true)
        {
            animator.SetBool("isDash", true);
            speedScale = 2.0f;

            if(stamina > 0)//スタミナシステム
            {
                stamina -= 30f * Time.deltaTime;
            }
            else
            {
                asDash = false;
                StartCoroutine(healStamina());
            }
        }
        else if(asDash == true)
        {
            if (stamina >= 100f)
            {
                stamina = 100f;
            }
            else
            {
                stamina += 15f * Time.deltaTime;
            }
            speedScale = 1.0f;
        }
        else
        {
            animator.SetBool("isDash", false);
            speedScale = 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpAble == true)//ジャンプ関係 地面と接触していないと飛べない
        {
            StartCoroutine(jumpFunc());
            //jumpForce = 0f;
            jumpAble = false;
            //Debug.Log("tonda");
        }
        else if (jumpForce <= 270f)                       //連続で飛ぶと高く飛べない
        {
            //jumpForce += 1f;
        }

        if(jumpAble == false && this.rigid.velocity.y <= 0)//空中の時
        {
            ray = new Ray(this.transform.position, -transform.up);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 5f))
            {
                this.dropPoint = hit.point;
            }
            //運動方程式から地面に落ちる時刻を計測 h=0.5gt^2 → t^2 - 2h/g = 0を満たすまでやる
            dropTime = 2 * (this.dropPoint.y - this.transform.position.y)
                /(UImanager.GetComponent<gameDirector>().gravityPower.y);// 2h/gの値
            if(Mathf.Pow(jumpTime,2) - dropTime > 0)
            {
                animator.SetBool("top", true);
            }
            //Debug.Log("drop:"+ (Mathf.Pow(jumpTime, 2) - dropTime));
            Debug.Log("h:" + (this.dropPoint.y - this.transform.position.y));
        }
    }
    void FixedUpdate() //一定時間で呼び出される(等間隔)
    {
        UImanager.GetComponent<gameDirector>().attachGravity(this.rigid);
        if (TotalWalkspeed < maxWalkSpeed)
        {
            this.rigid.AddForce(transform.forward * this.walkSpeed * speedScale);
        }
    }
    void OnTriggerEnter(Collider objName)
    {
        if(objName.gameObject.name != "attackAreaPrefab(Clone)")
        {
            //animator.SetBool("isJumping", false);
        }
        
    }
    void OnCollisionEnter(Collision objName)
    {
        StartCoroutine(jumpInterval());
        animator.SetBool("isJumping", false);
        if (objName.gameObject.name == "Field")
        {
            animator.SetBool("top", false);
            //Debug.Log("field");
        }
        else
        {
            //Debug.Log("air");
            animator.SetBool("isJumping", false);
            //jumpAble = false;
        }
        if (objName.gameObject.name == areaName)//ゲームオーバーでキャラを消す
        {
            deletePlayer();
            Debug.Log("effectOn!");
        }

        animator.SetBool("top", false);
    }
    void OnCollisionStay(Collision objName)
    {
        animator.SetBool("top", false);
    }
    IEnumerator healStamina()
    {
        while(stamina < 100)
        {
            stamina += 8f * Time.deltaTime;
            if(stamina > 100f)
            {
                stamina = 100f;
                asDash = true;
            }
            yield return null;
        }
    }
    IEnumerator jumpInterval()
    {
        yield return new WaitForSeconds(0.5f);
        jumpAble = true;
    }
    IEnumerator jumpFunc()
    {
        animator.SetBool("isJumping", true);
        yield return new WaitForSeconds(0.1f);
        this.rigid.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    void deletePlayer()
    {
        Vector3 playPos = this.transform.position;
        playPos.y += 1.0f;
        UImanager.GetComponent<gameDirector>().playEffect(playPos);
        UImanager.GetComponent<gameDirector>().hideGauge();
        this.gameObject.SetActive(false);
        this.enemy.GetComponent<enemyController>().chase = false;
        this.enemy.GetComponent<enemyController>().E_animator.SetBool("attack", true);
        this.enemy.GetComponent<enemyController>().E_animator.SetBool("is_running", false);
    }
}
