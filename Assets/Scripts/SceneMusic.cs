using System;
using FMODUnity;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu]
    public class SceneMusic : ScriptableObject
    {
        [Serializable]
        public class Info
        {
            public string sceneName;
            public EventReference eventReference;
            public string parameterId;
            public float parameterValue;
        }

        public Info[] info;

        public Info GetInfo(string sceneName)
        {
            foreach (Info i in info)
            {
                if (i.sceneName == sceneName)
                    return i;
            }

            return null;
        }
    }
}
