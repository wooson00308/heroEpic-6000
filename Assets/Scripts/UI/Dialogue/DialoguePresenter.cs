using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.UI
{
    public class DialoguePresenter : BasePresenter<DialogueView, DialogueModel>
    {
        public static Action<DialogueTreeData> InitializeDialogue;
        public static Action Start;
        public static Action End;

        private void OnEnable()
        {
            InitializeDialogue += OnInitializeDialogue;
        }

        private void OnDisable()
        {
            InitializeDialogue -= OnInitializeDialogue;
        }

        // 다이얼로그 트리를 초기화하고 UI 업데이트
        public void OnInitializeDialogue(DialogueTreeData dialogueTreeData)
        {
            Start?.Invoke();

            Model.SetDialogueTree(dialogueTreeData);

            View.SetActive(true);

            View.OnNextDialogueRequest += HandleNextDialogueRequest;
            View.UpdateUI(Model);
        }

        // 선택한 인덱스에 따라 다음 다이얼로그로 진행하고 UI 업데이트
        public void MoveToNextDialogue(int choiceIndex = 0)
        {
            if (Model.ChildNodes.Count > 0)
            {
                Model.MoveToNextNode(choiceIndex);
                View.UpdateUI(Model);
            }
            else
            {
                EndDialogue(); // 자식이 없는 경우 대화 종료
            }
        }

        // 대화 종료 메서드
        public void EndDialogue()
        {
            Debug.Log("Dialogue Ended");
            View.OnNextDialogueRequest -= HandleNextDialogueRequest; // 이벤트 구독 해제
            // 추가 종료 로직 (예: UI 숨기기)도 여기서 수행할 수 있습니다.

            View.SetActive(false);

            End?.Invoke();
        }

        private void HandleNextDialogueRequest()
        {
            MoveToNextDialogue(0); // 기본적으로 첫 번째 선택지로 이동
        }

        // 다이얼로그를 처음부터 시작하고 UI 업데이트
        public void ResetDialogue()
        {
            Model.ResetDialogue();
            View.UpdateUI(Model);
        }
    }
}
