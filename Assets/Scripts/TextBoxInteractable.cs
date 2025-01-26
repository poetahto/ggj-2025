using UnityEngine;

namespace DefaultNamespace
{
    public class TextBoxInteractable : MonoBehaviour
    {
        public TextBox textBox;
        public string[] lines;

        public int CurrentLine { get; set; } = -1;

        public void HandleInteract()
        {
            CurrentLine++;

            if (CurrentLine >= lines.Length)
            {
                CurrentLine = -1;
                textBox.Hide();
            }
            else
            {
                textBox.SetText(lines[CurrentLine]);
            }
        }
    }
}
