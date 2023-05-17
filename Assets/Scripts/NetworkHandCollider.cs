using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NetworkHandCollider : MonoBehaviour
{
    [Serializable]
    public class CollisionEvent : UnityEvent<GameObject> { }
    public CollisionEvent onHandCollision = new CollisionEvent();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hand")
        {
            Debug.Log("Hand in box");

            //var shuffleboard = Game.Instance.GetComponentInParent<NetworkShuffleboard>();

            //shuffleboard.MakePuck(team);

            if (onHandCollision != null) onHandCollision.Invoke(gameObject);
        }
    }
}
