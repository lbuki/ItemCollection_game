using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RotatePlane : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed;
    Rigidbody rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(Vector3.up * rotateSpeed * 0.1f));
    }
}
