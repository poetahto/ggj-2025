using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TextAnimationRecorder : MonoBehaviour
{
    public TextAnimation output;
    public TMP_Text preview;
    public EventReference chirpPreview;
    
    private char _prevChar;
    private float _prevTime;
    private Keyboard _keyboard;
    
    private void OnEnable()
    {
        output.events.Clear();
        _keyboard = InputSystem.GetDevice<Keyboard>();
        _keyboard.onTextInput += HandleTextInput;
    }

    private void OnDisable()
    {
        _keyboard.onTextInput -= HandleTextInput;
        
        TextAnimation.PressEvent pressEvent = new TextAnimation.PressEvent {
            value = _prevChar,
            delay = 1,
        };
        
        output.events.Add(pressEvent);
    }

    private void Update()
    {
        // if (_keyboard.enterKey.wasPressedThisFrame)
        //     HandleTextInput('\n');
    }

    private void HandleTextInput(char value)
    {
        if (value == '\r') value = '\n';
        preview.text += value;
        
        TextAnimation.PressEvent pressEvent = new TextAnimation.PressEvent {
            value = _prevChar,
            delay = Time.time - _prevTime,
        };
        
        if (value != '\n')
            RuntimeManager.PlayOneShot(chirpPreview);
        
        _prevChar = value;
        _prevTime = Time.time;
        output.events.Add(pressEvent);
    }
}
