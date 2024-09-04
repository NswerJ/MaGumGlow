using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterHP : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;

    private int _hp;
    private bool _isDead;

    public UnityEvent Hit = null;
    public UnityEvent Dead = null;

    public void Initialize(Monster monster)
    {
        _monster = monster;
        _isDead = false;

        _hp = monster.GetCompo<MonsterGetSO>().SO.MonsterHP;
    }

    public void OnDamage(int dmg)
    {
        if (_isDead) return;

        // GGAMBBACK
        _hp -= dmg;
        Hit?.Invoke();
        //UI UPDATE

        if (_hp <= 0) DeadProcess();
    }

    private void DeadProcess()
    {
        _isDead = true;
        Dead?.Invoke();
        //Player Run & Next Monster
    }
}
