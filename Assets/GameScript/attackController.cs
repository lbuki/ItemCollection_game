using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackController : MonoBehaviour
{
    [SerializeField]
    private GameObject areaPrefab;//アウトレット接続
    Vector3 pos;
    GameObject Player;
    const string name = "Misaki_win_humanoid";
    private void Start()
    {
        this.Player=GameObject.Find(name);
    }
    // Start is called before the first frame update
    
    public void generateAttackArea()
    {
            GameObject square = Instantiate(areaPrefab) as GameObject;
            pos = this.Player.transform.position;
            square.transform.position = pos;
    }
}
