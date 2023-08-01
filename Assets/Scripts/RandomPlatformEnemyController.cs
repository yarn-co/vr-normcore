using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;
    public Transform platform; // Assign this in the inspector
    private Vector3 targetPosition;
    private float platformHalfWidth;
    private float platformHalfLength;
    private bool isPaused = false;

    IEnumerator PauseAndSetNewTarget()
    {
        isPaused = true;
        yield return new WaitForSeconds(1);
        SetRandomTarget();
        isPaused = false;
    }

    private void Start()
    {
        // Assume the platform has a BoxCollider
        var platformSize = platform.GetComponent<BoxCollider>().bounds.size;
        platformHalfWidth = platformSize.x / 2;
        platformHalfLength = platformSize.z / 2;

        SetRandomTarget();
    }

    private void Update()
    {
        if (!isPaused)
        {
            // Move towards the target
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            // If the enemy has reached its target, set a new one
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                StartCoroutine(PauseAndSetNewTarget());
            }
        }
    }

    private void SetRandomTarget()
    {
        // Set a random target position within the platform's boundaries
        float randomX = platform.position.x + Random.Range(-platformHalfWidth, platformHalfWidth);
        float randomZ = platform.position.z + Random.Range(-platformHalfLength, platformHalfLength);
        targetPosition = new Vector3(randomX, transform.position.y, randomZ);
    }

    /* Enemy destroy implemented in Player Respawn script.
      
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                // Check if collision is from above
                if (contact.normal.y < -0.5)
                {
                    Destroy(gameObject);  // destroy the enemy
                    break;
                }
            }
        }
    }*/
}
