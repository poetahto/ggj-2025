using FMODUnity;
using UnityEngine;

public class DeviceScreen : MonoBehaviour
{
    public float duration;
    public Renderer target;
    public Material offMaterial;
    public Material onMaterial;

    private float _elapsed;
    private bool _isOn;

    private void Update()
    {
        if (_isOn)
        {
            _elapsed += Time.deltaTime;

            if (_elapsed > duration)
            {
                _isOn = false;
                _elapsed = 0;
                target.material = offMaterial;
            }
        }
    }

    public void PowerOn()
    {
        RuntimeManager.PlayOneShot("event:/Datapad_Interact");
        target.material = onMaterial;
        _elapsed = 0;
        _isOn = true;
    }
}
