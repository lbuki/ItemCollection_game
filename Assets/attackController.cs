using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackController : MonoBehaviour
{
    [SerializeField]
    private GameObject areaPrefab;
    Vector3 square;
    Vector3 pos;
    GameObject Player;
    private void Start()
    {
        this.Player=GameObject.Find("SD_unitychan_humanoid");
    }
    // Start is called before the first frame update
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameObject area = Instantiate(areaPrefab) as GameObject;
            pos = this.Player.transform.position;
            pos.z += 5;
            area.transform.position = pos;
        }

    }

}
