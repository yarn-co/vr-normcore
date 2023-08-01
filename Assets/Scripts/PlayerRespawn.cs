using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private float bounceForce = 500.0f;
    [SerializeField] private TMP_Text bounceText;
    public Transform RespawnPoint;
    public List<int> collectedNumbers = new List<int>();
    private int enemyNumber;

    /*private Transform xrRig;

    // Set the xrRig
    public void SetXRRig(Transform xrRig)
    {
        this.xrRig = xrRig;
        Debug.Log("XRRig is as follows:" + this.xrRig);
    }*/

    IEnumerator DisplayBounceText()
    {
        bounceText.text = "Bonk! You've obtained a number: " + enemyNumber.ToString();
        yield return new WaitForSeconds(2.0f);
        bounceText.text = "";
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");
        // Checks if the object it collided with has the "Death" tag
        if (collision.gameObject.CompareTag("Death"))
        {
            Respawn();
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Enemy.");
            Vector3 contactPoint = collision.contacts[0].point;
            float allowedOffset = 0.3f;
            Debug.Log("ContactPoint.y: " + contactPoint.y);
            Debug.Log("Player y position: " + this.transform.position.y);
            Debug.Log("Allowed Offset: " + allowedOffset);
            if (contactPoint.y < this.transform.position.y + allowedOffset)
            {
                Destroy(collision.gameObject);  // destroy the enemy
                enemyNumber = Random.Range(1, 10);
                collectedNumbers.Add(enemyNumber);

                Rigidbody playerRb = GetComponent<Rigidbody>();
                if (playerRb != null)
                {
                    playerRb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
                }

                // Display a UI feedback
                StartCoroutine(DisplayBounceText());
                return;
            }
            else
            {
                Respawn();
                Debug.Log("RESPAWNED: CONTACT AT" + contactPoint.y + "PLAYER AT" + this.transform.position.y + "ALLOWED OFFSET" + allowedOffset);
                return;
            }
        }
    }

    void Respawn()
    {
        Debug.Log("Did I die?");
        transform.position = RespawnPoint.position;
        Debug.Log("Current location:" + transform.position);
        Debug.Log("Respawn location:" + RespawnPoint.transform.position);
    }
}