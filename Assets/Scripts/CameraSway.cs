using UnityEngine;

public class CameraSway : MonoBehaviour
{
    public float maxOffset;
    public float speed;
    private Vector3 _originalDirection;

    private void Awake()
    {
        _originalDirection = transform.forward;
    }

    private void Update()
    {
        Vector3 offset = new Vector2 {
            x = Mathf.PerlinNoise(Time.time * speed, 0) * 2 - 1,
            y = Mathf.PerlinNoise(0, Time.time * speed) * 2 - 1,
        };
        offset *= maxOffset;
        transform.forward = Quaternion.Euler(offset) * _originalDirection;
    }
}
