using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.MinDiab.Core
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "Save";
        static bool alreadyExists = false;
        SavingSystem saveSystem;

        void Awake()
        {
            if (alreadyExists) return;

            saveSystem = GetComponent<SavingSystem>();
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        private void Load()
        {
            saveSystem.Load(defaultSaveFile);
        }
        private void Save()
        {
            saveSystem.Save(defaultSaveFile);
        }
    }
}