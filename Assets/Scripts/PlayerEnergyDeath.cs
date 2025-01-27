using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace DefaultNamespace
{
    public class PlayerEnergyDeath : MonoBehaviour
    {
        public float pauseDurationPostDeath = 1;
        
        private void Start()
        {
            GlobalState state = GlobalState.GetInstance();
            state.OnUseEnergy += HandleUseEnergy;
        }

        private void OnDestroy()
        {
            GlobalState state = GlobalState.GetInstance();
            state.OnUseEnergy -= HandleUseEnergy;
        }

        private void HandleUseEnergy()
        {
            GlobalState state = GlobalState.GetInstance();
            TextBox textBox = state.textBox;

            switch (state.EnergyCount)
            {
                case 5:
                    textBox.SetText("Energy refilled.", 1);
                    break;
                case 3:
                    textBox.SetText("Energy: 75%", 1);
                    RuntimeManager.PlayOneShot("event:/Robot_Health_1");
                    break;
                case 2:
                    textBox.SetText("Energy: 50%", 1);
                    RuntimeManager.PlayOneShot("event:/Robot_Health_2");
                    break;
                case 1:
                    textBox.SetText("Energy: 25%", 1);
                    RuntimeManager.PlayOneShot("event:/Robot_Health_3");
                    break;
                case <= 0:
                    RuntimeManager.PlayOneShot("event:/Robot_Health_4");
                    textBox.SetText("Energy: Critical", 1);
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

            Shader.SetGlobalFloat("_GlitchProgress", 0);
            GlobalState.GetInstance().Respawn();
        }
    }
}
