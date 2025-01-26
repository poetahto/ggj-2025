using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class TextBox : MonoBehaviour
    {
        public TMP_Text display;
        public float fadeTime = 0.25f;
        public float characterDelay = 0.1f;
        
        private WaitForSeconds _characterDelay;
        private Coroutine _currentCoroutine;

        private void Awake()
        {
            _characterDelay = new WaitForSeconds(characterDelay);
        }

        public void SetText(string text)
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            
            _currentCoroutine = StartCoroutine(SetTextCoroutine(text));
        }

        public void Hide()
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            
            _currentCoroutine = StartCoroutine(FadeTo(0));
        }

        private IEnumerator SetTextCoroutine(string text)
        {
            yield return StartCoroutine(FadeTo(0));
            display.maxVisibleCharacters = 0;
            display.text = text;
            display.alpha = 1;

            while (display.maxVisibleCharacters < text.Length)
            {
                display.maxVisibleCharacters++;
                yield return _characterDelay;
            }
        }

        private IEnumerator FadeTo(float alpha)
        {
            float elapsed = 0;
            float initial = display.alpha;
            
            while (elapsed <= fadeTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / fadeTime);
                display.alpha = Mathf.Lerp(initial, alpha, t);
                yield return null;
            }

            display.alpha = alpha;
        }
    }
}
