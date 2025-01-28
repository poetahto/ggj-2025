using System.Collections;
using FMODUnity;
using TMPro;
using UnityEngine;

public class TextAnimationPlayer : MonoBehaviour
{
    public TextAnimation textAnimation;
    public TMP_Text text;
    public EventReference chirp;

    private IEnumerator Start()
    {
        foreach (TextAnimation.PressEvent pressEvent in textAnimation.events)
        {
            if (pressEvent.value == '\0')
                continue;
            
            string t = text.text;
            t += pressEvent.value;
            text.SetText(t);
            if (pressEvent.value != '\n')
                RuntimeManager.PlayOneShot(chirp);
            yield return new WaitForSeconds(pressEvent.delay);
        }
    }
}
