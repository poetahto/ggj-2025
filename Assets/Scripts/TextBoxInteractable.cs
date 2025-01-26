using UnityEngine;

namespace DefaultNamespace
{
    public class TextBoxInteractable : MonoBehaviour
    {
        public string[] lines;

        public int CurrentLine { get; set; } = -1;

        public void HandleInteract()
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
                textBox.SetText(lines[CurrentLine]);
            }
        }
    }
}
