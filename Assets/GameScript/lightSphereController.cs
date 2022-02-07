using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightSphereController : MonoBehaviour
{
    GameObject searcher;
    float areaHight = 2.1f;
    // Start is called before the first frame update
    void Start()
    {
        searcher = GameObject.Find("searchArea");
    }

    void Update()
    {
        this.transform.position =
            new Vector3(searcher.transform.position.x,
            searcher.transform.position.y + areaHight + (Mathf.Sin(Time.time) / 8f),
            searcher.transform.position.z);
        Debug.Log("searcherPos"+searcher.transform.position);
    }
}
