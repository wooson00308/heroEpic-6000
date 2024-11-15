using UnityEngine;

[CreateAssetMenu(fileName = "SubQuestData", menuName = "Scriptable Objects/SubQuestData")]
public class SubQuestData : ScriptableObject
{
    public int id;
    public string displayName;
    public int amount;
    public bool isIncreaseQuest;
}
