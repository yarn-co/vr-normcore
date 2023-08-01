using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Level2ArrivalScript : MonoBehaviour
{
    private bool playerHasArrived = false;
    public TMP_Text arrivalText;


    private void OnCollisionEnter(Collision collision)
    {
        if (!playerHasArrived && collision.gameObject.CompareTag("Player")) // Assuming the player has a tag "Player"
        {
            playerHasArrived = true;
            StartCoroutine(DisplayArrivalText());
        }
    }

    IEnumerator DisplayArrivalText()
    {
        arrivalText.text = "<color=red>Congratulations!</color> Welcome to Level 2!";
        yield return new WaitForSeconds(10.0f);
        arrivalText.text = "";
    }
}