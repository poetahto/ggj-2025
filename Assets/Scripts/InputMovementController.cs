using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class InputMovementController : MonoBehaviour
    {        
        public CharacterController controller;
        public float moveSpeed = 0.8f;
        public float rotateSpeed = .8f;
        public float rotateAccel = 90;
        [FormerlySerializedAs("acceleration")]
        public float moveAccel = 1;
        public StudioEventEmitter treadAudioEmitter;

        private Vector2 _targetVelocity;
        public Vector2 _velocity = new Vector2(0, 0);// x is rotation velocity, y is move velocity

        private float _currentSpeed;
        private Camera _camera;
        private Vector2 _inputDirection;

        private void Start()
        {
            moveSpeed *= transform.lossyScale.magnitude;
            moveAccel *= transform.lossyScale.magnitude;
            _camera = Camera.main;
            InputSystem.actions["Walk"].performed += HandleWalk;
            InputSystem.actions["Walk"].canceled += HandleWalk;
            InputSystem.actions["DebugSpeedUp"].started += HandleSpeedUp;
            InputSystem.actions["DebugSlowDown"].started += HandleSlowDown;
        }

        private void OnDestroy()
        {
            InputSystem.actions["Walk"].performed -= HandleWalk;
            InputSystem.actions["Walk"].canceled -= HandleWalk;
            InputSystem.actions["DebugSpeedUp"].started -= HandleSpeedUp;
            InputSystem.actions["DebugSlowDown"].started -= HandleSlowDown;
        }

        private void HandleSpeedUp(InputAction.CallbackContext context)
        {
            Time.timeScale *= 2;
        }
        
        private void HandleSlowDown(InputAction.CallbackContext context)
        {
            Time.timeScale *= 0.5f;
        }

        private void Update()
        {
            _targetVelocity.y = Mathf.Clamp(_inputDirection.x * rotateSpeed, -rotateSpeed, rotateSpeed);//Lmao
            _targetVelocity.x = Mathf.Clamp(_inputDirection.y * moveSpeed, -moveSpeed, moveSpeed);
            _velocity.x = Mathf.MoveTowards(_velocity.x, _targetVelocity.x, moveAccel * Time.deltaTime);
            _velocity.y = Mathf.MoveTowards(_velocity.y, _targetVelocity.y, rotateAccel * Time.deltaTime);
            
            transform.Rotate(new Vector3(0,_velocity.y * Time.deltaTime, 0));
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
