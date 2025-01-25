using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class DeathTester : MonoBehaviour
    {
        private void Start()
        {
            InputSystem.actions["Interact"].started += HandleInteract;
        }
        
        private void HandleInteract(InputAction.CallbackContext context)
        {
            GlobalState.GetInstance().Respawn();
        }
    }
}
