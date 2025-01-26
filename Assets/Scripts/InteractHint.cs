using System.Collections;
using UnityEngine;

public class InteractHint : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeTime = 0.5f;
    private Camera _camera;
    private Transform _target;
    private Coroutine _coroutine;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_target != null)
        {
            Vector3 screenPos = _camera.WorldToScreenPoint(_target.position);
            transform.position = screenPos;
        }
    }

    public void ShowAbove(Transform target)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        
        _coroutine = StartCoroutine(ShowCoroutine(target));
    }

    public void Hide()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(HideCoroutine());
    }

    private IEnumerator HideCoroutine()
    {
        yield return StartCoroutine(FadeTo(0));
        _target = null;
    }

    private IEnumerator ShowCoroutine(Transform target)
    {
        yield return StartCoroutine(FadeTo(0));
        _target = target;
        yield return StartCoroutine(FadeTo(1));
    }
    
    private IEnumerator FadeTo(float alpha)
    {
        float elapsed = 0;
        float initial = canvasGroup.alpha;
            
        while (elapsed <= fadeTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeTime);
            canvasGroup.alpha = Mathf.Lerp(initial, alpha, t);
            yield return null;
        }

        canvasGroup.alpha = alpha;
    }
}
