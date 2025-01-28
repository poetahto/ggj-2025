using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class Conversation : MonoBehaviour
    {
        [Serializable]
        public class Moment
        {
            public int characterId;
            public string[] lines;
            public float postDelay;
        }
        
        public string[] characters;
        public Moment[] moments;
        public float preDelay;
        
        public IEnumerator Start()
        {
            GlobalState state = GlobalState.GetInstance();
            TextBox textBox = state.textBox;
            yield return new WaitForSeconds(preDelay);
            
            foreach (Moment moment in moments)
            {
                textBox.SetText($"Incoming transmission from:\n<u>{characters[moment.characterId]}</u>\n", 5);
                yield return null;
                yield return new WaitUntil(() => !textBox.IsRunning || InputSystem.actions["Interact"].WasPressedThisFrame());
                
                foreach (string line in moment.lines)
                {
                    textBox.SetText($"{line}", 5);
                    yield return null;
                    yield return new WaitUntil(() => !textBox.IsRunning || InputSystem.actions["Interact"].WasPressedThisFrame());
                }

                yield return new WaitForSeconds(moment.postDelay);
            }
        }
    }
}
