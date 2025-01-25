using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class InputMovementController : MonoBehaviour
    {
        public CharacterController controller;
        public float speed;

        private void Start()
        {
            GlobalState.GetInstance().CurrentEnergy++;
            
            InputSystem.actions["Walk"].performed += HandleWalk;
            InputSystem.actions["Walk"].canceled += HandleWalk;
        }
        
        private void HandleWalk(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            print($"{direction.x} {direction.y}");
        }
    }
}
