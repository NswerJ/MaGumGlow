using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/MonsterStats")]
public class MonsterStats : ScriptableObject
{
    public List<MonsterStat> stats;
}
