using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.RotateTowards
                            (this.transform.rotation,
                            Quaternion.Euler(transform.rotation.x, this.transform.rotation.y - 180f, transform.rotation.z),
                            100f * Time.deltaTime);
    }
}
