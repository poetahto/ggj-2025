using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace DefaultNamespace
{
    public enum GameState
    {
        Intro, Playing,
    }

    public class GlobalState : MonoBehaviour
    {
        [SerializeField] private GameObject bsod;
        [SerializeField] private SceneMusic sceneMusic;
        [SerializeField] private CanvasGroup fadeScreen;
        [SerializeField] private float fadeDuration = 5;
        [SerializeField] private float musicFade = 3;
        [SerializeField] private GameObject deathModel;
        public TextBox textBox;

        private EventInstance _musicInstance;
        private SceneMusic.Info _musicInfo;
        private List<DeathInfo> _deathInfo = new List<DeathInfo>();

        public event Action OnUseEnergy;
        public event Action OnRefillEnergy;

        public int ProgressCounter { get; set; } = 2;
        public int DeathCount { get; set; }
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

        public void RefillPower()
        {
            OnRefillEnergy?.Invoke();
            EnergyCount = 6;
            RuntimeManager.PlayOneShot("event:/Robot_Recharge");
        }

        public void Warp(string targetScene, string targetId)
        {
            if (!IsTransitioning)
                StartCoroutine(WarpCoroutine(targetScene, targetId));
        }

        private IEnumerator RespawnCoroutine()
        {
            _musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
            _musicInstance.release();
            _musicInfo = null;
            IsTransitioning = true;
            GameObject player = GameObject.FindWithTag("Player");
            DeathInfo deathInfo = new DeathInfo {
                Position = player.transform.position,
                SceneName = player.scene.name,
            };
            _deathInfo.Add(deathInfo);
            bsod.SetActive(true);
            textBox.Hide();
            Time.timeScale = 1;
            yield return StartCoroutine(LoadSceneAtWarp(RespawnScene, RespawnId));
            RuntimeManager.PlayOneShot("event:/Computer_Boot_Up");
            yield return new WaitForSeconds(1.64f);
            fadeScreen.alpha = 1;
            bsod.SetActive(false);
            yield return StartCoroutine(InitializeScene(RespawnScene, RespawnId));
            yield return new WaitForSeconds(2.0f);
            // fadeScreen.alpha = 0; 
            StartCoroutine(FadeTo(0));
            EnergyCount = 5;
            OnRefillEnergy?.Invoke();
            player = GameObject.FindWithTag("Player");
            
            // play spawn animation
            WarpLocation warp = GetWarpLocation(RespawnId);
            textBox.SetText($"Fabricated B.A.R.T. Model {DeathCount + 2}", 1);
            DeathCount++;

            if (warp != null && warp.HasWarpCutscene())
            {
                InputSystem.actions.FindActionMap("Player").Disable();
                CharacterController controller = player.GetComponentInChildren<CharacterController>();
                controller.enabled = false;
                yield return StartCoroutine(warp.PlayCutsceneCoroutine());
                controller.enabled = true;
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
            yield return StartCoroutine(InitializeScene(targetScene, targetId));
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
            // yield return SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
            // yield return new WaitForSecondsRealtime(1); // allow things to initialize
            SceneManager.LoadScene(targetScene);
            yield return null;
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
            else
            {
                print("failed");
            }
            
            // spawn bodies
            foreach (DeathInfo deathInfo in _deathInfo)
            {
                if (deathInfo.SceneName == targetScene)
                {
                    GameObject instance = Instantiate(deathModel, deathInfo.Position, Quaternion.Euler(0, Random.Range(0, 360), 0));
                    instance.transform.localScale = player.transform.lossyScale;
                }
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

        public class DeathInfo
        {
            public string SceneName;
            public Vector3 Position;
        }
    }
}
