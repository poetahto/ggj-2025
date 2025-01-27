using UnityEngine;

namespace DefaultNamespace
{
    public class ProgressAdvancer : MonoBehaviour
    {
        public int progressCount;
        
        public void Advance()
        {
            GlobalState state = GlobalState.GetInstance();
            
            if (state.ProgressCounter < progressCount)
                state.ProgressCounter = progressCount;
        }
    }
}
