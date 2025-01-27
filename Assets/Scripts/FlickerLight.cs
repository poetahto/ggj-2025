using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickerLight : MonoBehaviour
{
    public Preset preset = Preset.Normal;
    public float speed = 10;

    private Light _light;
    private float _maxEnergy;
    private int _index;
    private float _cooldown;

    private void Start()
    {
        _light = GetComponent<Light>();
        _maxEnergy = _light.intensity;
    }

    private void Update()
    {
        if (_cooldown > 1.0f / speed)
        {
            const int min = 'a';
            const int max = 'z';
            const int range = max - min;
            
            _cooldown = 0;
            _index = (int)Mathf.Repeat(_index + 1, range);
            int cur = Presets[(int)preset][_index];
            _light.intensity = (cur - min) / (float)range * _maxEnergy;
        }
        else _cooldown += Time.deltaTime;
    }
    
    public enum Preset
    {
        Normal,
        Flicker,
        SlowStrongPulse,
        Candle1,
        FastStrobe,
        GentlePulse1,
        Flicker2,
        Candle2,
        Candle3,
        SlowStrobe,
        FluorescentFlicker,
        SlowPulseNoFadeToBlack,
    }
    
    private readonly string[] Presets = new[] {
        "m",
        "mmnmmommommnonmmonqnmmo",
        "abcdefghijklmnopqrstuvwxyzyxwvutsrqponmlkjihgfedcba",
        "mmmmmaaaaammmmmaaaaaabcdefgabcdefg",
        "mamamamamama",
        "jklmnopqrstuvwxyzyxwvutsrqponmlkj",
        "nmonqnmomnmomomno",
        "mmmaaaabcdefgmmmmaaaammmaamm",
        "mmmaaammmaaammmabcdefaaaammmmabcdefmmmaaaa",
        "aaaaaaaazzzzzzzz",
        "mmamammmmammamamaaamammma",
        "abcdefghijklmnopqrrqponmlkjihgfedcba",
    };
}
