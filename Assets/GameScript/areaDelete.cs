using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDelete : MonoBehaviour
{
    float time = 0;
    void Update()
    {
        //Debug.Log(time);
        time += Time.deltaTime;
        if(time > 1f)//1秒経過でオブジェクト消去
        {
            time = 0;
            Destroy(gameObject);
        }
    }
}
