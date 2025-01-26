using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputInteractionController : MonoBehaviour
{
    public InteractHint interactHint;
    public Transform distanceOrigin;
    public CinemachineTargetGroup targetGroup;
    
    public Interactable _interactable;
    
    private void Start()
    {
        InputSystem.actions["Interact"].started += HandleInteractInput;
    }

    private void Update()
    {
        Interactable targetInteractable = null;
        float nearestDistance = float.PositiveInfinity;
        
        foreach (Interactable interactable in FindObjectsByType<Interactable>(FindObjectsSortMode.None))
        {
            float distance = Vector3.SqrMagnitude(interactable.transform.position - distanceOrigin.position);
            bool isClosest = distance < nearestDistance;
            bool isInRange = distance < interactable.range * interactable.range;
            
            if (isClosest && isInRange)
            {
                nearestDistance = distance;
                targetInteractable = interactable;
            }
        }

        if (targetInteractable != null && _interactable != targetInteractable)
            interactHint.ShowAbove(targetInteractable.hintPosition);
        
        else if (targetInteractable == null && _interactable != null)
            interactHint.Hide();

        _interactable = targetInteractable;
    }

    private void HandleInteractInput(InputAction.CallbackContext context)
    {
        if (_interactable != null)
            _interactable.Interact();
    }
}
