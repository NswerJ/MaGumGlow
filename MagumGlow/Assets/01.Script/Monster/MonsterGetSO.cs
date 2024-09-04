using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGetSO : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;

    public MonsterSO SO;

    public void Initialize(Monster monster)
    {
        _monster = monster;
    }
}
