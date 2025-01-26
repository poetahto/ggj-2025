using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class TextBox : MonoBehaviour
    {
        public TMP_Text display;
        public CanvasGroup canvasGroup;
        public float fadeTime = 0.25f;
        public float characterDelay = 0.1f;
        
        private WaitForSecondsRealtime _characterDelay;
        private Coroutine _currentCoroutine;

        public bool IsRunning => _currentCoroutine != null;

        private void Awake()
        {
            _characterDelay = new WaitForSecondsRealtime(characterDelay);
        }
        
        public void SetText(string text, float duration)
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            
            _currentCoroutine = StartCoroutine(SetTextCoroutine(text, duration));
        }

        public void Hide()
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            
            _currentCoroutine = StartCoroutine(HideCoroutine());
        }

        private IEnumerator HideCoroutine()
        {
            yield return StartCoroutine(FadeTo(0));
            _currentCoroutine = null;
        }

        private IEnumerator SetTextCoroutine(string text, float duration)
        {
            yield return StartCoroutine(FadeTo(0));
            display.maxVisibleCharacters = 0;
            display.text = text;
            yield return StartCoroutine(FadeTo(1));

            while (display.maxVisibleCharacters < text.Length)
            {
                display.maxVisibleCharacters++;
                yield return _characterDelay;
            }

            if (duration > 0)
            {
                yield return new WaitForSecondsRealtime(duration);
                yield return FadeTo(0);
            }

            _currentCoroutine = null;
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
}
