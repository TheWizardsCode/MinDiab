using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.MinDiab.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField, Tooltip("The weapon definition that this pickup represents.")]
        Weapon m_EquipableWeapon;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(m_EquipableWeapon);
                Destroy(gameObject);
            }
        }
    }
}