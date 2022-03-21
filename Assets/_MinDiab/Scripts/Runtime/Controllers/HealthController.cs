using System;
using UnityEngine;
using UnityEngine.Events;
using WizardsCode.MinDiab.Combat;
using WizardsCode.MinDiab.Configuration;
using WizardsCode.MinDiab.Controller;
using WizardsCode.MinDiab.Core;
using WizardsCode.MinDiab.Stats;
using WizardsCode.MinDiab.UI;

namespace WizardsCode.MinDiab.Character
{
    public class HealthController : MonoBehaviour, ISaveable, IRaycastable
    {
        [SerializeField, Tooltip("The UI element to display the current health.")]
        BaseHUDElement healthHUDElement;

        [Header("Events")]
        [SerializeField, Tooltip("Event fired whenever the character heals. The parameter is the amount of heling recieved.")]
        public ChangeHealth m_OnHeal;
        [SerializeField, Tooltip("Event fired whenever the character takes damage. The parameter is the amount of damage taken.")]
        public ChangeHealth m_OnTakeDamage;
        [SerializeField, Tooltip("Event fired when the character dies.")]
        public UnityEvent m_OnDie;

        [Serializable]
        public class ChangeHealth : UnityEvent<float> { };

        CharacterRoleController controller;
        CharacterFeedbackManager feedback;

        private float currentHealth;
        private float MaxHealth;

        private void Awake()
        {
            controller = GetComponent<CharacterRoleController>();
            feedback = GetComponentInChildren<CharacterFeedbackManager>();
        }
        private void Start()
        {
            Health = controller.GetStat(Stat.Health);
            MaxHealth = Health;
        }

        internal float Health {
            get { return currentHealth; } 
            set
            {
                if (currentHealth != value) {
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


                if (value < 0)
                {
                    m_OnTakeDamage.Invoke(value);
                } else if (value > 0)
                {
                    m_OnHeal.Invoke(value);
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

        public CursorType CursorType { get { return CursorType.Attack; } }

        private void OnEnable()
        {
            if (controller.experience)
            {
                controller.experience.onLevelUp += OnLevelUp;
            }
        }

        private void OnDisable()
        {
            if (controller.experience)
            {
                controller.experience.onLevelUp -= OnLevelUp;
            }
        }

        private void OnLevelUp()
        {
            currentHealth = Mathf.Max(MaxHealth * 0.7f, currentHealth);
        }
        internal void AddHealth(float health)
        {
            if (!IsDead)
            {
                Health += health;
            }
        }

        public void TakeDamage(float damage, Fighter source)
        {
            if (!IsDead)
            {
                mostRecentDamageSource = source;
                Health -= damage; 
                controller.AlertNearbyAI();
            }
        }

        void Die(Fighter source)
        {
            m_OnDie.Invoke();

            controller.animator.SetTrigger(AnimationParameters.DefaultDieTriggerID);
            controller.scheduler.StopAction();

            if (source != null)
            {
                CharacterRoleController sourceController = source.GetComponent<CharacterRoleController>();
                sourceController.GetComponent<CharacterRoleController>().AddExperience(controller.GetStat(Stat.ExperienceRewardOnKill));
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

        public bool HandleRaycast(CharacterRoleController controller)
        {
            if (controller.fighter == null) return false;
            if (controller.gameObject == gameObject) return false;

            // TODO: Only allow clicking on an item within a certain NavMesh path distance?
            if (!controller.fighter.CanAttack(this)) return false;

            if (Input.GetMouseButton(0))
            {
                controller.fighter.Attack(this);
            }
            return true;
        }

        [Serializable]
        private struct SaveData
        {
            public float currentHealth;
            public float maxHealth;
        }
    }
}
