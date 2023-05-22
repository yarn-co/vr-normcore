using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPuckCollider : MonoBehaviour
{
    
    private NetworkPuck NetworkPuck;
    
    // Start is called before the first frame update
    void Start()
    {
        NetworkPuck = gameObject.GetComponentInParent<NetworkPuck>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered " + other.tag);

        if (other.CompareTag("ScoreZone"))
        {

            var scoreZone = other.GetComponent<ScoreZone>();

            if (scoreZone.side != NetworkPuck.Side)
            {

                //Debug.Log("Triggered " + other.tag + " " + scoreZone.points);

                NetworkPuck.text.text = scoreZone.points.ToString();
            }
        }
    }
}
