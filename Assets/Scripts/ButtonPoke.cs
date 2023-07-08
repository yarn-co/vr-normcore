using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class ButtonPoke : MonoBehaviour
{
    public Transform visualTarget;
    public Vector3 localAxis;
    public float resetSpeed = 5f;

    private XRBaseInteractable interactable;
    private bool isFollowing = false;
    private bool isSelected = false;
    public Vector3 offset;
    public Vector3 visualTargetLocalPosition;
    public Vector3 initialVisualPosition;
    public Vector3 constrainedLocalTargetPosition;
    public Vector3 localtargetPosition;
    private Transform pokeAttachTransform;

    // Start is called before the first frame update
    void Start()
    {
        initialVisualPosition = visualTarget.localPosition;
        interactable = GetComponent<XRBaseInteractable>();
        
        interactable.hoverEntered.AddListener(Follow);
        interactable.hoverExited.AddListener(Unfollow);

        interactable.selectEntered.AddListener(Select);
        interactable.selectExited.AddListener(SelectExit);
    }

    public void Select(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            isSelected = true;
        }
    }

    public void SelectExit(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            isSelected = false;
        }
    }

    public void Follow(BaseInteractionEventArgs hover)
    {
        if(hover.interactorObject is XRPokeInteractor)
        {
            XRPokeInteractor interactor = (XRPokeInteractor)hover.interactorObject;
            isFollowing = true;
            pokeAttachTransform = interactor.attachTransform;
            offset = visualTarget.position - pokeAttachTransform.position;
        }
    }

    public void Unfollow(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            isFollowing = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing)
        {
            if (!isSelected)
            {
                localtargetPosition = visualTarget.InverseTransformPoint(pokeAttachTransform.position + offset);

                constrainedLocalTargetPosition =  Vector3.Project(localtargetPosition, localAxis);

                visualTarget.position = visualTarget.TransformPoint(constrainedLocalTargetPosition);
                
                if(visualTarget.localPosition.y > initialVisualPosition.y) visualTarget.localPosition = initialVisualPosition;
            }
        }
        else
        {
            visualTarget.localPosition = Vector3.Lerp(visualTarget.localPosition, initialVisualPosition, Time.deltaTime * resetSpeed);
        }
    }
}
