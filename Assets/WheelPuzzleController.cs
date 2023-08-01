using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<StoneWheel> stones = new List<StoneWheel>();  // Assign all your stone wheels here in the Inspector
    public PlayerRespawn playerRespawnScript;


    public void CheckCode()
    {
        for (int i = 0; i < stones.Count; i++)
        {
            if (stones[i].currentNumber != playerRespawnScript.collectedNumbers[i])
            {
                Debug.Log("Wrong code!");
                return;
            }
        }
        Debug.Log("Correct code! Move to the next level!");
        // Load the next level or whatever action you want to perform on correct input
    }
}
