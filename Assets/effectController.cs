using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectController : MonoBehaviour
{
    public void playEffect(Vector3 playPos)
    {
        this.transform.position = playPos;
        GetComponent<ParticleSystem>().Play();
    }
}
