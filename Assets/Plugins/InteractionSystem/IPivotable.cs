using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractionSystem2D
{
    public interface IPivotable
    {
        Vector2 pickupPosition { get; }
    }
}
