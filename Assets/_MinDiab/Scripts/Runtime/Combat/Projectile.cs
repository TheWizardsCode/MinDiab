using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WizardsCode.MinDiab.Character;
using WizardsCode.MinDiab.FX;

namespace WizardsCode.MinDiab.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField, Tooltip("The base speed of the flight of the arrow.")]
        float m_Speed = 1;
        [SerializeField, Tooltip("Is this a homing projectile, that is does it follow its target?")]
        bool m_IsHoming = false;
        [SerializeField, Tooltip("An effect object to instantiate when the projectile hits something. If null this will be ignored.")]
        DestroyAfterParticles m_HitEffect;
        [SerializeField, Tooltip("Should the projectile be destroyed on impact? If false it will be destroyed after a period of time (see below)")]
        bool m_DestroyOnImpact = false;
        [SerializeField, Tooltip("How long should the projectile live if it does not make contact with something. If destroyOnImpact is true then the projectile may not live this long.")]
        float m_TimeToLive = 10;

        [Header("Events")]
        [SerializeField, Tooltip("Events to fire when this projectile is launched.")]
        UnityEvent m_OnLaunch;
        [SerializeField, Tooltip("Events to fire when this projectile hits something..")]
        UnityEvent m_OnHit;

        /// <summary>
        /// The Damage this projectile will do if it hits. This is set when the projectile is
        /// fired by calling the Launc(...) method.
        /// </summary>
        public float Damage
        {
            get; private set;
        }
        public Fighter Source { get; private set; }

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
        internal void Launch(HealthController target, float damage, Fighter source)
        {
            m_OnLaunch.Invoke();
            Target = target;
            Damage = damage;
            Source = source;
            transform.LookAt(GetAimLocation());
            Destroy(gameObject, m_TimeToLive);
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
            if (hit.gameObject == Source.gameObject)
            {
                return;
            }

            if (hit)
            {
                m_OnHit.Invoke();
                hit.TakeDamage(Damage, Source);
            }

            if (m_HitEffect)
            {
                GameObject go = Instantiate(m_HitEffect, transform.position, transform.rotation).gameObject;
                Destroy(go, 5);
            }

            if (m_DestroyOnImpact)
            {
                Destroy(gameObject);
            }
            else
            {
                GetComponent<Collider>().enabled = false;
                transform.SetParent(hit.transform);
                m_Speed = 0;
            }
        }
    }
}
