using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour 
{
    public bool Run = false;
    void Update()
    {
            Destroy(gameObject);
    }
}
