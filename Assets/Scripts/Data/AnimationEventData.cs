using UnityEngine;

namespace Scripts
{
    public abstract class AnimationEventData : ScriptableObject
    {
        public abstract void OnEvent(Unit unit);
    }
}
