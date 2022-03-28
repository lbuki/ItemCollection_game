using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    const float Max_y = 3f;
    const float Min_y = -0.25f;

    ObjNameList nameList;

    [System.NonSerialized]
    internal GameObject player;
    GameDirector UImanager;

    Vector3 playerPos;
    Vector3 playerMove;
    Vector3 pivot;

    Quaternion playerRot;

    float defoHight = 1.35f;
    float cameraHight = 0f;

    internal bool tatchable = true;
    bool L_push = false;
    bool R_push = false;
    bool U_push = false;
    bool D_push = false;

    void Awake()
    {
        nameList = GameObject.Find("GameDirector").GetComponent<ObjNameList>();
    }
    void Start()
    {
        UImanager = nameList.UImanager;
        player = nameList.player;
        //pivot.SetParent(charactor);
        playerRot = player.transform.rotation;
        transform.rotation = playerRot;
        transform.Rotate(10f, 0f, 0f);
        transform.position = new Vector3(playerPos.x, playerPos.y + defoHight, playerPos.z - 2f);
        pivot = new Vector3(playerPos.x, playerPos.y + defoHight, playerPos.z);
        playerMove = playerPos;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        test = this.player.transform;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            test.transform.position = new Vector3(0, 0, 0);
        }
        */

        if (Input.GetKeyDown(KeyCode.RightShift))　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　//カメラが変になった時に初期化してほしい
        {
            cameraInit();
        }

        playerPos = player.transform.position;
        playerPos.y += defoHight;
        cameraHight = transform.position.y - defoHight;
        pivot = new Vector3(playerPos.x, playerPos.y + cameraHight, playerPos.z);　　　　　//ピボットの位置の設定
    }
    void LateUpdate()
    {

        this.transform.Translate(player.transform.position - playerMove, Space.World);
        playerMove = player.transform.position;                                          //カメラの移動

        //---------------------------------------------------------------------------------------------------
        //ここから上とここから下の順番を入れ替えて、動きながら視点移動をするとカメラが少しずつズレていきます　←　なんで？？
        switch(UImanager.DeviceType)
        {
            case 0:
                if (Input.GetKey(KeyCode.DownArrow) && this.transform.position.y < player.transform.position.y + Max_y)
                {
                    transform.RotateAround(playerPos, transform.TransformDirection(Vector3.right), 50f * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.UpArrow) && this.transform.position.y > player.transform.position.y + Min_y)
                {
                    transform.RotateAround(playerPos, transform.TransformDirection(Vector3.right), -50f * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    transform.RotateAround(pivot, Vector3.up, 100f * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.RotateAround(pivot, Vector3.up, -100f * Time.deltaTime);
                }　　　　　
                break;
            case 1:
                if (L_push == true)
                {
                    transform.RotateAround(pivot, Vector3.up, -100f * Time.deltaTime);
                    //tatchable = false;
                }
                if (R_push == true)
                {
                    transform.RotateAround(pivot, Vector3.up, 100f * Time.deltaTime);
                    //tatchable = false;
                }
                if (U_push == true && this.transform.position.y > player.transform.position.y + Min_y)
                {
                    transform.RotateAround(playerPos, transform.TransformDirection(Vector3.right), -50f * Time.deltaTime);
                    //tatchable = false;
                }
                if (D_push == true && this.transform.position.y < player.transform.position.y + Max_y)
                {
                    transform.RotateAround(playerPos, transform.TransformDirection(Vector3.right), 50f * Time.deltaTime);
                    //tatchable = false;
                }
                break;
            default:
                break;
        }
        
    }
    internal void cameraInit()
    {
        transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z);
        pivot = new Vector3(playerPos.x, playerPos.y + defoHight, playerPos.z);
        playerRot = player.transform.rotation;
        transform.rotation = playerRot;
        transform.Translate(0, 0, -2, Space.Self);
        transform.Rotate(10f, 0f, 0f);

    }
    internal void cameraSet()
    {
        transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z);
        pivot = new Vector3(playerPos.x, playerPos.y + defoHight, playerPos.z);
        playerRot = player.transform.rotation;
        transform.rotation = playerRot;
        transform.Translate(0f, -0.5f, 1.3f, Space.Self);
        transform.Rotate(5f, 160f, 0f);
    }
    internal void L_buttonPush()
    {
        L_push = true;
    }
    internal void R_buttonPush()
    {
        R_push = true;
    }
    internal void U_buttonPush()
    {
        U_push = true;
    }
    internal void D_buttonPush()
    {
        D_push = true;
    }
    internal void L_buttonUp()
    {
        L_push = false;
    }
    internal void R_buttonUp()
    {
        R_push = false;
    }
    internal void U_buttonUp()
    {
        U_push = false;
    }
    internal void D_buttonUp()
    {
        D_push = false;
    }
}
