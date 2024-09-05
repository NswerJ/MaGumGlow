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

    public Action Hit;
    public Action Dead;

    public void Initialize(Monster monster)
    {

        _monster = monster;
        _isDead = false;

        _hp = monster.GetCompo<MonsterGetSO>().SO.MonsterHP;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnDamage(5);    
        }
    }

    public void OnDamage(int dmg)
    {

        if (_isDead) return;

        _hp -= dmg;
        Hit?.Invoke();
        // GGAMBBACK
        //UI UPDATE

        if (_hp <= 0) DeadProcess();

    }

    private void DeadProcess()
    {
    
        _isDead = true;
        //Player Run & Next Monster


        Dead?.Invoke();
        //Drop Coin
        
    }
}
