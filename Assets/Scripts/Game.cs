using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Game : MonoBehaviour
{    
    public static Game Instance;

    public NetworkRunner runner;

    public NetworkObject localPlayerObject;

    public bool started;
    
    public float height;

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public NetworkObject GetPlayer()
    {
        if(localPlayerObject != null)
        {
            return localPlayerObject;
        }
        
        if (runner && runner.LocalPlayer.ToString() != "[Player:None]")
        {
            Debug.Log("Local Player Ref: " + runner.LocalPlayer);

            localPlayerObject = runner.GetPlayerObject(runner.LocalPlayer);

            return localPlayerObject;
        }

        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
