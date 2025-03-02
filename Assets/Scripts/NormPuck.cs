using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Normal.Realtime;

/*
public class NormPuck : MonoBehaviour
{
    //this exists for dealing with compilation issues when changing model
}
*/

public class NormPuck : RealtimeComponent<NormPuckModel>
{
    public NormPuckModel _model;

    private Puck _puck;
    private RealtimeTransform _realtimeTransform;

    private MeshRenderer myRenderer;
    private Rigidbody myBody;

    public Material TeamZeroMaterial;
    public Material TeamOneMaterial;
    public TextMeshPro text;

    public AudioSource dragSound;
    public AudioSource tableNockSound;

    public int team = 0;
    public int Team {
        get
        {
            return team;
        }
        set
        {
            Debug.Log("NormPuck SetTeam: " + value);

            team = value;
            _puck.Team = value;
            _model.team = value;
        }
    }

    public int side = 1;
    public int Side
    {
        get
        {
            return side;
        }
        set
        {
            Debug.Log("NormPuck SetSide: " + value);

            side = value;
            _puck.side = value;
            _model.side = value;
        }
    }

    private float velocity;

    protected override void OnRealtimeModelReplaced(NormPuckModel previousModel, NormPuckModel currentModel)
    {
        if (previousModel != null)
        {
            // Unregister from events
            previousModel.teamDidChange -= TeamChanged;
            previousModel.isOnTableDidChange -= IsOnTableChanged;
        }

        if (currentModel != null)
        {
            _model = currentModel;

            _model.teamDidChange += TeamChanged;
            _model.isOnTableDidChange += IsOnTableChanged;
            _model.collisionCountDidChange += CollisionCountChanged;

            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                _model.team = Team;
                _model.side = Side;
                _model.collisionCount = 0;
            }
            else
            {
                Team = _model.team;
                Side = _model.side;
            }
        }
    }

    private void TeamChanged(NormPuckModel model, int value)
    {
        Debug.Log("TeamChanged: " + value);

        team = value;
        _puck.team = value;
    }

    private void IsOnTableChanged(NormPuckModel model, bool isOnTable)
    {
        Debug.Log("isOnTableChanged: " + isOnTable);

        if (isOnTable)
        {
            dragSound.volume = 0f;
            dragSound.Play();
        }
        else
        {
            dragSound.Stop();
        }
    }

    private void CollisionCountChanged(NormPuckModel model, int value)
    {
        tableNockSound.Play();
    }

    // Start is called before the first frame update
    void Awake()
    {
        myRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        myBody = gameObject.GetComponent<Rigidbody>();

        _realtimeTransform = GetComponent<RealtimeTransform>();
        _puck = GetComponent<Puck>();
    }



    void Start()
    {
        Debug.Log("NormPuck Start");

        text.text = "";
    }

    private void Update()
    {
        if (_realtimeTransform.isOwnedLocallySelf)
        {
            velocity = myBody.velocity.magnitude;

            _model.velocity = velocity;
        } 
        else {

            velocity = _model.velocity;
        }

        if (_model.isOnTable)
        {
            dragSound.volume = velocity * 0.4f;

            //Debug.Log("dragSound.volume: " + dragSound.volume);
        }
    }

    

    protected virtual void OnCollisionEnter(Collision other)
    {
        Debug.Log("Puck Collision " + other);

        if (_realtimeTransform.isOwnedLocallySelf)  // has ownershipt
        {
            _model.collisionCount++;

            if (other.gameObject.CompareTag("Table"))
            {
                _model.isOnTable = true;
            }
        }

        
    }

    private void OnCollisionExit(Collision other)
    {
        if (_realtimeTransform.isOwnedLocallySelf)  // has ownershipt
        {
            if (other.gameObject.CompareTag("Table"))
            {
                _model.isOnTable = false;
            }
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ScoreZone")){

            var scoreZone = other.GetComponent<ScoreZone>();

            if(scoreZone.side != _model.side){

                Debug.Log("Triggered " + other.tag + " " + scoreZone.points);

                text.text = scoreZone.points.ToString();
            }
        }
    }*/
}