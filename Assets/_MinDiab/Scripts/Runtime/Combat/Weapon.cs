﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.MinDiab.Combat
{
    [Serializable]
    [CreateAssetMenu(fileName = "Weapon", menuName = "Wizards Code/MinDiab/New Weapon")]
    public class Weapon : ScriptableObject
    {
        public enum Handedness { Dominant, NonDominant, BothDominantLead, BothNonDominantLead }

        [SerializeField, Tooltip("The weapon to equip.")]
        internal GameObject Prefab = null;
        [SerializeField, Tooltip("The range we need to be within in order to attack.")]
        float m_Range = 2;
        [SerializeField, Tooltip("The time between attacks in seconds.")]
        internal float TimeBetweenAttacks = 1;
        [SerializeField, Tooltip("The amount of damage to do on each hit.")]
        internal int Damage = 10;
        [SerializeField, Tooltip("The animation controller override to use when the weapon is equipped.")]
        internal AnimatorOverrideController AnimationController;
        [SerializeField, Tooltip("Indicate whether this is a Dominant, Non-Dominant, or both hands. In the case of both hands the dominant and non-dominant versions will indicate which hand the model will be placed in.")]
        internal Handedness Hand = Handedness.Dominant;

        float? rangeSqr = null;
        internal float WeaponRangeSqr { 
            get
            {
                if (rangeSqr == null)
                {
                    rangeSqr = m_Range * m_Range;
                }
                return (float)rangeSqr;
            }
        }
    }
}