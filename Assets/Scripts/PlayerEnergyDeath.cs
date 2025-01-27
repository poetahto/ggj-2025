using System.Collections;
using FMODUnity;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerEnergyDeath : MonoBehaviour
    {
        public float pauseDurationPostDeath = 1;
        public GameObject[] bubbles;

        private GameObject _popEffectPrefab;
        
        private void Start()
        {
            GlobalState state = GlobalState.GetInstance();
            state.OnUseEnergy += HandleUseEnergy;
            state.OnRefillEnergy += HandleRefillEnergy;
            _popEffectPrefab = Resources.Load<GameObject>("PopEffect");
            
            for (int i = 0; i < Mathf.Clamp(5 - state.EnergyCount, 0, bubbles.Length); i++)
                bubbles[i].SetActive(false);
        }

        private void OnDestroy()
        {
            GlobalState state = GlobalState.GetInstance();
            state.OnUseEnergy -= HandleUseEnergy;
            state.OnRefillEnergy -= HandleRefillEnergy;
        }

        private void HandleRefillEnergy()
        {
            foreach (GameObject bubble in bubbles)
                bubble.SetActive(true);
        }

        private void HandleUseEnergy()
        {
            GlobalState state = GlobalState.GetInstance();
            TextBox textBox = state.textBox;

            switch (state.EnergyCount)
            {
                case 4:
                    Instantiate(_popEffectPrefab, bubbles[0].transform.position, Quaternion.identity);
                    bubbles[0].SetActive(false);
                    RuntimeManager.PlayOneShot("event:/Robot_Health_1");
                    break;
                case 3:
                    Instantiate(_popEffectPrefab, bubbles[1].transform.position, Quaternion.identity);
                    bubbles[1].SetActive(false);
                    RuntimeManager.PlayOneShot("event:/Robot_Health_2");
                    textBox.SetText("Energy: 50%", 1);
                    break;
                case 2:
                    Instantiate(_popEffectPrefab, bubbles[2].transform.position, Quaternion.identity);
                    bubbles[2].SetActive(false);
                    RuntimeManager.PlayOneShot("event:/Robot_Health_3");
                    break;
                case 1:
                    Instantiate(_popEffectPrefab, bubbles[3].transform.position, Quaternion.identity);
                    bubbles[3].SetActive(false);
                    RuntimeManager.PlayOneShot("event:/Robot_Health_4");
                    textBox.SetText("Energy: Critical", 1);
                    break;
                case <= 0:
                    StartCoroutine(DeathCoroutine());
                    break;
            }
        }

        private IEnumerator DeathCoroutine()
        {
            float remaining = pauseDurationPostDeath;
            RuntimeManager.PlayOneShot("event:/Robot_Death_Screen");

            while (remaining >= 0)
            {
                float t = 1 - remaining / pauseDurationPostDeath;
                Time.timeScale = Mathf.Lerp(1, 0, t);
                remaining -= Time.unscaledDeltaTime;
                Shader.SetGlobalFloat("_GlitchProgress", t);
                yield return null;
            }

            Time.timeScale = 1;
            Shader.SetGlobalFloat("_GlitchProgress", 0);
            GlobalState.GetInstance().Respawn();
        }
    }
}
