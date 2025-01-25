using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class GlobalState : MonoBehaviour
    {
        [SerializeField] private CanvasGroup fadeScreen;
        [SerializeField] private float fadeDuration = 5;

        public Dictionary<string, float> Floats =  new Dictionary<string, float>();
        
        public bool IsTransitioning { get; set; }
        public string RespawnScene { get; set; }
        public string RespawnId { get; set; }
        // todo: track respawn scene + id
        // todo: respawn logic
        // todo: track all death scenes + position

        public void Respawn()
        {
            if (RespawnScene != string.Empty && RespawnId != string.Empty)
                Warp(RespawnScene, RespawnId);
        }

        public void Warp(string targetScene, string targetId)
        {
            if (!IsTransitioning)
                StartCoroutine(WarpCoroutine(targetScene, targetId));
        }

        private IEnumerator WarpCoroutine(string targetScene, string targetId)
        {
            IsTransitioning = true;
            yield return StartCoroutine(FadeTo(1));
            yield return SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
            yield return null;
            WarpLocation targetWarp = null;

            foreach (WarpLocation warp in FindObjectsByType<WarpLocation>(FindObjectsSortMode.None))
            {
                if (warp.id != targetId)
                    continue;
                
                targetWarp = warp;
                break;
            }
            
            GameObject player = GameObject.FindWithTag("Player");
            
            if (targetWarp != null && player != null)
            {
                Vector3 pos = targetWarp.transform.position;
                Quaternion rot = targetWarp.transform.rotation;
                player.transform.SetPositionAndRotation(pos, rot);
            }
            
            yield return StartCoroutine(FadeTo(0));
            IsTransitioning = false;
        }

        private IEnumerator FadeTo(float alpha)
        {
            float elapsed = 0;
            float initialValue = fadeScreen.alpha;
            
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;
                fadeScreen.alpha = Mathf.Lerp(initialValue, alpha, t);
                yield return null;
            }

            fadeScreen.alpha = alpha;
        }
        
        private static GlobalState _instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _instance = null;
        }
        
        public static GlobalState GetInstance()
        {
            if (_instance == null)
            {
                GlobalState globalState = Resources.Load<GlobalState>("global_state");
                _instance = Instantiate(globalState);
                DontDestroyOnLoad(_instance);
            }
            
            return _instance;
        }
    }
}
