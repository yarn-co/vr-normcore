using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSlowly : MonoBehaviour
{

    public Vector3 torque;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddTorque(torque);
    }

    // Update is called once per frame
    void Update()
    {
       //transform.Rotate(new Vector3(0f, 0.03f, 0.02f));
    }
}
