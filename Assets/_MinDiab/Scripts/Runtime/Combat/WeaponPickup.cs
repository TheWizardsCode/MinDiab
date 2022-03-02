using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Core;

namespace WizardsCode.MinDiab.Combat
{
    public class WeaponPickup : Pickup
    {
        [SerializeField, Tooltip("The weapon definition that this pickup represents.")]
        Weapon m_EquipableWeapon;

        internal override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            if (CanPickup(other))
            {
                other.GetComponent<Fighter>().EquipWeapon(m_EquipableWeapon);
            }
        }
    }
}