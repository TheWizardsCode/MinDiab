using System;
using UnityEngine;
using WizardsCode.MinDiab.Combat;
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

        CharacterRoleController controller;

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
                    Die(mostRecentDamageSource);
                }
            } 
        }

        private Fighter mostRecentDamageSource;

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
            controller = GetComponent<CharacterRoleController>();
            Health = controller.GetStat(Stat.Health);
            MaxHealth = Health;
        }

        public void TakeDamage(float damage, Fighter source)
        {
            if (!IsDead)
            {
                mostRecentDamageSource = source;
                Health = Health - damage;
            }
        }

        void Die(Fighter source)
        {
            controller.animator.SetTrigger(AnimationParameters.DefaultDieTriggerID);
            controller.scheduler.StopCurrentAction();

            if (source != null)
            {
                CharacterRoleController sourceController = source.GetComponent<CharacterRoleController>();
                sourceController.GetComponent<CharacterRoleController>().AddExperience(controller.GetStat(Stat.ExperienceReward));
            }

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
                Die(null);
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
