using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/QuestData")]
public class QuestData : ScriptableObject
{
    public int id;
    public List<SubQuestData> subQuests;
    public List<Prerequisite> prerequisites;
    public List<QuestReward> rewards;
}

public enum QuestStatus
{
    Inactive,
    Active,
    Fail,
    Complete
}

[System.Serializable]
public class Prerequisite
{
    public int questId;
}

[System.Serializable]
public class QuestReward
{
    public int itemId;
    public int amount;
}
