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
    public Material TeamZeroMaterial;
    public Material TeamOneMaterial;

    private MeshRenderer myRenderer;
    private Rigidbody myBody;

    public TextMeshPro text; 

    public int side = 1;

    public AudioSource dragSound;
    public AudioSource tableNockSound;

    public bool isOnTable = false;
    public float velocity;

    private NetworkShuffleboard shuffleboard;

    [Networked(OnChanged = nameof(TeamChanged))]
    public int Team { get; set; }

    private static void TeamChanged(Changed<NetworkPuck> changed)
    {
        Debug.Log("TeamChanged: " + changed.Behaviour.Team);

        changed.Behaviour.SetTeam(changed.Behaviour.Team);
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
        velocity = myBody.velocity.magnitude;

        Debug.Log("Velocity: " + velocity);

        if (isOnTable)
        {
            dragSound.volume = velocity * 0.4f;
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

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Puck Collision " + other);

        tableNockSound.Play();

        if (other.gameObject.CompareTag("Table"))
        {
            isOnTable = true;
            dragSound.volume = 0;
            dragSound.Play();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Table"))
        {
            isOnTable = false;
            dragSound.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ScoreZone")){

            var scoreZone = other.GetComponent<ScoreZone>();

            if(scoreZone.side != side){

                Debug.Log("Triggered " + other.tag + " " + scoreZone.points);

                text.text = scoreZone.points.ToString();
            }
        }
    }
}
