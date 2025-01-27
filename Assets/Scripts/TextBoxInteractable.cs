using UnityEngine;

namespace DefaultNamespace
{
    public class TextBoxInteractable : MonoBehaviour
    {
        [Multiline] public string[] lines;
        public float timeBetweenLines = 1.0f;

        public int CurrentLine { get; set; } = -1;

        private void Update()
        {
            if (CurrentLine >= 0 && !GlobalState.GetInstance().textBox.IsRunning)
                AdvanceText();
        }

        private void AdvanceText()
        { 
            TextBox textBox = GlobalState.GetInstance().textBox;
            CurrentLine++;

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
