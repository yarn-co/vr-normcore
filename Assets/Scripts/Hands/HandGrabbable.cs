using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrabbable : MonoBehaviour
{ 
    public bool isGrabbed = false;

    public GameObject grabber;

    public GameObject grabbable;

    public Rigidbody _rigidbody;
   
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbed)
        {
            transform.position = grabber.transform.position;
            transform.rotation = grabber.transform.rotation;
        }
    }

    public void startGrab(GameObject newGrabber)
    {
        isGrabbed = true;
        grabber = newGrabber;

        _rigidbody.isKinematic = true;
    }

    public void endGrab()
    {
        isGrabbed = false;
        grabber = null;

        _rigidbody.isKinematic = false;
    }
}
