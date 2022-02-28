using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using WizardsCode.MinDiab.Cinematics;
using WizardsCode.MinDiab.Controller;
using WizardsCode.MinDiab.Core;

namespace WizardsCode.MinDiab.SceneManagement
{
    public class ScenePortal : MonoBehaviour
    {
        [SerializeField, Tooltip("The name of scene that can enter at this portal point. If left empty this is an exit only portal.")]
        string m_ArrivalFromScene;
        [SerializeField, Tooltip("The name of the scene to load when this portal is triggered. If left empty this is an entry only portal.")]
        string m_DestinationScene;
        [SerializeField, Tooltip("The point at which items coming through this portal will be spawned in this world.")]
        Transform m_SpawnPoint;
        [SerializeField, Tooltip("The time taken to fade out when changing scenes.")]
        float m_FadeOutTime = 2;
        [SerializeField, Tooltip("The time to pause between fade in and out after the scene has loaded.")]
        float m_FadePauseTime = 1;
        [SerializeField, Tooltip("The time taken to fade back in when the scene has loaded.")]
        float m_FadeInTime = 1;


        Fader fadeToBlack;
        SavingWrapper saveWrapper;

        private void Awake()
        {
            fadeToBlack = GameObject.FindObjectOfType<Fader>();
            saveWrapper = GameObject.FindObjectOfType<SavingWrapper>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!string.IsNullOrEmpty(m_DestinationScene))
            {
                PlayerController player = other.GetComponent<PlayerController>();
                if (player)
                {
                    player.StopAllActions();
                    StartCoroutine(TransitionToScene(m_DestinationScene));
                }
            }
        }

        private IEnumerator TransitionToScene(string sceneName)
        {
            yield return fadeToBlack.FadeOut(m_FadeOutTime);
            saveWrapper.Save();

            DontDestroyOnLoad(gameObject);

            yield return SceneManager.LoadSceneAsync(sceneName);

            yield return null;

            saveWrapper.Load();

            ScenePortal otherPortal = GetOtherPortal();
            if (otherPortal)
            {
                GameObject player = GameObject.FindWithTag("Player");
                player.GetComponent<NavMeshAgent>().Warp(otherPortal.m_SpawnPoint.position);
                player.transform.rotation = otherPortal.m_SpawnPoint.rotation;
            } else
            {
                Debug.LogError($"Unable to find the spawn point when travelling through {this}.");
            }

            // OPTIMIZATION: this save is to ensure we have the right position for the player when the load back into a scene, we should find a better way of doing this.
            saveWrapper.Save();

            yield return new WaitForSeconds(m_FadePauseTime);

            yield return fadeToBlack.FadeIn(m_FadeInTime);

            Destroy(gameObject);
        }

        /// <summary>
        /// Get the other portal in the scene that has an destination of the scene containing this portal.
        /// </summary>
        private ScenePortal GetOtherPortal()
        {
            ScenePortal[] portals = FindObjectsOfType<ScenePortal>();
            for (int i = 0; i < portals.Length; i++)
            {
                if (portals[i] == this) continue;

                if (m_DestinationScene == SceneManager.GetActiveScene().name)
                {
                    return portals[i];
                }
            }
            return null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawRay(m_SpawnPoint.transform.position, m_SpawnPoint.transform.forward);
        }
    }
}
