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

        internal override void PickupItem(GameObject character)
        {
            if (!CanPickup(character)) return;

            if (CanPickup(character))
            {
                character.GetComponent<Fighter>().EquipWeapon(m_EquipableWeapon);
            }

            base.PickupItem(character);
        }
    }
}