using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using TMPro;
using Fusion;

public class NetworkShuffleboard : MonoBehaviour
{
      
    public NetworkObject puck;

    private GameObject[] pucks;


    void Awake(){

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void MakePuckForZero(){
        MakePuck(0);
    }

    public void MakePuckForOne(){
        MakePuck(1);
    }

    public void MakePuck(int team){

        //GameObject newPuck = Instantiate(puck, new Vector3(0, 1, 0.3f), Quaternion.identity);

        //newPuck.GetComponent<Puck>().team = team;

        NetworkObject newPuck = Game.Instance.runner.Spawn(puck, new Vector3(0, 1, 0.3f), Quaternion.identity);

        newPuck.GetComponent<NetworkPuck>().Team = team;
    }

    public void ClearPucks(){

        pucks = GameObject.FindGameObjectsWithTag("Puck");

        foreach (GameObject puckX in pucks)
        {
            Destroy(puckX);
        }
    }

    public void Reset()
    {

    }
}
