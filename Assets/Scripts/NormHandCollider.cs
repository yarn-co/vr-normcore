using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NormHandCollider : MonoBehaviour
{
    [Serializable]
    public class CollisionEvent : UnityEvent<GameObject> { }
    public CollisionEvent onHandCollision = new CollisionEvent();

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("NormHandCollider: " + other);

        if (other.tag == "Hand")
        {
            Debug.Log("Hand in box");

            if (onHandCollision != null) onHandCollision.Invoke(gameObject);
        }
    }
}
