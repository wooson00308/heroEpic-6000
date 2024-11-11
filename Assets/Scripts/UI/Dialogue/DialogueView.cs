using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class DialogueView : BaseView
    {
        public event Action OnNextDialogueRequest; // 다음 대화 요청 이벤트

        public float _maxDisplayDuration = 5f;
        private bool _isUpdatingView;


        private void OnEnable()
        {
            BindUI();
            SetActive(false);
        }

        public void OnNextDialogue()
        {
            if (!Get<GameObject>((int)GameObjects.View).activeSelf) return;
            if (_isUpdatingView) return;

            OnNextDialogueRequest?.Invoke();
        }

        public enum GameObjects
        {
            Left_Illustration_Parent,
            Left_DisplayName_Parent,
            Right_Illustration_Parent,
            Right_DisplayName_Parent,
            View
        }

        public enum Texts
        {
            Dialogue_Text,
            Dialogue_Text_Shadow,
            Left_DisplayName_Text,
            Right_DisplayName_Text
        }

        public enum Images
        {
            Left_Illustration_Image,
            Right_Illustration_Image
        }

        public enum Buttons
        {
            Select_Option_Button_1,
            Select_Option_Button_2,
            Select_Option_Button_3,
            Select_Option_Button_4,
        }

        public override void BindUI()
        {
            Bind<TextMeshProUGUI>(typeof(Texts));
            Bind<Image>(typeof(Images));
            Bind<GameObject>(typeof(GameObjects));
            Bind<Button>(typeof(Buttons));
        }

        public void SetActive(bool value)
        {
            Get<GameObject>((int)GameObjects.View).SetActive(value);
        }

        // Model을 참조하여 UI 업데이트
        public void UpdateUI(DialogueModel model)
        {
            _isUpdatingView = true;

            if (model.ChildNodes.Count >= 2)
            {
                HideGameObject(GameObjects.Left_DisplayName_Parent);
                HideGameObject(GameObjects.Right_DisplayName_Parent);
                SetText(Texts.Dialogue_Text, string.Empty);
                SetText(Texts.Dialogue_Text_Shadow, string.Empty);
                ShowOptionsButtons(model);
            }
            else
            {
                SetText(Texts.Dialogue_Text_Shadow, model.DialogueText);
                SetDialogueText(Texts.Dialogue_Text, model.DialogueText, true);

                if (model.IsLeftSpeaker)
                {
                    SetText(Texts.Left_DisplayName_Text, model.LeftDisplayName);
                    SetImage(Images.Left_Illustration_Image, model.LeftIllustration);
                    ShowGameObject(GameObjects.Left_DisplayName_Parent);
                    ShowGameObject(GameObjects.Left_Illustration_Parent);
                    HideGameObject(GameObjects.Right_DisplayName_Parent);
                    HideGameObject(GameObjects.Right_Illustration_Parent);
                }
                else
                {
                    SetText(Texts.Right_DisplayName_Text, model.RightDisplayName);
                    SetImage(Images.Right_Illustration_Image, model.RightIllustration);
                    ShowGameObject(GameObjects.Right_DisplayName_Parent);
                    ShowGameObject(GameObjects.Right_Illustration_Parent);
                    HideGameObject(GameObjects.Left_DisplayName_Parent);
                    HideGameObject(GameObjects.Left_Illustration_Parent);
                }

                HideAllOptionButtons();
            }
        }

        private void ShowOptionsButtons(DialogueModel model)
        {
            HideAllOptionButtons();
            for (int i = 0; i < model.ChildNodes.Count && i < 4; i++)
            {
                var button = Get<Button>((int)Buttons.Select_Option_Button_1 + i);
                button.gameObject.SetActive(true);

                string text = model.ChildNodes[i].optionText;

                var buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = text;
                }
            }
        }

        private void HideAllOptionButtons()
        {
            foreach (Buttons buttonEnum in Enum.GetValues(typeof(Buttons)))
            {
                var button = Get<Button>((int)buttonEnum);
                if (button != null)
                {
                    button.gameObject.SetActive(false);
                }
            }
        }

        private void SetText(Texts textElement, string content)
        {
            var text = Get<TextMeshProUGUI>((int)textElement);
            if (text != null)
            {
                text.SetText(content);
            }
        }

        private void SetDialogueText(Texts textElement, string content, bool isDOText = false)
        {
            var text = Get<TextMeshProUGUI>((int)textElement);
            if (text != null)
            {
                if (isDOText)
                {
                    int contentSize = content.Length;
                    float displayDuration = Mathf.Min(_maxDisplayDuration, contentSize * 0.1f); // 글자 길이에 따라 최대 5초 내에서 조절

                    text.text = string.Empty;
                    text.DOText(content, displayDuration, true, ScrambleMode.None)
                        .OnComplete(() =>
                        {
                            _isUpdatingView = false;
                        });
                }
                else
                {
                    text.SetText(content);

                    _isUpdatingView = false;
                }
            }
        }


        private void SetImage(Images imageElement, Sprite sprite)
        {
            var image = Get<Image>((int)imageElement);
            if (image != null)
            {
                image.sprite = sprite;
                image.enabled = sprite != null;
            }
        }

        private void ShowGameObject(GameObjects gameObjectElement)
        {
            var obj = Get<GameObject>((int)gameObjectElement);
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        private void HideGameObject(GameObjects gameObjectElement)
        {
            var obj = Get<GameObject>((int)gameObjectElement);
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
