using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DeleteChara : MonoBehaviour
{
    Rigidbody rigid;
    [SerializeField]
    Vector3 gravity = new Vector3(0, -10, 0);
    float time = 0;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        if (rigid.useGravity == true)
        {
            rigid.useGravity = false;
        }
    }
    void Update()
    {
        time += Time.deltaTime;
        if (time > 10f)//1秒経過でオブジェクト消去
        {
            time = 0;
            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        rigid.AddForce(gravity, ForceMode.Acceleration);
    }
}
