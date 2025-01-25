using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class AmbientEnergyUsage : MonoBehaviour
    {
        public float ambientUsage;
        public string energyId;
        
        private void Update()
        {
            GlobalState state = GlobalState.GetInstance();
            state.Floats[energyId] -= ambientUsage * Time.deltaTime;
            state.Floats[energyId] = Math.Max(state.Floats[energyId], 0);
        }
    }
}
