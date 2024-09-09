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
        //_monster.GetCompo<ParralaxBackground>().enabled = false;
        StartCoroutine(DelayReset());
    }

    private IEnumerator DelayReset()
    {
        transform.position = _startPos.position;
        _maxHp += 20;
        _hp = _maxHp;

        if (_maxHp >= 500)
            _monsterGetSO.SO.Sprite = _monsterSprites[4];
        else if (_maxHp >= 400)
            _monsterGetSO.SO.Sprite = _monsterSprites[3];
        else if (_maxHp >= 300) 
            _monsterGetSO.SO.Sprite = _monsterSprites[2];
        else if (_maxHp >= 200)
            _monsterGetSO.SO.Sprite = _monsterSprites[1];
        else if (_maxHp >= 100)
            _monsterGetSO.SO.Sprite = _monsterSprites[0];


        yield return new WaitForSeconds(1);
        //_monster.GetCompo<ParralaxBackground>().enabled = true;
        _isDead = false;
    }
    #endregion
}
