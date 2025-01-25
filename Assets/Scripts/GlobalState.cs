using UnityEngine;

namespace DefaultNamespace
{
    public class GlobalState : MonoBehaviour
    {
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
        
        public float CurrentEnergy { get; set; }
        // todo: track respawn scene + id
        // todo: respawn logic
        // todo: warp logic
        // todo: track all death scenes + position
    }
}
