using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    public float rotationLookModifier = 2;
    public float lookSpeed = 2;

    [Range(0, 0.1f)]
    public float bounceIntensity = 0.5f;
    public float bounceSpeed = 5;

    public Transform head;
    public Transform headGoal;
    // public Transform neck;
    public Transform torso;
    public Transform feet;

    public Material treadL,treadR;

    public Material faceMat;

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

    public InputInteractionController interactionController;
    public InputMovementController movementController;

    public Vector3 neckInitPosition;
    public GenericSpring bodySpring;
    public GenericSpring headSpring;
    public GenericSpring bodySpringY;
    GenericSpring headSpringY, headSpringZ;
    public float initalBodyY;

    public QuaternionSpring headRotSpring;


    // Start is called before the first frame update
    void Start(){
        initalBodyY = torso.transform.localPosition.y;

        headSpringY = Instantiate(headSpring);
        headSpringZ = Instantiate(headSpring);
        bodySpringY = Instantiate(bodySpring);

        // neckInitPosition = neck.transform.localPosition;
        interactionController = this.GetComponent<InputInteractionController>();
        movementController = this.GetComponent<InputMovementController>();
    }

    public void SetFace(RobotFace emotion){
        int y = (int)emotion / 4;
        int x = (int)emotion % 4; 
        faceMat.mainTextureOffset = new Vector2(0.25f * x, .5f * y);
    }

    // Update is called once per frame
    void Update(){
        treadL.mainTextureOffset += new Vector2(movementController._velocity.x/movementController.moveSpeed + movementController._velocity.y/movementController.rotateSpeed, 0);
        treadR.mainTextureOffset += new Vector2(movementController._velocity.x/movementController.moveSpeed + movementController._velocity.y/movementController.rotateSpeed, 0);

        float goal = movementController._velocity.x/movementController.moveSpeed;
        float bodySpringResult = -bodySpring.Evaluate(goal);
        torso.transform.localRotation = Quaternion.Euler(bodySpringResult, 0, 0);
        // torso.transform.localRotation = Quaternion.Euler(-Mathf.Sign(bodyAngle) * springCurve.Evaluate(Mathf.Abs(bodyAngle)) * moveLaxivityIntensity, 0, 0);
        

        Quaternion headLookGoal = headGoal.transform.rotation;
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
