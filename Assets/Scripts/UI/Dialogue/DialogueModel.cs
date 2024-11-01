using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UI
{
    public class DialogueModel : BaseModel
    {
        private DialogueTreeData dialogueTreeData;
        private DialogueNodeData currentNode;
        private int currentIndex;

        public string DialogueText => currentNode.dialogueText;
        public string LeftDisplayName => currentNode.leftDisplayName;
        public Sprite LeftIllustration => currentNode.leftIllustration;
        public string RightDisplayName => currentNode.rightDisplayName;
        public Sprite RightIllustration => currentNode.rightIllustration;
        public bool IsLeftSpeaker => currentNode.isLeftSpeaker;
        public List<DialogueOptionData> ChildNodes => currentNode.childNodes;

        public override void Initialize()
        {
            // Any initialization logic if needed
        }

        public void SetDialogueTree(DialogueTreeData data)
        {
            dialogueTreeData = data;

            if (dialogueTreeData == null || dialogueTreeData.nodes.Count == 0)
            {
                Debug.LogError("Dialogue Tree Data is not loaded or has no nodes.");
                return;
            }

            currentIndex = 0;
            currentNode = dialogueTreeData.nodes[currentIndex];
        }

        public void SetCurrentNode(string nodeId)
        {
            currentNode = dialogueTreeData.nodes.Find(node => node.nodeId == nodeId);
            if (currentNode == null)
            {
                Debug.LogError($"Node with ID {nodeId} not found.");
            }
        }

        public void MoveToNextNode(int choiceIndex = 0)
        {
            if (currentNode.childNodes.Count > choiceIndex)
            {
                string nextNodeId = currentNode.childNodes[choiceIndex].id;
                SetCurrentNode(nextNodeId);
            }
            else
            {
                Debug.LogWarning("Choice index is out of range.");
            }
        }

        public void ResetDialogue()
        {
            currentIndex = 0;
            currentNode = dialogueTreeData.nodes[currentIndex];
        }
    }
}
