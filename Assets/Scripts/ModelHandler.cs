using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class ModelHandler : MonoBehaviour
    {
        public List<GameObject> attackColliders;
        public List<GameObject> hitColliders;

        /// <summary>
        /// int-> colliderIndex, string -> stringToBool
        /// </summary>
        /// <param name="e"></param>
        public void OnAttackEvent(AnimationEvent e)
        {
            attackColliders[e.intParameter].SetActive(bool.Parse(e.stringParameter));
        }

        public void OnPlaySFX(AnimationEvent e)
        {
            float volume = e.floatParameter == 0 ? 1 : e.floatParameter;
            AudioManager.Instance.PlaySFX(e.stringParameter, volume);
        }

        public void SetHitable(AnimationEvent e)
        {
            foreach(var coll in hitColliders)
            {
                coll.SetActive(bool.Parse(e.stringParameter));
            }
        }
    }
}

