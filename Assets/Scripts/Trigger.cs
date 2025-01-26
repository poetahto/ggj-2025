using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class Trigger : MonoBehaviour
    {
        public UnityEvent onTrigger;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                onTrigger.Invoke();
        }
    }
}
