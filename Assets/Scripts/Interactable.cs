using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public Transform hintPosition;
    public UnityEvent onInteract;
    public float range = 5;
    public float radius = 1;
    public float maxWeight = 1;

    public void Interact()
    {
        onInteract.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.DrawSphere(transform.position, radius);
    }
}