using UnityEngine;

namespace Scripts
{
    public class AnimationEvents : MonoBehaviour
    {
        private Unit _unit;

        private void Awake()
        {
            _unit = GetComponentInParent<Unit>();
        }

        public void OnEvents(AnimationEvent e)
        {

        }
    }
}
