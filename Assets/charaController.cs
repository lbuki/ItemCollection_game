using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class charaController : MonoBehaviour
{
    GameObject camera;//デストロイ用
    GameObject effect;//デストロイ用
    GameObject enemy;//デストロイ用
    GameObject gravity;
    Vector3 cameraRot;
    Rigidbody rigid;
    float walkSpeed;
    float jumpForce = 7f;
    float speedx;
    float speedz;
    float speedy;
    float TotalWalkspeed;
    float speedScale;
    bool jumpAble = true;

    const string areaName = "attackAreaPrefab(Clone)";
    const float maxWalkSpeed = 5;
    const float addSpeed = 100f;//
    private Animator animator;
    // Use this for initialization
    void Start()
    {
        this.gravity = GameObject.Find("G-force");
        this.effect = GameObject.Find("effect");
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

        if (Input.GetKey(KeyCode.R))
        {
            animator.SetBool("isDash", true);
            speedScale = 2.0f;
        }
        else
        {
            animator.SetBool("isDash", false);
            speedScale = 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpAble == true)//ジャンプ関係 地面と接触していないと飛べない
        {
            animator.SetBool("isJumping", true);
            this.rigid.AddForce(transform.up * jumpForce ,ForceMode.Impulse);
            //jumpForce = 0f;
            jumpAble = false;
            //Debug.Log("tonda");
        }
        else if (jumpForce <= 270f)                       //連続で飛ぶと高く飛べない
        {
            //jumpForce += 1f;
        }
        
        //Debug.Log("velo = "+this.rigid.velocity);
        //Debug.Log(jumpAble);
        //Debug.Log(speedScale);
    }
    void FixedUpdate() //一定時間で呼び出される(等間隔)
    {
        gravity.GetComponent<worldGravitySetting>().attachGravity(this.rigid);
        if (TotalWalkspeed < maxWalkSpeed)
        {
            this.rigid.AddForce(transform.forward * this.walkSpeed * speedScale);
        }
    }
    void OnTriggerEnter(Collider objName)
    {
        animator.SetBool("isJumping", false);
        
    }
    void OnCollisionEnter(Collision collision)
    {
        jumpAble = true;
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Field")
        {
            
            //Debug.Log("field");
        }
        else
        {
            //Debug.Log("air");
            animator.SetBool("isJumping", false);
            //jumpAble = false;
        }
        if (collision.gameObject.name == areaName)//ゲームオーバーでキャラを消す
        {
            deletePlayer();
            Debug.Log("effectOn!");
        }

    }
    private void OnTriggerStay(Collider objName)
    {
        
    }
    void deletePlayer()
    {
        Vector3 playPos = this.transform.position;
        playPos.y += 1.0f;
        effect.GetComponent<effectController>().playEffect(playPos);
        Destroy(this);//このオブジェクトのコンポーネントを消す
        Destroy(camera.GetComponent<cameraController>());//メインカメラのスクリプトを消す
        Destroy(this.gameObject);//ユニティちゃんを消す
        this.enemy.GetComponent<enemyController>().E_animator.SetBool("attack", true);
        this.enemy.GetComponent<enemyController>().E_animator.SetBool("is_running", false);
        Destroy(this.enemy.GetComponent<enemyController>());
    }
}
