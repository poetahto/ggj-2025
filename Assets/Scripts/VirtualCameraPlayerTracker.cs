using Cinemachine;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class VirtualCameraPlayerTracker : MonoBehaviour
    {
        private CinemachineVirtualCamera _camera;

        private void Awake()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
            GameObject player = GameObject.FindWithTag("Player");
            _camera.LookAt = player.transform;
        }
    }
}
