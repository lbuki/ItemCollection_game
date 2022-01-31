using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    // Start is called before the first frame update
    const float Max_y = 3f;
    const float Min_y = -0.25f;
    [System.NonSerialized]
    public GameObject player;
    Vector3 playerPos;
    Vector3 playerMove;
    Quaternion playerRot;
    [SerializeField, Tooltip("カメラの回転中心にする対象を入れるっちゃ")]
    Transform charactor = null;
    [SerializeField, Tooltip("回転中心本体を入れるんご")]
    Transform pivot = null;
    float defoHight = 1.5f;
    float cameraHight = 0f;
    //Transform test;
    void Start()
    {
        this.player = GameObject.Find("SD_unitychan_humanoid");
        //pivot.SetParent(charactor);
        playerRot = player.transform.rotation;
        transform.rotation = playerRot;
        transform.Rotate(10f, 0f, 0f);
        transform.position = new Vector3(playerPos.x, playerPos.y + defoHight, playerPos.z - 2f);
        pivot.transform.position = new Vector3(playerPos.x, playerPos.y + defoHight, playerPos.z);
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
        pivot.transform.position
            = new Vector3(playerPos.x, playerPos.y + cameraHight, playerPos.z);　　　　　//ピボットの位置の設定
    }
    void LateUpdate()
    {

        this.transform.Translate(player.transform.position - playerMove, Space.World);
        playerMove = player.transform.position;                                          //カメラの移動

        //---------------------------------------------------------------------------------------------------
        //ここから上とここから下の順番を入れ替えて、動きながら視点移動をするとカメラが少しずつズレていきます　←　なんで？？

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
            transform.RotateAround(pivot.transform.position, Vector3.up, 100f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(pivot.transform.position, Vector3.up, -100f * Time.deltaTime);
        }　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　//カメラの位置の設定

    }
    void cameraInit()
    {
        transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z);
        pivot.transform.position = new Vector3(playerPos.x, playerPos.y + defoHight, playerPos.z);
        playerRot = player.transform.rotation;
        transform.rotation = playerRot;
        transform.Translate(0, 0, -2, Space.Self);
        transform.Rotate(10f, 0f, 0f);

    }
}
