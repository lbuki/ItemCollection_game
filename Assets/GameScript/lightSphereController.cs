using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSphereController : MonoBehaviour
{
    [SerializeField]
    GameObject searcher = null;
    float areaHight = 2.1f;
    // Start is called before the first frame update
    

    void Update()
    {
        this.transform.position =
            new Vector3(searcher.transform.position.x,
            searcher.transform.position.y + areaHight + (Mathf.Sin(Time.time) / 8f),
            searcher.transform.position.z);
    }
}
