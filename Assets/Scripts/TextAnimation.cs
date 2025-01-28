using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TextAnimation : ScriptableObject
{
    [Serializable]
    public struct PressEvent
    {
        public char value;
        public float delay;
    }

    public List<PressEvent> events = new List<PressEvent>();
}
