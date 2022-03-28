using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField]
    GameObject areaPrefab;//アウトレット接続
    GameObject player;

    Vector3 pos;

    ObjNameList nameList;

    void Awake()
    {
        nameList = GameObject.Find("GameDirector").GetComponent<ObjNameList>();
    }
    private void Start()
    {
        player = nameList.enemy;
    }
    // Start is called before the first frame update

    internal void generateAttackArea()
    {
        GameObject square = Instantiate(areaPrefab) as GameObject;
        pos = this.player.transform.position;
        square.transform.position = pos;
    }
}
