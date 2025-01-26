using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class InputMovementController : MonoBehaviour
    {        
        public CharacterController controller;
        public float moveSpeed = 0.8f;
        public float rotateSpeed = .8f;
        public float acceleration = 1;
        public StudioEventEmitter treadAudioEmitter;

        private Vector2 _targetVelocity;
        public Vector2 _velocity = new Vector2(0, 0);// x is rotation velocity, y is move velocity

        private float _currentSpeed;
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
            _targetVelocity.y = Mathf.Clamp(_inputDirection.x * rotateSpeed, -rotateSpeed, rotateSpeed);//Lmao
            _targetVelocity.x = Mathf.Clamp(_inputDirection.y * moveSpeed, -moveSpeed, moveSpeed);
            _velocity = Vector3.MoveTowards(_velocity, _targetVelocity, acceleration * Time.deltaTime);
            
            transform.Rotate(new Vector3(0,_velocity.y, 0));
            controller.SimpleMove(this.transform.forward * _velocity.x);
            float forwardPercent = Mathf.Abs(_velocity.x) / moveSpeed;
            float rotatePercent = Mathf.Abs(_velocity.y) / rotateSpeed;
            if(treadAudioEmitter != null){
                treadAudioEmitter.SetParameter("Speed", (forwardPercent + rotatePercent) / 2.0f);
                treadAudioEmitter.SetParameter("Ptich_Turning", rotatePercent);
            }
        }

        private void HandleWalk(InputAction.CallbackContext context)
        {
            _inputDirection = context.ReadValue<Vector2>();
        }
    }
}
