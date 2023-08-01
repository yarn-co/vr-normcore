using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCollisionHandler : MonoBehaviour
{
    public Transform respawnPointType1; // Drag the respawn point for Type 1 in the editor
    public Transform respawnPointType2; // Drag the respawn point for Type 2 in the editor

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Type1"))
        {
            // Respawn at the location of respawnPointType1
            collision.gameObject.transform.position = respawnPointType1.position;
        }
        else if (collision.gameObject.CompareTag("Type2"))
        {
            // Respawn at the location of respawnPointType2
            collision.gameObject.transform.position = respawnPointType2.position;
        }
    }
}
