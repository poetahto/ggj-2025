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
        public float moveSpeed = 0.8f;
        public float rotateSpeed = .8f;

        public Vector2 _velocity = new Vector2(0, 0);// x is rotation velocity, y is move velocity

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
            _velocity.y = Mathf.Clamp(_inputDirection.x * rotateSpeed, -rotateSpeed, rotateSpeed);//Lmao
            _velocity.x = Mathf.Clamp(_inputDirection.y * moveSpeed, -moveSpeed, moveSpeed);
            this.transform.Rotate(new Vector3(0,_velocity.y, 0));
            controller.SimpleMove(this.transform.forward * _velocity.x);
            animator.SetFloat(MoveX, controller.velocity.x);
            animator.SetFloat(MoveY, controller.velocity.y);
        }

        private void HandleWalk(InputAction.CallbackContext context)
        {
            _inputDirection = context.ReadValue<Vector2>();
        }
    }
}
