using System;
using UnityEngine;

namespace Scripts.UI
{
    public class HealthBarPresenter : BasePresenter<HealthBarView, HealthBarModel>
    {
        public static Action<Unit> Update;

        public Team team;

        private void OnEnable()
        {
            Update += OnUpdate;
        }

        private void OnDisable()
        {
            Update -= OnUpdate;
        }

        public void OnUpdate(Unit unit)
        {
            if (unit.team != team) return;

            Model.SetUnitName(unit.DisplayName);
            Model.SetHealth(unit.Health, unit.data.health);

            UpdateUI();
        }

        private void UpdateUI()
        {
            View.UpdateUI(Model.Health, Model.MaxHealth, Model.UnitName);
        }
    }
}
