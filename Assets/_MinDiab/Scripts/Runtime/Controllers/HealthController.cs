using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Configuration;

namespace WizardsCode.MinDiab.Character
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField, Tooltip("The current health of this character in hitpoints.")]
        float m_HealthPoints = 100;

        Animator animator;

        public bool IsDead
        {
            get
            {
                return m_HealthPoints <= 0;
            }
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            if (!IsDead)
            {
                m_HealthPoints = Mathf.Max(m_HealthPoints - damage, 0);
                if (m_HealthPoints == 0)
                {
                    Die();
                }
            }
        }

        void Die()
        {
            animator.SetTrigger(AnimationParameters.DefaultDieTriggerID);
        }
    }
}
