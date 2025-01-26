using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public enum GameState
    {
        Intro, Playing,
    }
    
    public class GlobalState : MonoBehaviour
    {
        [SerializeField] private CanvasGroup fadeScreen;
        [SerializeField] private float fadeDuration = 5;
        public TextBox textBox;

        public event Action OnUseEnergy;
        public event Action OnRefillEnergy;

        public GameState GameState { get; set; } = GameState.Intro;
        public int EnergyCount { get; private set; } = 4;
        public bool IsTransitioning { get; set; }
        public string RespawnScene { get; set; } = "bio1";
        public string RespawnId { get; set; } = "Bio1Respawn";

        public void Respawn()
        {
            if (RespawnScene != string.Empty && RespawnId != string.Empty && !IsTransitioning)
                StartCoroutine(RespawnCoroutine());
        }

        public void Warp(string targetScene, string targetId)
        {
            if (!IsTransitioning)
                StartCoroutine(WarpCoroutine(targetScene, targetId));
        }

        private IEnumerator RespawnCoroutine()
        {
            IsTransitioning = true;
            textBox.Hide();
            yield return StartCoroutine(FadeTo(1));
            Time.timeScale = 1;
            yield return StartCoroutine(LoadSceneAtWarp(RespawnScene, RespawnId));
            EnergyCount = 4;
            OnRefillEnergy?.Invoke();
            yield return StartCoroutine(FadeTo(0));
            
            // play spawn animation
            WarpLocation warp = GetWarpLocation(RespawnId);

            if (warp != null && warp.HasWarpCutscene())
            {
                InputSystem.actions.FindActionMap("Player").Disable();
                yield return StartCoroutine(warp.PlayCutsceneCoroutine());
                InputSystem.actions.FindActionMap("Player").Enable();
            }
            
            IsTransitioning = false;
        }

        private IEnumerator WarpCoroutine(string targetScene, string targetId)
        {
            IsTransitioning = true;
            textBox.Hide();
            yield return StartCoroutine(FadeTo(1));
            yield return StartCoroutine(LoadSceneAtWarp(targetScene, targetId));
            yield return StartCoroutine(FadeTo(0));

            // pop a bubble after a little bit
            yield return new WaitForSeconds(1);
            EnergyCount--;
            OnUseEnergy?.Invoke();
            
            IsTransitioning = false;
        }

        private IEnumerator FadeTo(float alpha)
        {
            float elapsed = 0;
            float initialValue = fadeScreen.alpha;
            
            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / fadeDuration;
                fadeScreen.alpha = Mathf.Lerp(initialValue, alpha, t);
                yield return null;
            }

            fadeScreen.alpha = alpha;
        }

        public void DepleteAllEnergy()
        {
            EnergyCount = 0;
            OnUseEnergy?.Invoke();
        }
        
        private IEnumerator LoadSceneAtWarp(string targetScene, string targetId)
        {
            yield return SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
            yield return null; // allow things to initialize
            
            GameObject player = GameObject.FindWithTag("Player");
            WarpLocation warp = GetWarpLocation(targetId);
            
            if (warp != null && player != null)
            {
                Vector3 pos = warp.transform.position;
                Quaternion rot = warp.transform.rotation;
                player.transform.SetPositionAndRotation(pos, rot);
            }
        }
        
        private static WarpLocation GetWarpLocation(string id)
        {
            foreach (WarpLocation warp in FindObjectsByType<WarpLocation>(FindObjectsSortMode.None))
            {
                if (warp.id != id)
                    continue;

                return warp;
            }
            
            return null;
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
