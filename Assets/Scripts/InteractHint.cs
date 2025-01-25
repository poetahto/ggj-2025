using UnityEngine;

public class InteractHint : MonoBehaviour
{
    private Camera _camera;
    private Transform _target;

    private void Start()
    {
        _camera = Camera.main;
        Hide();
    }

    private void Update()
    {
        if (_target != null)
        {
            Vector3 screenPos = _camera.WorldToScreenPoint(_target.position);
            transform.position = screenPos;
        }
    }

    public void ShowAbove(Transform position)
    {
        _target = position;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        _target = null;
        gameObject.SetActive(false);
    }
}
