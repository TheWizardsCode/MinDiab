using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.MinDiab.Stats
{
    public interface IStatModifierProvider
    {
        /// <summary>
        /// Get any additive modifiers for a specific stat. These will be added
        /// to the stat before multiplication. The additive value could be negative.
        /// </summary>
        /// <param name="stat">The stat to get the additive value for</param>
        /// <returns>Returns a number to be added to the stat before the multiplier is applied.</returns>
        IEnumerable<float> GetStatAdditiveModifier(Stat stat);

        /// <summary>
        /// Get the multiplier to apply to a specific stat. This will be applied
        /// after additive values have been added. The multiplier can be less than
        /// one resulting in a reduction of the stat.
        /// </summary>
        /// <param name="stat">The stat to get the multiplier for.</param>
        /// <returns>The multiplier to apply after the stats additive value has been applied.</returns>
        IEnumerable<float> GetStatMultiplier(Stat stat);
    }
}
