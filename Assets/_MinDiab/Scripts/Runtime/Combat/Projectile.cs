using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Character;

namespace WizardsCode.MinDiab.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField, Tooltip("The base speed of the flight of the arrow.")]
        float m_Speed = 1;
        [SerializeField, Tooltip("Is this a homing projectile, that is does it follow its target?")]
        bool m_IsHoming = false;

        /// <summary>
        /// The Damage this projectile will do if it hits. This is set when the projectile is
        /// fired by calling the Launc(...) method.
        /// </summary>
        public float Damage
        {
            get; private set;
        }

        /// <summary>
        /// The Target this projectile attempt to hit. This is set when the project is
        /// fired by calling the Launc(...) method.
        /// </summary>
        public HealthController Target
        {
            get; private set;
        }

        void Update()
        {
            if (!Target) return;

            if (m_IsHoming && !Target.IsDead)
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * m_Speed * Time.deltaTime);
        }

        /// <summary>
        /// Launch the projectile at a given target, doing a given amount of damage if it hits.
        /// </summary>
        /// <param name="target">The health controller of the target.</param>
        /// <param name="damage">The amount of damage to do.</param>
        internal void Launch(HealthController target, float damage)
        {
            Target = target;
            Damage = damage;
            transform.LookAt(GetAimLocation());
            Destroy(gameObject, 10);
        }

        Vector3 GetAimLocation()
        {
            CapsuleCollider collider = Target.GetComponent<CapsuleCollider>();
            
            if (!collider) return Target.transform.position;

            return Target.transform.position + (Target.transform.up * collider.height / 2);
        }

        private void OnTriggerEnter(Collider other)
        {
            HealthController hit = other.GetComponent<HealthController>();
            if (hit && !hit.IsDead)
            {
                DoDamage(hit);
            }
            m_Speed = 0;
        }

        private void DoDamage(HealthController hit) 
        {
            hit.TakeDamage(Damage);
        }
    }
}
