using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Tree", menuName = "Dialogue/Dialogue Tree")]
public class DialogueTreeData : ScriptableObject
{
    public List<DialogueNodeData> nodes; // Stores all dialogue nodes in a list
}

[System.Serializable]
public class DialogueNodeData
{
    public string nodeId;
    public string leftDisplayName;
    public Sprite leftIllustration;
    public Vector3 leftIllustrationPos;
    public Vector3 leftIllustrationScale;
    public string rightDisplayName;
    public Sprite rightIllustration;
    public Vector3 rightIllustrationPos;
    public Vector3 rightIllustrationScale;
    public bool isLeftSpeaker;
    public string dialogueText;
    public List<DialogueOptionData> childNodes;

    public DialogueNodeData()
    {
        nodeId = "";
        leftDisplayName = "Left Speaker";
        rightDisplayName = "Right Speaker";
        dialogueText = "";
        childNodes = new List<DialogueOptionData>();
    }
}

[System.Serializable]
public class DialogueOptionData
{
    public string id;
    public string optionText;

    public DialogueOptionData()
    {
        id = "";
        optionText = "Option Text";
    }
}
