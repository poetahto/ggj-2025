using UnityEngine;

namespace DefaultNamespace
{
    public class EnergyMaterial : MonoBehaviour
    {
        private static readonly int Energy = Shader.PropertyToID("_Energy");
        
        public Material material;
        public string energyId;
        public float maxValue = 1;
        public float minValue;

        private void Update()
        {
            float energy = GlobalState.GetInstance().Floats[energyId];
            float t = Mathf.InverseLerp(minValue, maxValue, energy);
            material.SetFloat(Energy, Mathf.Clamp01(t));
        }
    }
}
