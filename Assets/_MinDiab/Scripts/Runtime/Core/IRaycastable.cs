using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Controller;

namespace WizardsCode.MinDiab.Core
{
    public interface IRaycastable
    {

        bool HandleRaycast(CharacterRoleController controller);

        CursorType CursorType { get; }
    }
}