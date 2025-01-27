using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class ProgressAdvancer : MonoBehaviour
    {
        public int progressCount;
        public UnityEvent onAdvance;
        
        public void Advance()
        {
            GlobalState state = GlobalState.GetInstance();

            if (state.ProgressCounter < progressCount)
            {
                state.ProgressCounter = progressCount;
                onAdvance.Invoke();
            }
        }
    }
}
