using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
                    break;
                case 2:
                    textBox.SetText("Energy: 50%", 1);
                    break;
                case 1:
                    textBox.SetText("Energy: 25%", 1);
                    break;
                case <= 0:
                    textBox.SetText("Energy: Critical", 1);
                    StartCoroutine(DeathCoroutine());
                    break;
            }
        }

        private IEnumerator DeathCoroutine()
        {
            float remaining = pauseDurationPostDeath;

            while (remaining >= 0)
            {
                float t = 1 - remaining / pauseDurationPostDeath;
                Time.timeScale = Mathf.Lerp(1, 0, t);
                remaining -= Time.unscaledDeltaTime;
                yield return null;
            }
            
            GlobalState.GetInstance().Respawn();
        }
    }
}
