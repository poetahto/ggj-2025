﻿using UnityEngine;

namespace DefaultNamespace
{
    public class ProgressEnabler : MonoBehaviour
    {
        public int minimumProgress;
        public bool isActive;

        private void Awake()
        {
            gameObject.SetActive(GlobalState.GetInstance().ProgressCounter >= minimumProgress ? isActive : !isActive);
        }
    }
}
