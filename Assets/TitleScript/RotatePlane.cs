using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RotatePlane : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed = 10f;
    Rigidbody rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (ButtonManager.DeviceType == 0)
        {
            rotateSpeed = 7f;
        }
        else if (ButtonManager.DeviceType == 1)
        {
            rotateSpeed = -7f;
        }
    }
    void FixedUpdate()
    {
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(Vector3.up * rotateSpeed * 0.1f));
    }
}
