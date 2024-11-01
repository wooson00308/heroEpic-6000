using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class DialogueView : BaseView
    {
        public event Action OnNextDialogueRequest; // 다음 대화 요청 이벤트

        private void OnEnable()
        {
            BindUI();
            SetActive(false);
        }

        private void Update()
        {
            // 선택지가 2개 미만일 때, 마우스 클릭이나 엔터키로 다음 대화 요청
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
            {
                OnNextDialogueRequest?.Invoke();
            }
        }

        public enum GameObjects
        {
            Left_DisplayName_Parent,
            Right_DisplayName_Parent,
            View
        }

        public enum Texts
        {
            Dialogue_Text,
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
            if (model.ChildNodes.Count >= 2)
            {
                HideGameObject(GameObjects.Left_DisplayName_Parent);
                HideGameObject(GameObjects.Right_DisplayName_Parent);
                SetText(Texts.Dialogue_Text, string.Empty);
                ShowOptionsButtons(model);
            }
            else
            {
                SetText(Texts.Dialogue_Text, model.DialogueText);

                if (model.IsLeftSpeaker)
                {
                    SetText(Texts.Left_DisplayName_Text, model.LeftDisplayName);
                    SetImage(Images.Left_Illustration_Image, model.LeftIllustration);
                    ShowGameObject(GameObjects.Left_DisplayName_Parent);
                    HideGameObject(GameObjects.Right_DisplayName_Parent);
                }
                else
                {
                    SetText(Texts.Right_DisplayName_Text, model.RightDisplayName);
                    SetImage(Images.Right_Illustration_Image, model.RightIllustration);
                    ShowGameObject(GameObjects.Right_DisplayName_Parent);
                    HideGameObject(GameObjects.Left_DisplayName_Parent);
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

                var buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = model.ChildNodes[i].optionText;
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
                text.text = content;
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
