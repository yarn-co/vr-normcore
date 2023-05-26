using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class RigidFollower : MonoBehaviour
{
    public Transform target;
    private Rigidbody _rigidbody;

    float amp = 50f;
    float speed = 10.0f;
    private Vector3 desiredVelocity = Vector3.zero;
    private Vector3 distance;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    { 
        if (target != null) {

            distance = target.position - transform.position;

            speed = distance.magnitude * amp;

            desiredVelocity = distance.normalized * speed;
        }
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = desiredVelocity;
    }
}
