using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace DefaultNamespace
{
    public enum GameState
    {
        Intro, Playing,
    }
    
    public class GlobalState : MonoBehaviour
    {
        [SerializeField] private SceneMusic sceneMusic;
        [SerializeField] private CanvasGroup fadeScreen;
        [SerializeField] private float fadeDuration = 5;
        [SerializeField] private float musicFade = 3;
        public TextBox textBox;

        private EventInstance _musicInstance;
        private SceneMusic.Info _musicInfo;

        public event Action OnUseEnergy;
        public event Action OnRefillEnergy;

        public GameState GameState { get; set; } = GameState.Intro;
        public int EnergyCount { get; private set; } = 5;
        public bool IsTransitioning { get; set; }
        public string RespawnScene { get; set; } = "bio1";
        public string RespawnId { get; set; } = "Bio1Respawn";

        private void Start()
        {
            StartCoroutine(InitializeScene(SceneManager.GetActiveScene().name, string.Empty));
        }

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
            EnergyCount = 5;
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
            yield return StartCoroutine(InitializeScene(targetScene, targetId));
        }

        private IEnumerator InitializeScene(string targetScene, string targetId)
        {
            // play the correct music
            SceneMusic.Info newInfo = sceneMusic.GetInfo(targetScene);

            if (newInfo != null)
            {
                if (_musicInfo != null)
                {
                    if (_musicInfo.eventReference.Guid != newInfo.eventReference.Guid)
                    {
                        float remaining = musicFade;
                        while (remaining > 0)
                        {
                            remaining -= Time.unscaledDeltaTime;
                            _musicInstance.setVolume(Mathf.Clamp01(remaining / musicFade));
                            yield return null;
                        }
                        _musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
                        _musicInstance.release();
                        _musicInstance = RuntimeManager.CreateInstance(newInfo.eventReference);
                        _musicInstance.start();
                    }
                }
                else
                {
                    _musicInstance = RuntimeManager.CreateInstance(newInfo.eventReference);
                    _musicInstance.start();
                }

                
                if (newInfo.parameterId != string.Empty)
                    _musicInstance.setParameterByName(newInfo.parameterId, newInfo.parameterValue);
            }
            else if (_musicInfo != null)
            {
                float remaining = musicFade;
                while (remaining > 0)
                {
                    remaining -= Time.unscaledDeltaTime;
                    _musicInstance.setVolume(Mathf.Clamp01(remaining / musicFade));
                    yield return null;
                }
                _musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
                _musicInstance.release();
            }
            
            _musicInfo = newInfo;
            
            // teleport player to correct warp
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
