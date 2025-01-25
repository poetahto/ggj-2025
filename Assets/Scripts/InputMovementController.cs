using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class InputMovementController : MonoBehaviour
    {
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");
        
        public CharacterController controller;
        public Animator animator;
        public float speed;

        private Camera _camera;
        private Vector2 _inputDirection;

        private void Start()
        {
            _camera = Camera.main;
            InputSystem.actions["Walk"].performed += HandleWalk;
            InputSystem.actions["Walk"].canceled += HandleWalk;
        }

        private void Update()
        {
            Vector3 velocity = new Vector3(_inputDirection.x, 0, _inputDirection.y);
            velocity *= speed;
            velocity = Quaternion.Euler(0, _camera.transform.forward.y, 0) * velocity;
            controller.SimpleMove(velocity);
            animator.SetFloat(MoveX, controller.velocity.x);
            animator.SetFloat(MoveY, controller.velocity.y);
        }

        private void HandleWalk(InputAction.CallbackContext context)
        {
            _inputDirection = context.ReadValue<Vector2>();
        }
    }
}
