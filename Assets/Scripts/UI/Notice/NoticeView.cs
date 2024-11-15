using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class NoticeView : BaseView
    {
        public int _durationTime;

        private bool _isHide;

        public enum Texts
        {
            Notice_Text,
            Notice_Amount_Text
        }

        private void Awake()
        {
            BindUI();
        }

        private void OnEnable()
        {
            _isHide = false;

            StartCoroutine(ProcessView());
        }

        public override void BindUI()
        {
            Bind<TextMeshProUGUI>(typeof(Texts));
        }

        public void UpdateUI(NoticeModel model)
        {
            Get<TextMeshProUGUI>((int)Texts.Notice_Text).SetText($"{model.NoticeText}");
            Get<TextMeshProUGUI>((int)Texts.Notice_Amount_Text).SetText($"{model.NoticeAmountText}");
        }

        public void Hide()
        {
            _isHide = true;
        }

        private IEnumerator ProcessView()
        {
            float time = 0;

            while(time < _durationTime)
            {
                if (_isHide) break;

                time += Time.deltaTime;

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}

