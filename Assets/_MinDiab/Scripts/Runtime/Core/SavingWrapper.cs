using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WizardsCode.MinDiab.Cinematics;

namespace WizardsCode.MinDiab.Core
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField, Tooltip("The Save System that will manage loading and saving of the game.")]
        SavingSystem m_SaveSystem;
        [SerializeField, Tooltip("The screen fader effect to use when loading.")]
        Fader m_FadeToBlack;

        const string defaultSaveFile = "Save";
        const string defaultScene = "Sandbox Level 1";
        static bool alreadyLoaded = false;
        
        void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        IEnumerator LoadLastScene()
        {
            if (alreadyLoaded) yield break;
            alreadyLoaded = true;

            yield return m_FadeToBlack.FadeOut(0.25f);
            if (m_SaveSystem.Exists(defaultSaveFile))
            {
                yield return m_SaveSystem.LoadLastScene(defaultSaveFile);
            } else
            {
                yield return SceneManager.LoadSceneAsync(defaultScene);
            }
            yield return new WaitForSeconds(2);
            yield return m_FadeToBlack.FadeIn(0.25f);
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
                Delete();
            }
        }

        public void Load()
        {
            m_SaveSystem.Load(defaultSaveFile);
        }
        public void Save()
        {
            m_SaveSystem.Save(defaultSaveFile);
        }

        private void Delete()
        {
            m_SaveSystem.Delete(defaultSaveFile);
        }
    }
}