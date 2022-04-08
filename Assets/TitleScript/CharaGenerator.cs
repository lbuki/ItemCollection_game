using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject[] chara = new GameObject[6];
    Vector3 pos;
    Vector3 rot;
    float time = 1;

    void Update()
    {
        time += Time.deltaTime;
        if (time > 0.5f)//1秒経過でオブジェクト消去
        {
            time = 0;
            GameObject charactor = Instantiate(chara[Random.Range(0,chara.Length)]) as GameObject;
            pos = new Vector3(Random.Range(-4, 4), Random.Range(3, 5), Random.Range(-4, 4));
            rot = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            charactor.transform.rotation = Quaternion.Euler(rot);
            charactor.transform.position = pos;
        }
    }
}
