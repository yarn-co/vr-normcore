using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;

    public void PlayerJoined(PlayerRef playerRef)
    {
        if (playerRef == Runner.LocalPlayer)
        {
            NetworkObject myPlayerObject = Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, playerRef);

            Runner.SetPlayerObject(playerRef, myPlayerObject);

            Debug.Log("My Player Set: " + playerRef + " : " + myPlayerObject);
        }
    }
}