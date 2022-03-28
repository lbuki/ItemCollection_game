using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharaController : MonoBehaviour
{
    GameObject mainCamera;

    CameraController cameraScript;
    ObjNameList nameList;
    GameDirector UImanager;
    EnemyController enemyScript;

    Vector3 cameraRot;
    Vector3 dropPoint;
    Vector2 startFinger;
    Vector2 swipeFinger;

    Ray ray;

    Rigidbody rigid;

    float walkSpeed;
    float jumpForce = 7f;
    float speedx;
    float speedz;
    float speedy;
    float totalWalkspeed;
    float speedScale;
    float dropTime;
    float degreeDirection;
    float swipeDistance;
    [System.NonSerialized]
    public float stamina;

    bool asDash = true;
    bool jumpAble = true;
    bool touching = false;
    bool R_touching = false;
    bool isMoving = false;
    bool playOnce = false;
    internal bool asClear = false;

    string areaName;
    const float maxWalkSpeed = 5;
    const float addSpeed = 40f;//aaaaaaaaaaaaaaaaa
    const float jumpTime = 0.15f;
    private Animator animator;
    // Use this for initialization
    void Awake()
    {
        nameList = GameObject.Find("GameDirector").GetComponent<ObjNameList>();
    }
    void Start()
    {
        mainCamera = nameList.mainCamera;
        UImanager = nameList.UImanager;
        cameraScript = nameList.cameraScript;
        enemyScript = nameList.enemyScript;
        stamina = 100f;
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        areaName = nameList.areaName;
        
        walkSpeed = 0;
    }

    void Update() //毎フレーム実行される(FPS毎)
    {
        speedx = Mathf.Abs(rigid.velocity.x);
        speedz = Mathf.Abs(rigid.velocity.z);
        speedy = Mathf.Abs(rigid.velocity.y);
        totalWalkspeed = speedx + speedz;
        cameraRot = mainCamera.transform.rotation.eulerAngles;  //カメラの角度（オイラー角）
        cameraRot.x = 0;
        cameraRot.z = 0;
        if(asClear == false)
        {
            switch (UImanager.DeviceType)
            {
                case 0:
                    if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))           //↗︎
                    {
                        transform.rotation = Quaternion.Euler(cameraRot);
                        transform.Rotate(0, 45f, 0);
                        walkSpeed = addSpeed;
                        animator.SetBool("isWalking", true);
                    }
                    else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))  //↘︎
                    {
                        transform.rotation = Quaternion.Euler(cameraRot);
                        transform.Rotate(0, 135f, 0);
                        walkSpeed = addSpeed;
                        animator.SetBool("isWalking", true);
                        isMoving = true;
                    }
                    else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))  //↙︎
                    {
                        transform.rotation = Quaternion.Euler(cameraRot);
                        transform.Rotate(0, 225f, 0);
                        walkSpeed = addSpeed;
                        animator.SetBool("isWalking", true);
                        isMoving = true;
                    }
                    else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))  //↖︎
                    {
                        transform.rotation = Quaternion.Euler(cameraRot);
                        transform.Rotate(0, 315f, 0);
                        walkSpeed = addSpeed;
                        animator.SetBool("isWalking", true);
                        isMoving = true;
                    }
                    else if (Input.GetKey(KeyCode.W))                             //↑
                    {
                        transform.rotation = Quaternion.Euler(cameraRot);
                        walkSpeed = addSpeed;
                        animator.SetBool("isWalking", true);
                        isMoving = true;
                    }
                    else if (Input.GetKey(KeyCode.S))                              //↓
                    {
                        transform.rotation = Quaternion.Euler(cameraRot);
                        transform.Rotate(0, 180f, 0);
                        walkSpeed = addSpeed;
                        animator.SetBool("isWalking", true);
                        isMoving = true;
                    }
                    else if (Input.GetKey(KeyCode.A))                               //←
                    {
                        transform.rotation = Quaternion.Euler(cameraRot);
                        walkSpeed = addSpeed;
                        transform.Rotate(0, 270f, 0);
                        animator.SetBool("isWalking", true);
                        isMoving = true;
                    }
                    else if (Input.GetKey(KeyCode.D))                              //→
                    {
                        transform.rotation = Quaternion.Euler(cameraRot);
                        transform.Rotate(0, 90f, 0);
                        walkSpeed = addSpeed;
                        animator.SetBool("isWalking", true);
                        isMoving = true;
                    }
                    else
                    {
                        walkSpeed *= 0f;
                        isMoving = false;
                        animator.SetBool("isWalking", false);
                    }
                    //キャラ移動コード
                    break;



                case 1:
                    if (Input.GetMouseButtonDown(0) == true)
                    {
                        startFinger = Input.mousePosition;
                    }
                    if (Input.GetMouseButton(0) == true && touching == true)
                    {
                        if (R_touching == false)
                        {
                            swipeFinger = Input.mousePosition;
                            swipeFinger -= startFinger;
                            swipeDistance = Mathf.Pow(swipeFinger.x, 2) + Mathf.Pow(swipeFinger.y, 2);
                            getAngle(swipeFinger);
                        }

                        if (swipeDistance > 3000f)
                        {
                            transform.rotation = Quaternion.Euler(cameraRot);
                            transform.Rotate(0, degreeDirection, 0);
                            walkSpeed = addSpeed;
                            animator.SetBool("isWalking", true);
                            isMoving = true;
                        }

                    }
                    else
                    {
                        walkSpeed *= 0f;
                        isMoving = false;
                        animator.SetBool("isWalking", false);
                        swipeDistance = 0;
                    }
                    break;

                default:
                    break;
            }
            switch (UImanager.DeviceType)
            {
                case 0:
                    if (Input.GetKey(KeyCode.R) && asDash == true)
                    {
                        if (stamina > 0)//スタミナシステム
                        {
                            if (isMoving == true)
                            {
                                stamina -= 20f * Time.deltaTime;
                                animator.SetBool("isDash", true);
                                speedScale = 3.0f;
                            }
                            else
                            {
                                stamina += 15f * Time.deltaTime;
                            }
                        }
                        else
                        {
                            asDash = false;
                            animator.SetBool("isDash", false);
                            StartCoroutine(healStamina());
                        }
                    }
                    else if (asDash == true)
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
                        animator.SetBool("isDash", false);
                    }
                    else
                    {
                        animator.SetBool("isDash", false);
                        speedScale = 1.0f;
                    }
                    //ジャンプシステムの一部
                    if (Input.GetKeyDown(KeyCode.Space) && jumpAble == true)//ジャンプ関係 地面と接触していないと飛べない
                    {
                        StartCoroutine(jumpFunc());
                        //jumpForce = 0f;
                        jumpAble = false;
                        //Debug.Log("tonda");
                    }
                    break;

                case 1:
                    if (swipeDistance > 20000f && asDash == true)
                    {
                        if (stamina > 0)//スタミナシステム
                        {
                            if (isMoving == true)
                            {
                                stamina -= 20f * Time.deltaTime;
                                animator.SetBool("isDash", true);
                                speedScale = 3.0f;
                            }
                        }
                        else
                        {
                            asDash = false;
                            animator.SetBool("isDash", false);
                            StartCoroutine(healStamina());
                        }
                    }
                    else if (asDash == true)
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
                        animator.SetBool("isDash", false);
                    }
                    else
                    {
                        animator.SetBool("isDash", false);
                        speedScale = 1.0f;
                    }
                    break;

                default:
                    break;

            }


            if(jumpAble == false)
            {
                Debug.Log("速さ: "+rigid.velocity.y);
            }
            if (jumpAble == false &&rigid.velocity.y < -0.05f )//空中の時
            {
                if(playOnce == false)
                {
                    ray = new Ray(transform.position, -transform.up);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 5f))
                    {
                        dropPoint = hit.point;
                    }
                    dropTime = Mathf.Sqrt(2 * (dropPoint.y - transform.position.y) / (UImanager.gravityPower.y));
                    StartCoroutine(landing(dropTime - jumpTime));
                    Debug.Log((dropTime - jumpTime)+"秒後に着地");
                    playOnce = true;
                }
                
            }
        }
        else//ゲームクリアした時
        {
            walkSpeed = 0;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            animator.SetBool("isWalking", false);
            animator.SetBool("isDash",false);
            animator.SetBool("isJumping", false);
            animator.SetBool("top", true);
            cameraScript.cameraSet();
            animator.SetBool("winPose",true);
            if(playOnce == false)
            {
                UImanager.playVoice();
                playOnce = true;
            }
        }
    }
    void FixedUpdate() //一定時間で呼び出される(等間隔)
    {
        if(UImanager.DeviceType == 0)
        {
            rigid.velocity = new Vector3(rigid.velocity.x * 0.8f, rigid.velocity.y, rigid.velocity.z * 0.8f);
        }
        else if(UImanager.DeviceType == 1)
        {
            rigid.velocity = new Vector3(rigid.velocity.x * 0.7f, rigid.velocity.y, rigid.velocity.z * 0.7f);
        }
        UImanager.attachGravity(rigid);
        if (totalWalkspeed < maxWalkSpeed)
        {
            rigid.AddForce(transform.forward * walkSpeed * speedScale);
        }
    }
    void OnTriggerEnter(Collider objName)
    {
        if(objName.gameObject.name != areaName)
        {
            //animator.SetBool("isJumping", false);
        }
        if (objName.gameObject.name == areaName)//ゲームオーバーでキャラを消す
        {
            deletePlayer();
            Debug.Log("effectOn!");
        }
    }
    void OnTriggerStay(Collider objName)
    {
        animator.SetBool("top", true);
    }
    void OnCollisionEnter(Collision objName)
    {
        if (gameObject.activeSelf == true)
        {
            StartCoroutine(jumpInterval());
        }
        animator.SetBool("isJumping", false);
        if (objName.gameObject.name == "Field")
        {
            animator.SetBool("top", true);
            //Debug.Log("field");
        }
        else
        {
            //Debug.Log("air");
            animator.SetBool("isJumping", false);
            //jumpAble = false;
        }

        playOnce = false;
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
            stamina += 15f * Time.deltaTime;
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
        rigid.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    IEnumerator landing(float time)
    {
        if(time < 0)
        {
            time = Mathf.Abs(time);
        }
        yield return new WaitForSeconds(time);
        animator.SetBool("top",true);
    }
    IEnumerator touchInterval()
    {
        yield return null;
        R_touching = false;
    }
    void getAngle(Vector2 direction)
    {
        degreeDirection = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        if(degreeDirection < 0)
        {
            degreeDirection += 360;
        }
    }
    void deletePlayer()
    {
        Vector3 playPos = transform.position;
        playPos.y += 1.0f;
        UImanager.playEffect(playPos);
        UImanager.hideSwitch = true;
        gameObject.SetActive(false);
        enemyScript.chase = false;
        enemyScript.animator.SetBool("attack", true);
        enemyScript.animator.SetBool("is_running", false);
    }
    internal void J_button()
    {
        if (UImanager.DeviceType == 1 &&jumpAble == true)//ジャンプ関係 地面と接触していないと飛べない
        {
            StartCoroutine(jumpFunc());
            //jumpForce = 0f;
            jumpAble = false;
            //Debug.Log("tonda");
        }
    }
    internal void Touching()
    {
        touching = true;
    }
    internal void nonTouch()
    {
        touching = false;
    }
    internal void R_Touching()
    {
        R_touching = true;
    }
    internal void R_nonTouch()
    {
        StartCoroutine(touchInterval());
    }
}
