using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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

        private void OnTriggerEnter(Collider other)
        {
            if (!string.IsNullOrEmpty(m_DestinationScene))
            {
                if (other.CompareTag("Player"))
                {
                    StartCoroutine(TransitionToScene(m_DestinationScene));
                }
            }
        }

        private IEnumerator TransitionToScene(string sceneName)
        {
            DontDestroyOnLoad(gameObject);

            yield return SceneManager.LoadSceneAsync(sceneName);

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

            yield return null;

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
