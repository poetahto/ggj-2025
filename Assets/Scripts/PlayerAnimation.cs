using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    public float rotationLookModifier = 2;
    public float bodyLookModifier = 6;
    public float lookSpeed = 2;

    [Range(0, 0.1f)]
    public float bounceIntensity = 0.5f;
    public float bounceSpeed = 5;

    [Header("General Transforms")]

    public Transform head;
    public Transform headGoal;
    // public Transform neck;
    public Transform torso;
    public Transform feet;

    [Header("Tread Stuff")]
    public MeshRenderer treadL;
    public MeshRenderer treadR;

    public TrailRenderer trackTrailL, trackTrailR;
    public MeshRenderer faceRenderer;

    public enum RobotFace {
        shocked,
        nervous,
        angry,
        blank,
        neutral,
        sad,
        happy,
        question
    }

    [Header("Controllers")]

    public InputInteractionController interactionController;
    public InputMovementController movementController;

    [Header("Springs")]
    public GenericSpring bodySpring;
    public GenericSpring headSpring;
    public GenericSpring bodySpringY;
    GenericSpring headSpringY, headSpringZ;
    
    float initalBodyY;



    // Start is called before the first frame update
    void Start(){
        initalBodyY = torso.transform.localPosition.y;

        headSpringY = Instantiate(headSpring);
        headSpringZ = Instantiate(headSpring);
        bodySpringY = Instantiate(bodySpring);
    }

    public void SetFace(RobotFace emotion){
        int y = (int)emotion / 4;
        int x = (int)emotion % 4; 
        faceRenderer.material.mainTextureOffset = new Vector2(0.25f * x, .5f * y);
    }

    // Update is called once per frame
    void Update(){

        this.transform.position = movementController.transform.position;
        feet.transform.rotation = movementController.transform.rotation;

        treadL.material.mainTextureOffset -= new Vector2(0, movementController._velocity.x + movementController._velocity.y) * 4 * Time.deltaTime;
        treadR.material.mainTextureOffset -= new Vector2(0, movementController._velocity.x - movementController._velocity.y) * 4 * Time.deltaTime;
        trackTrailL.emitting = movementController.controller.isGrounded;
        trackTrailR.emitting = movementController.controller.isGrounded;

        //Physics.Raycast(new Ray(treadL.transform.position, -treadL.transform.up), out , 0.25f, ~LayerMask.NameToLayer("Player"));
        //trackTrailL.emitting = Physics.Raycast(new Ray(treadL.transform.position, -treadL.transform.up), 0.25f, LayerMask.NameToLayer("Snow"));
        //trackTrailR.emitting = Physics.Raycast(new Ray(treadR.transform.position, -treadR.transform.up), 0.25f, LayerMask.NameToLayer("Snow"));

        float bodySpringResult = -bodySpring.Evaluate(movementController._velocity.x/movementController.moveSpeed);
        torso.transform.localRotation = Quaternion.LookRotation(movementController.transform.forward) * Quaternion.Euler(bodySpringResult, 0, 0);
        

        Quaternion headLookGoal = headGoal.transform.rotation;
        SetFace(RobotFace.neutral);
        if(interactionController._interactable != null){
            headLookGoal = Quaternion.Euler(bodySpringResult, 0, 0) * Quaternion.LookRotation(interactionController._interactable.transform.position - head.transform.position);
            SetFace(interactionController._interactable.emotion);
        }
        headSpring.curr = head.transform.position.x;
        headSpringY.curr = head.transform.position.y;
        headSpringZ.curr = head.transform.position.z;

        bodySpringY.curr = torso.transform.localPosition.y;
        torso.transform.localPosition = new Vector3(torso.transform.localPosition.x, initalBodyY + Mathf.Sin(Time.time * bounceSpeed) * bounceIntensity, torso.transform.localPosition.x);
        // torso.transform.localPosition = new Vector3(torso.transform.localPosition.x,
        //                                             bodySpringY.Evaluate(initalBodyY + Mathf.Sin(Time.time * bounceSpeed) * bounceIntensity),
        //                                             torso.transform.localPosition.z);
        //bodySpring.Evaluate()
        //Add spring
        head.transform.position = new Vector3(  headSpring.Evaluate(headGoal.transform.position.x),
                                                headSpringY.Evaluate(headGoal.transform.position.y),
                                                headSpringZ.Evaluate(headGoal.transform.position.z));
        
        head.transform.rotation = Quaternion.Slerp(head.transform.rotation, headLookGoal, (lookSpeed + Mathf.Abs(movementController._velocity.y) * rotationLookModifier) * Time.deltaTime);
        // head.transform.rotation = head.transform.rotation * Quaternion.Inverse(headLookGoal);
    }
}
