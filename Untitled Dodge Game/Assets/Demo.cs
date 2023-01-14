using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Demo : MonoBehaviour
{
    private void Start()
    {
        
    }

    void ChangeParameters()
    {
        transform.eulerAngles = new Vector3(
            transform.localRotation.eulerAngles.x,
            100,
            transform.eulerAngles.z
        );
    }
    public void ResetParameters()
    {
        Debug.Log("Change");
        Debug.Log(transform.eulerAngles);
        transform.position = new Vector3(0, -1.126f, 0);
        ChangeParameters();
        Debug.Log(transform.eulerAngles + " after");

    }
}
