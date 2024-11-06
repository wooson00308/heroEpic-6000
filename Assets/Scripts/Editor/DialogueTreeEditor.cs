using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueTreeData))]
public class DialogueTreeEditor : Editor
{
    private DialogueTreeData dialogueTreeData;
    private bool showLeftSpeakerSettings = true;
    private bool showRightSpeakerSettings = true;
    private const int MaxOptions = 4; // Maximum number of child options

    // Dictionary to track foldout state for each node by node ID
    private Dictionary<string, bool> nodeFoldoutStates = new Dictionary<string, bool>();

    private void OnEnable()
    {
        dialogueTreeData = (DialogueTreeData)target;

        if (dialogueTreeData == null)
        {
            Debug.LogError("Failed to initialize dialogueTreeData. Make sure you have selected a DialogueTreeData asset.");
        }
        else if (dialogueTreeData.nodes == null)
        {
            dialogueTreeData.nodes = new List<DialogueNodeData>();
        }
    }

    public override void OnInspectorGUI()
    {
        if (dialogueTreeData == null)
        {
            EditorGUILayout.HelpBox("DialogueTreeData is not assigned or has not been initialized correctly.", MessageType.Error);
            return;
        }

        EditorGUILayout.LabelField("Dialogue Tree Editor", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (GUILayout.Button("Add New Node", GUILayout.Height(25)))
        {
            AddNewNode();
        }

        EditorGUILayout.Space();

        for (int i = 0; i < dialogueTreeData.nodes.Count; i++)
        {
            DrawNode(dialogueTreeData.nodes[i], i);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(dialogueTreeData);
        }
    }

    private void DrawNode(DialogueNodeData node, int index)
    {
        // Ensure foldout state exists for this node
        if (!nodeFoldoutStates.ContainsKey(node.nodeId))
        {
            nodeFoldoutStates[node.nodeId] = true; // Default to expanded
        }

        EditorGUILayout.BeginHorizontal(); // Start horizontal layout

        // Toggle for node foldout with node title
        nodeFoldoutStates[node.nodeId] = EditorGUILayout.Foldout(nodeFoldoutStates[node.nodeId], $"Dialogue Node - {node.nodeId}", true, EditorStyles.foldoutHeader);

        // Move Up/Down Buttons
        GUIContent upIcon = EditorGUIUtility.IconContent("arrow up");
        GUIContent downIcon = EditorGUIUtility.IconContent("arrow down");

        // Display the up button
        if (GUILayout.Button(upIcon, GUILayout.Width(25), GUILayout.Height(18)) && index > 0)
        {
            SwapNodes(index, index - 1);
        }

        // Display the down button
        if (GUILayout.Button(downIcon, GUILayout.Width(25), GUILayout.Height(18)) && index < dialogueTreeData.nodes.Count - 1)
        {
            SwapNodes(index, index + 1);
        }

        EditorGUILayout.EndHorizontal(); // End horizontal layout

        // If the node is expanded
        if (nodeFoldoutStates[node.nodeId])
        {
            EditorGUILayout.BeginVertical("box");

            // Node Identification Section
            node.nodeId = EditorGUILayout.TextField("Node ID", node.nodeId);

            // Multi-line Dialogue Text
            EditorGUILayout.LabelField("Dialogue Text");
            node.dialogueText = EditorGUILayout.TextArea(node.dialogueText, GUILayout.MinHeight(60), GUILayout.ExpandWidth(true));

            // Left Speaker Settings Section
            EditorGUILayout.Space();
            showLeftSpeakerSettings = EditorGUILayout.Foldout(showLeftSpeakerSettings, "Left Speaker Settings", true);
            if (showLeftSpeakerSettings)
            {
                EditorGUILayout.BeginVertical("box");
                node.leftDisplayName = EditorGUILayout.TextField("Left Display Name", node.leftDisplayName);
                node.leftIllustration = (Sprite)EditorGUILayout.ObjectField("Left Illustration", node.leftIllustration, typeof(Sprite), allowSceneObjects: false);
                node.leftIllustrationPos = EditorGUILayout.Vector3Field("Left Illustration Pos", node.leftIllustrationPos);
                node.leftIllustrationScale = EditorGUILayout.Vector3Field("Left Illustration Scale", node.leftIllustrationScale);
                EditorGUILayout.EndVertical();
            }

            // Right Speaker Settings Section
            EditorGUILayout.Space();
            showRightSpeakerSettings = EditorGUILayout.Foldout(showRightSpeakerSettings, "Right Speaker Settings", true);
            if (showRightSpeakerSettings)
            {
                EditorGUILayout.BeginVertical("box");
                node.rightDisplayName = EditorGUILayout.TextField("Right Display Name", node.rightDisplayName);
                node.rightIllustration = (Sprite)EditorGUILayout.ObjectField("Right Illustration", node.rightIllustration, typeof(Sprite), allowSceneObjects: false);
                node.rightIllustrationPos = EditorGUILayout.Vector3Field("Right Illustration Pos", node.rightIllustrationPos);
                node.rightIllustrationScale = EditorGUILayout.Vector3Field("Right Illustration Scale", node.rightIllustrationScale);
                EditorGUILayout.EndVertical();
            }

            // Speaker Side Selection
            EditorGUILayout.Space();
            node.isLeftSpeaker = EditorGUILayout.Toggle("Is Left Speaker", node.isLeftSpeaker);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Child Nodes", EditorStyles.boldLabel);

            // Display each child node option up to the maximum allowed
            for (int i = 0; i < node.childNodes.Count; i++)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField($"Child Option {i + 1}", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Node ID", GUILayout.Width(80));
                node.childNodes[i].id = EditorGUILayout.TextField(node.childNodes[i].id, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Option Text", GUILayout.Width(80));
                node.childNodes[i].optionText = EditorGUILayout.TextArea(node.childNodes[i].optionText, GUILayout.MinHeight(40), GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Remove Option", GUILayout.Width(100)))
                {
                    node.childNodes.RemoveAt(i);
                    break;
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }

            // Show a message if the maximum number of options is reached
            if (node.childNodes.Count >= MaxOptions)
            {
                EditorGUILayout.HelpBox($"A maximum of {MaxOptions} options are allowed.", MessageType.Info);
            }
            else if (GUILayout.Button("Add Child Node"))
            {
                node.childNodes.Add(new DialogueOptionData());
            }

            // Remove Node Button
            if (GUILayout.Button("Remove Node", GUILayout.Height(20)))
            {
                RemoveNode(node.nodeId);
            }

            EditorGUILayout.EndVertical();
        }
    }

    private void SwapNodes(int indexA, int indexB)
    {
        var temp = dialogueTreeData.nodes[indexA];
        dialogueTreeData.nodes[indexA] = dialogueTreeData.nodes[indexB];
        dialogueTreeData.nodes[indexB] = temp;
    }

    private void AddNewNode()
    {
        DialogueNodeData newNode = new DialogueNodeData
        {
            nodeId = $"Node_{dialogueTreeData.nodes.Count + 1}",
            dialogueText = "Enter dialogue text here..."
        };
        dialogueTreeData.nodes.Add(newNode);
    }

    private void RemoveNode(string nodeId)
    {
        var nodeToRemove = dialogueTreeData.nodes.Find(n => n.nodeId == nodeId);
        if (nodeToRemove != null)
        {
            dialogueTreeData.nodes.Remove(nodeToRemove);
            nodeFoldoutStates.Remove(nodeId); // Remove foldout state when node is deleted
        }
    }
}
