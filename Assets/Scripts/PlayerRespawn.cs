using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Transform xrRig;

    // Set the xrRig
    public void SetXRRig(Transform xrRig)
    {
        this.xrRig = xrRig;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided");
        // Checks if the object it collided with has the "Death" tag
        if (other.gameObject.CompareTag("Death"))
        {
            Debug.Log("Did I die?");
            if (this.xrRig != null)
            {
                this.xrRig.position = RespawnPoint.Instance.transform.position;
                Debug.Log("Current location:" + this.xrRig.position);
                Debug.Log("Respawn location:" + RespawnPoint.Instance.transform.position);
            }
            else
            {
                Debug.LogWarning("XR Rig not set in PlayerRespawn script. Can't respawn player.");
            }
        }
    }
}
