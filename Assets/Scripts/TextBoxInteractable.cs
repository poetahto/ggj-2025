using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class TextBoxInteractable : MonoBehaviour
    {
        public float timeBetweenLines = 1.0f;
        public string[] lines;

        private float _elapsed;

        public int CurrentLine { get; set; } = -1;

        private void Update()
        {
            if (CurrentLine >= 0 && !GlobalState.GetInstance().textBox.IsRunning)
            {
                // _elapsed += Time.deltaTime;
                //
                // if (_elapsed >= timeBetweenLines)
                    AdvanceText();
            }
        }

        private void AdvanceText()
        { 
            TextBox textBox = GlobalState.GetInstance().textBox;
            CurrentLine++;
            _elapsed = 0;

            if (CurrentLine >= lines.Length)
            {
                CurrentLine = -1;
                textBox.Hide();
            }
            else
            {
                textBox.SetText(lines[CurrentLine], timeBetweenLines);
            }
        }

        public void HandleInteract()
        {
            AdvanceText();
        }
    }
}
