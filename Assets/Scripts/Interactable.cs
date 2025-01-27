using System;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public Transform hintPosition;
    public UnityEvent onInteract;
    public float range = 5;
    public float radius = 1;
    public float maxWeight = 1;
    public PlayerAnimation.RobotFace emotion;
    [SerializeField] private bool defaultIsInteractable = true;
    
    public bool IsInteractable { get; set; }

    private void Awake()
    {
        IsInteractable = defaultIsInteractable;
    }

    public void Interact()
    {
        if (IsInteractable)
            onInteract.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}