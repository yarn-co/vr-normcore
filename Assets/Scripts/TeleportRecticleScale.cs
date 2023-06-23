using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spacebar.Realtime;

public class TeleportRecticleScale : MonoBehaviour
{
    public NormPlayer _normPlayer;
    
    public bool isLocal = true;

    // Start is called before the first frame update
    void Start()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");

        RealtimeAvatarManager avatarManager = controller.GetComponent<RealtimeAvatarManager>();

        if (isLocal)
        {
            _normPlayer = avatarManager.localAvatar.GetComponent<NormPlayer>();
        }

        _normPlayer.onScaleChange += OnScaleChanged;

        Debug.Log("TeleportRecticleScale Start: " + _normPlayer);
    }

    public void OnScaleChanged()
    {
        Debug.Log("TeleportRecticleScale OnScaleChanged: " + _normPlayer.Scale);
        
        float scale = _normPlayer.Scale;

        gameObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
