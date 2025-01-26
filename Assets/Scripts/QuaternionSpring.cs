using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class QuaternionSpring : ScriptableObject
{
    public Quaternion springForce;
    public Quaternion curr;
    public float k;
    public float dampening;
    public float mass;

    public float intensity;


    public Quaternion Evaluate(Quaternion goal){
        Quaternion diff = (curr * Quaternion.Inverse(goal));//k * deltaX
        //curr -= springForce / mass;
        return diff;
    }
}
