using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Cinematics;

namespace WizardsCode.MinDiab.Core
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "Save";
        static bool alreadyExists = false;
        SavingSystem saveSystem;
        Fader fadeToBlack;

        void Awake()
        {
            if (alreadyExists) return;

            fadeToBlack = GameObject.FindObjectOfType<Fader>();
            saveSystem = GetComponent<SavingSystem>();
            DontDestroyOnLoad(gameObject);
        }
        IEnumerator Start()
        {
            yield return fadeToBlack.FadeOut(0.25f);
            yield return saveSystem.LoadLastScene(defaultSaveFile);
            yield return new WaitForSeconds(2);
            yield return fadeToBlack.FadeIn(0.25f);
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

            if (Input.GetKeyDown(KeyCode.Minus))
            {
                Save();
            }
        }

        public void Load()
        {
            saveSystem.Load(defaultSaveFile);
        }
        public void Save()
        {
            saveSystem.Save(defaultSaveFile);
        }

        private void Delete()
        {
            saveSystem.Delete(defaultSaveFile);
        }
    }
}