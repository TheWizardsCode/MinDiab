using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Character;

namespace WizardsCode.MinDiab.Combat
{
    public class CombatTarget : MonoBehaviour
    {

        HealthController health;

        public bool IsAlive { 
            get
            {
                return !health.IsDead;
            }
        }

        private void Start()
        {
            health = GetComponent<HealthController>();
        }

        internal void TakeDamage(int damage, Fighter source)
        {
            health.TakeDamage(damage, source);
        }
    }
}
