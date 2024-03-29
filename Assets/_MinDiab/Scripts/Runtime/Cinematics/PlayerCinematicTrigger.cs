﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using WizardsCode.MinDiab.Controller;
using WizardsCode.MinDiab.Core;

namespace WizardsCode.MinDiab.Cinematics
{
    public class PlayerCinematicTrigger : MonoBehaviour, ISaveable
    {

        Scheduler playerScheduler;
        CharacterRoleController playerController;
        PlayableDirector director;
        bool hasPlayed = false;

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerController = player.GetComponent<CharacterRoleController>();
            playerScheduler = player.GetComponent<Scheduler>();
        }

        private void Start()
        {
            TimelineAsset timelineAsset = director.playableAsset as TimelineAsset;
            IEnumerable<TrackAsset> trackList = timelineAsset.GetOutputTracks();
            foreach (TrackAsset track in trackList)
            {
                if (track.name == "Cinemachine Track")
                {
                    director.SetGenericBinding(track, Camera.main.GetComponent<CinemachineBrain>());
                }
            }
        }

        private void OnEnable()
        {
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
            playerScheduler.StopAction();
            playerController.enabled = false;
        }

        void EnableControl(PlayableDirector director)
        {
            playerController.enabled = true;
        }

        public object CaptureState()
        {
            return hasPlayed;
        }

        public void RestoreState(object state)
        {
            hasPlayed = (bool)state;
        }
    }
}
