using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrabber : MonoBehaviour
{
    public XRHand _hand;

    public GameObject grabbing;
    public HandGrabbable grabbable;

    public bool isGrabbing = false;

    private void Awake()
    {
        _hand = GetComponentInParent<XRHand>();
        _hand.onGripEnd += OnGripEnd;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGripEnd()
    {
        if (isGrabbing)
        {
            Debug.Log("Releasing Grab " + grabbing);
            Debug.Log("_hand " + _hand.handType);

            grabbable.endGrab();
            
            grabbing = null;
            grabbable = null;

            isGrabbing = false; 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isGrabbing && _hand != null && _hand.isGripping)
        {
            Debug.Log("Trying to grab " + other);

            other.TryGetComponent<HandGrabbable>(out grabbable);

            if (grabbable != null)
            {
                Debug.Log("Can Grab " + other);

                isGrabbing = true;
                grabbing = other.gameObject;
                grabbable.startGrab(_hand.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered " + other);

        if (other.tag == "ScoreZone")
        {


        }
    }
}
