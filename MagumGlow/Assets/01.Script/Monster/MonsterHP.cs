using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterHP : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;

    private MonsterGetSO _monsterGetSO;

    public List<Sprite> _monsterSprites;

    private float _hp;
    private float _maxHp;

    private float _LV = 1;

    private bool _isDead;

    public Action Hit;
    public Action Dead;

    public Transform _startPos;


    public void Initialize(Monster monster)
    {

        _monster = monster;
        _isDead = false;

        _monsterGetSO = _monster.GetCompo<MonsterGetSO>();

        _maxHp = _monsterGetSO.SO.MonsterHP;

        _hp = _maxHp;

    }



    public void OnDamage(float dmg)
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

        TempResetMonster();
    }

    #region юс╫ц
    private void TempResetMonster()
    {
        _monster.GetComponent<ParralaxBackground>().enabled = false;
        StartCoroutine(DelayReset());
    }

    private IEnumerator DelayReset()
    {
        transform.position = _startPos.position;
        _LV++;
        _maxHp += _LV * 10000;
        _hp = _maxHp;

        int index = Math.Min((int)(_maxHp / 1000000), 5);
        _monsterGetSO.SO.Sprite = _monsterSprites[index];
        _monster.GetCompo<MonsterVisual>().UpdateSprite();


        yield return new WaitForSeconds(1);
        _monster.GetComponent<ParralaxBackground>().enabled = true;
        _isDead = false;
    }
    #endregion
}
