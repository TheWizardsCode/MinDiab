using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using WizardsCode.MinDiab.Controller;
using WizardsCode.MinDiab.Core;

namespace WizardsCode.MinDiab.Cinematics
{
    public class PlayerCinematicTrigger : MonoBehaviour
    {

        Scheduler playerScheduler;
        PlayerController playerController;
        PlayableDirector director;
        bool hasPlayed = false;

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerController = player.GetComponent<PlayerController>();
            playerScheduler = player.GetComponent<Scheduler>();
            director.played += DisableControl;
            director.stopped += EnableControl;
        }

        private void OnDisable()
        {
            director.played -= DisableControl;
            director.stopped -= EnableControl;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!hasPlayed)
            {
                if (other.CompareTag("Player"))
                {
                    director.Play();
                    hasPlayed = true;
                }
            }
        }

        void DisableControl(PlayableDirector director)
        {
            playerScheduler.StopCurrentAction();
            playerController.enabled = false;
        }

        void EnableControl(PlayableDirector director)
        {
            playerController.enabled = true;
        }
    }
}
