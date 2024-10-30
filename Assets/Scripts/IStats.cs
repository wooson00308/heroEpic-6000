using UnityEngine;

namespace Scripts
{
    public interface IStats
    {
        public string DisplayName { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public float RunSpeed { get; set; }
    }
}

