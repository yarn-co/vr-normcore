using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleboardSideTable : MonoBehaviour
{
    public int side = 1;
    public int team = 0;
    public ShuffleboardMachine machine;
    private NormShuffleboard normShuffle;
    public Transform puckSpawnPoint;
    private GameObject currentPuck;

    // Start is called before the first frame update
    void Start()
    {
        normShuffle = machine.GetComponent<NormShuffleboard>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakePuck()
    {
        currentPuck = normShuffle.MakePuck(team, side, puckSpawnPoint.position);
    }

    
}
