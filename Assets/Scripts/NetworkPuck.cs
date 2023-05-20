using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;
using Fusion.XR.Shared.Grabbing;

[RequireComponent(typeof(NetworkGrabbable))]
[RequireComponent(typeof(Grabbable))]
public class NetworkPuck : NetworkBehaviour
{
    private MeshRenderer myRenderer;
    private Rigidbody myBody;
    private NetworkShuffleboard shuffleboard;

    public Material TeamZeroMaterial;
    public Material TeamOneMaterial;
    public TextMeshPro text; 

    public AudioSource dragSound;
    public AudioSource tableNockSound;

    [Networked(OnChanged = nameof(isOnTableChanged))]
    public bool isOnTable { get; set; } = false;

    [Networked]
    public float Velocity { get; set; }

    [Networked]
    public int Side { get; set; } = 1;

    [Networked(OnChanged = nameof(TeamChanged))]
    public int Team { get; set; }

    [Networked(OnChanged = nameof(CollisionsChanged))]
    public int Collisions { get; set; }

    private static void TeamChanged(Changed<NetworkPuck> changed)
    {
        Debug.Log("TeamChanged: " + changed.Behaviour.Team);

        changed.Behaviour.SetTeam(changed.Behaviour.Team);
    }

    private static void isOnTableChanged(Changed<NetworkPuck> changed)
    {
        Debug.Log("isOnTableChanged: " + changed.Behaviour.isOnTable);

        if (changed.Behaviour.isOnTable)
        {
            changed.Behaviour.dragSound.volume = 0;
            changed.Behaviour.dragSound.Play();
        }
        else
        {
            changed.Behaviour.dragSound.Stop();
        }
    }

    private static void CollisionsChanged(Changed<NetworkPuck> changed)
    {
        changed.Behaviour.tableNockSound.Play();
    }

    // Start is called before the first frame update
    void Awake()
    {
        myRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        myBody = gameObject.GetComponent<Rigidbody>();

        shuffleboard = Game.Instance.GetComponent<NetworkShuffleboard>();

        NetworkGrabbable grabbable = GetComponent<NetworkGrabbable>();
        grabbable.onDidGrab.AddListener(OnDidGrab);
        grabbable.grabbable.onWillGrab.AddListener(OnWillGrab);
    }

    void Start()
    {
        Debug.Log("NetworkPuck Start");

        SetTeam(Team);

        text.text = "";
    }

    private void Update()
    {
        if (Object.HasStateAuthority)
        {
            Velocity = myBody.velocity.magnitude;
        }

        if (isOnTable)
        {
            dragSound.volume = Velocity * 0.4f;

            //Debug.Log("dragSound.volume: " + dragSound.volume);
        }
    }

    public void SetTeam(int t)
    {
        if (t == 1)
        {
            myRenderer.material = TeamOneMaterial;
        }
        else
        {
            myRenderer.material = TeamZeroMaterial;
        }
    }

    void OnDidGrab(NetworkGrabber newGrabber)
    {
        
    }

    void OnWillGrab(Grabber newGrabber)
    {
        shuffleboard.OwnAllPucks();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
    public void RPC_Collision()
    {
        tableNockSound.Play();
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        //Debug.Log("Puck Collision " + Object.HasStateAuthority);

        if (Object.HasStateAuthority)
        {
            Collisions++;

            if (other.gameObject.CompareTag("Table"))
            {
                isOnTable = true;
                //dragSound.volume = 0;
                //dragSound.Play();
            }
        }

        
    }

    private void OnCollisionExit(Collision other)
    {
        if (Object.HasStateAuthority)
        {
            if (other.gameObject.CompareTag("Table"))
            {
                isOnTable = false;
                //dragSound.Stop();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ScoreZone")){

            var scoreZone = other.GetComponent<ScoreZone>();

            if(scoreZone.side != Side){

                //Debug.Log("Triggered " + other.tag + " " + scoreZone.points);

                text.text = scoreZone.points.ToString();
            }
        }
    }
}
