using UnityEngine;

namespace DefaultNamespace
{
    public class AutoTextBox : MonoBehaviour
    {
        public string line;
        public float duration;
        
        private void Start()
        {
            GlobalState.GetInstance().textBox.SetText(line, duration);
        }
    }
}
