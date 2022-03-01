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

        public HealthController Target
        {
            get; set;
        }

        void Update()
        {
            if (!Target) return;

            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * m_Speed * Time.deltaTime);
        }

        Vector3 GetAimLocation()
        {
            CapsuleCollider collider = Target.GetComponent<CapsuleCollider>();
            
            if (!collider) return Target.transform.position;

            return Target.transform.position + (Target.transform.up * collider.height / 2);
        }
    }
}
