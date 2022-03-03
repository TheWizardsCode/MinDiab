using System;
using UnityEngine;
using WizardsCode.MinDiab.Configuration;
using WizardsCode.MinDiab.Controller;
using WizardsCode.MinDiab.Core;
using WizardsCode.MinDiab.Stats;
using WizardsCode.MinDiab.UI;

namespace WizardsCode.MinDiab.Character
{
    public class HealthController : MonoBehaviour, ISaveable
    {
        [SerializeField, Tooltip("The UI element to display the current health.")]
        BaseHUDElement healthHUDElement;

        PlayerController player;

        private float currentHealth;
        private float MaxHealth;

        internal float Health {
            get { return currentHealth; } 
            set
            {
                currentHealth = Mathf.Max(value, 0);
                if (healthHUDElement)
                {
                    healthHUDElement.UpdateUINormalized(Normalized);
                }
                if (Health == 0)
                {
                    Die();
                }
            } 
        }

        internal float Normalized
        {
            get
            {
                return Mathf.Clamp01(Health / MaxHealth);
            }
        }

        public bool IsDead
        {
            get
            {
                return Health <= 0;
            }
        }

        private void Awake()
        {
            player = GetComponent<PlayerController>();
            Health = GetComponent<BaseStats>().Health;
            MaxHealth = GetComponent<BaseStats>().Health;
        }

        public void TakeDamage(float damage)
        {
            if (!IsDead)
            {
                Health = Health - damage;
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
            SaveData data = new SaveData();
            data.currentHealth = Health;
            data.maxHealth = MaxHealth;
            return data;
        }

        public void RestoreState(object state)
        {
            SaveData data = (SaveData)state;
            Health = data.currentHealth;
            MaxHealth = data.maxHealth;


            if (Health <= 0)
            {
                Die();
            }
        }

        [Serializable]
        private struct SaveData
        {
            public float currentHealth;
            public float maxHealth;
        }
    }
}
