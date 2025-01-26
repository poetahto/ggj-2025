using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "Springs/QuaternionSpring")]
public class QuaternionSpring : ScriptableObject
{
    public Quaternion springAcceleration;
    public Quaternion curr;
    public float k;
    public float dampening;
    public float mass;

    public float intensity;


    public Quaternion Evaluate(Quaternion goal){
        Quaternion diff = (curr * Quaternion.Inverse(goal));//k * deltaX
        //curr -= springForce / mass;
        //Funky quaternion stuff??
        diff.x *= Time.deltaTime;
        diff.y *= Time.deltaTime;
        diff.z *= Time.deltaTime;
        diff.w *= Time.deltaTime;

        springAcceleration *= diff;

        springAcceleration.x *= dampening / mass;
        springAcceleration.y *= dampening / mass;
        springAcceleration.z *= dampening / mass;
        springAcceleration.w *= dampening / mass;

        curr *= Quaternion.Inverse(springAcceleration.normalized);
        
        curr.x *= intensity;
        curr.y *= intensity;
        curr.z *= intensity;
        curr.w *= intensity;
        
        return curr;
    }
}
