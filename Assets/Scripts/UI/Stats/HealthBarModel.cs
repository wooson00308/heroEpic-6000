using UniRx;
using UnityEngine;

namespace Scripts.UI
{
    public class HealthBarModel : BaseModel
    {
        private int _health;
        public int Health => _health;

        private int _maxHealth;
        public int MaxHealth => _maxHealth;

        private string _unitName;
        public string UnitName => _unitName;

        public override void Initialize()
        {
            
        }

        public void SetHealth(int health, int maxHealth)
        {
            _health = health;
            _maxHealth = maxHealth;
        }

        public void SetUnitName(string name)
        {
            _unitName = name;
        }
    }
}


