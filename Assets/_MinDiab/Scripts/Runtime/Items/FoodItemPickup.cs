using UnityEngine;
using WizardsCode.MinDiab.Character;
using WizardsCode.MinDiab.Core;

namespace WizardsCode.Core.Item
{
    public class FoodItemPickup : BasePickup
    {
        [SerializeField, Tooltip("The amount of health restored when this item is picked up.")]
        float m_HealthRestored = 10;
        private HealthController controller;

        internal override void PickupItem(GameObject character)
        {
            if (!CanPickup(character)) return;

            controller.AddHealth(m_HealthRestored);

            base.PickupItem(character);
        }

        /// <summary>
        /// Check this character can pick up the 
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        internal override bool CanPickup(GameObject character)
        {
            if (controller) return true;
            if (!character.CompareTag("Player")) return false;

            controller = character.GetComponent<HealthController>();
            return controller;
        }
    }
}
