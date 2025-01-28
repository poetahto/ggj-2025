using DefaultNamespace;
using FMODUnity;
using UnityEngine;

public class FmodParameterIncrease : MonoBehaviour
{
    public string parameterId;
    public float maxDuration;
    public bool alwaysRunning = false;
    public StudioEventEmitter emitter;

    private bool _hasPlayer = false;
    private float _stayDuration = 0;

    private void Update()
    {
        _stayDuration = _hasPlayer || alwaysRunning ? Mathf.Clamp(_stayDuration + Time.deltaTime, 0, maxDuration) : Mathf.Clamp(_stayDuration - Time.deltaTime, 0, maxDuration);
        float t = Mathf.Clamp01(_stayDuration / maxDuration);
        if (emitter != null)
            emitter.SetParameter(parameterId, t);
        else GlobalState.GetInstance()._musicInstance.setParameterByName(parameterId, t);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _hasPlayer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _hasPlayer = false;
    }
}
