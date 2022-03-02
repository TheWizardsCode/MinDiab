using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Configuration;
using WizardsCode.MinDiab.Controller;
using WizardsCode.MinDiab.Core;
using WizardsCode.MinDiab.Stats;

namespace WizardsCode.MinDiab.Character
{
    public class HealthController : MonoBehaviour, ISaveable
    {
        [SerializeField, Tooltip("The current health of this character in hitpoints.")]
        float m_HealthPoints = 100;

        PlayerController player;

        public bool IsDead
        {
            get
            {
                return m_HealthPoints <= 0;
            }
        }

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            m_HealthPoints = GetComponent<BaseStats>().Health;
        }

        private void Init()
        {
            player = GetComponent<PlayerController>();
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
            player.animator.SetTrigger(AnimationParameters.DefaultDieTriggerID);
            player.scheduler.StopCurrentAction();

            CapsuleCollider capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
            if (capsuleCollider) { capsuleCollider.enabled = false; }
        }

        public object CaptureState()
        {
            return m_HealthPoints;
        }

        public void RestoreState(object state)
        {
            m_HealthPoints = (float)state;

            if (m_HealthPoints <= 0)
            {
                
                Die();
            }
        }
    }
}
