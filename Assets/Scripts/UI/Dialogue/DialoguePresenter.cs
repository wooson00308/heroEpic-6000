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

        // ���̾�α� Ʈ���� �ʱ�ȭ�ϰ� UI ������Ʈ
        public void OnInitializeDialogue(DialogueTreeData dialogueTreeData)
        {
            Start?.Invoke();

            Model.SetDialogueTree(dialogueTreeData);

            View.SetActive(true);

            View.OnNextDialogueRequest += HandleNextDialogueRequest;
            View.UpdateUI(Model);
        }

        // ������ �ε����� ���� ���� ���̾�α׷� �����ϰ� UI ������Ʈ
        public void MoveToNextDialogue(int choiceIndex = 0)
        {
            if (Model.ChildNodes.Count > 0)
            {
                Model.MoveToNextNode(choiceIndex);
                View.UpdateUI(Model);
            }
            else
            {
                EndDialogue(); // �ڽ��� ���� ��� ��ȭ ����
            }
        }

        // ��ȭ ���� �޼���
        public void EndDialogue()
        {
            Debug.Log("Dialogue Ended");
            View.OnNextDialogueRequest -= HandleNextDialogueRequest; // �̺�Ʈ ���� ����
            // �߰� ���� ���� (��: UI �����)�� ���⼭ ������ �� �ֽ��ϴ�.

            View.SetActive(false);

            End?.Invoke();
        }

        private void HandleNextDialogueRequest()
        {
            MoveToNextDialogue(0); // �⺻������ ù ��° �������� �̵�
        }

        // ���̾�α׸� ó������ �����ϰ� UI ������Ʈ
        public void ResetDialogue()
        {
            Model.ResetDialogue();
            View.UpdateUI(Model);
        }
    }
}
