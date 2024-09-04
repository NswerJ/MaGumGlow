using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHP : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;

    private int _hp;
    private bool _isDead;

    public void Initialize(Monster monster)
    {
        _monster = monster;
        _isDead = false;

        _hp = monster.GetCompo<MonsterGetSO>().SO.MonsterHP;
    }

    public void OnDamage(int dmg)
    {
        if(_hp <= 0)
        {
            _isDead = true;
            Debug.Log("Dead");
        }

        _hp -= dmg;
    }
}
