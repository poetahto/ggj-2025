using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Springs/FloatSpring")]
public class GenericSpring : ScriptableObject{
    public float springForce;
    public float curr;
    public float k;
    public float dampening;
    public float mass;
    public float intensity;


    public float Evaluate(float goal){
        springForce += (curr - goal) * Time.deltaTime;//k * deltaX
        springForce *= dampening;
        curr -= springForce / mass;
        return curr * intensity;
    }
}