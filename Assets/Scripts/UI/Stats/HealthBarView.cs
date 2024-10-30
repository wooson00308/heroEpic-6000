using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class HealthBarView : BaseView
    {
        public bool _isAlawysEnable = false;
        public float _disableDelayTime;

        private bool _isRunningProcessDisable = false;

        private float _currentDisableDelayTime;
        public enum Texts
        {
            UnitName_Text,
        }

        public enum Images
        {
            HealthBar_Image
        }
        public enum GameObjects
        {
            Model_GameObject
        }

        private void OnEnable()
        {
            BindUI();

            Get<GameObject>((int)GameObjects.Model_GameObject).SetActive(_isAlawysEnable);
        }

        public override void BindUI()
        {
            Bind<TextMeshProUGUI>(typeof(Texts));
            Bind<GameObject>(typeof(GameObjects));
            Bind<Image>(typeof(Images));
        }

        public void UpdateUI(int health, int maxHealth, string unitName)
        {
            Get<GameObject>((int)GameObjects.Model_GameObject).SetActive(true);

            Get<TextMeshProUGUI>((int)Texts.UnitName_Text).SetText($"{unitName}");
            Get<Image>((int)Images.HealthBar_Image).fillAmount = (float)health / maxHealth;

            if (!_isRunningProcessDisable)
            {
                if (!_isAlawysEnable)
                {
                    StartCoroutine(ProcessDisable());
                }
            }
            else
            {
                _currentDisableDelayTime = 0;
            }
        }

        private IEnumerator ProcessDisable()
        {
            if (_isRunningProcessDisable) yield break;
            _isRunningProcessDisable = true;

            _currentDisableDelayTime = 0;

            while (_currentDisableDelayTime < _disableDelayTime)
            {
                _currentDisableDelayTime += Time.deltaTime;
                yield return null;
            }

            Get<GameObject>((int)GameObjects.Model_GameObject).SetActive(false);

            _isRunningProcessDisable = false;
        }
    }
}
