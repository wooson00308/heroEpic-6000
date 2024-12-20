using Scripts;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsData", menuName = "Scriptable Objects/StatsData")]
public class StatsData : ScriptableObject, IStatsSetable
{
    public int id;
    public string displayName;
    public int health;
    public int damage;
    public float runSpeed;

    public void Setup(IStats stats)
    {
        stats.DisplayName = displayName;
        stats.Health = health;
        stats.Damage = damage;
        stats.RunSpeed = runSpeed;
    }
}
