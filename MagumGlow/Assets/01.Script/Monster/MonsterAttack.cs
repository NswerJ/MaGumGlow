using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;

    private MonsterSO _monsterSO;
    private MonsterStat ATK;

    private MonsterRaycast _monsterRay;

    public void Initialize(Monster monster)
    {

        _monster = monster;

        _monsterRay = _monster.GetCompo<MonsterRaycast>();

        _monsterSO = _monster.SO;
        ATK = _monsterSO.StatSO.Stats.Find(stat => stat.statName == "°ø°Ý·Â");

    }

    private void Update()
    {
        
    }
}
