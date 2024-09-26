using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHP : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;

    private MonsterSO _monsterSO;
    private MonsterStat HP;

    private MagicSwordStats magicSwordStats;  // ���� ������ ������ ����
    private PlayerAnimation _playerAnim; // ��� �ٲܱ�


    #region Stat
    private float _currentHP;
    private float _maxHP;
    private int _LV;
    private bool _isDead, _isDamage;
    #endregion


    #region Event
    public event Action Hit;
    public event Action Dead;
    //���� �𸣰ٴ�
    #endregion


    [SerializeField] private GameObject damageTextPrefab;
    private GameObject damageTextClone;

    public void Initialize(MagicSwordStats stats)
    {

        magicSwordStats = stats;  // ���� ���� �Ҵ�
        _playerAnim = GameObject.Find("Player").GetComponentInChildren<PlayerAnimation>();

        _playerAnim.DamageTextEvent += SlashHit;

    }

    public void Initialize(Monster monster)
    {

        _monster = monster;
        _isDead = false;


        _monsterSO = _monster.SO;

        HP = _monsterSO.StatSO.Stats.Find(stat => stat.statName == "ü��");
        _LV = _monsterSO.MonsterLV;

        //���� ������ �ؾ��ҵ�?
        _maxHP = CalculateHP();

        _currentHP = _maxHP;

    }

    private float CalculateHP()
    {

        return HP.currentValue = Mathf.Min(HP.currentValue + HP.baseValue * _LV, HP.maxValue);

    }

    public void OnDamage(float dmg)
    {

        if (_isDead) return;

        _currentHP -= dmg;
        Hit?.Invoke();  // ���Ͱ� �������� ���� �� ����� �׼� ȣ��

    }

    //���Ͱ� Į�� ���� �� ȣ��
    public void SlashHit()
    {

        if (damageTextClone == null)
        {
            damageTextClone = Instantiate(damageTextPrefab);

            //�ڿ������� ���̰�
            if (_currentHP <= 0)
            {
                DeadProcess();
            }
        }

    }

    private void DeadProcess()
    {

        _isDead = true;

        // ���� ������ ������ ��� ��� �߰�
        if (magicSwordStats != null)
        {
            magicSwordStats.AddGold(_monsterSO.DropGold);  // ���� óġ �� ��� ȹ��
            Debug.Log($"��� {_monsterSO.DropGold} �߰���. �� ���: {magicSwordStats.playerGold}");
        }
        else
        {
            Debug.LogWarning("MagicSwordStats�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        Dead?.Invoke();  // ���Ͱ� ���� �� ����� �׼� ȣ��
                         //TempResetMonster();  // ���͸� �����ϴ� �ӽ� ó��

    }

    #region �ӽ�
    //private void TempResetMonster()
    //{
    //    _monster.GetComponent<ParralaxBackground>().enabled = false;
    //    StartCoroutine(DelayReset());
    //}

    //private IEnumerator DelayReset()
    //{
    //    transform.position = _startPos.position;
    //    _LV++;  // ���� ���� ����
    //    _maxHp += _LV * 10000;  // ���� ü�� ����
    //    _hp = _maxHp;

    //    // ������ ü�¿� ���� ��������Ʈ ����
    //    int index = Math.Min((int)(_maxHp / 1000000), _monsterSprites.Count - 1);
    //    _monsterGetSO.SO.Sprite = _monsterSprites[index];
    //    _monster.GetCompo<MonsterVisual>().UpdateSprite();

    //    _monster.GetComponent<ParralaxBackground>().monsterSpeed = 0;

    //    yield return new WaitForSeconds(.5f);
    //    _monster.GetComponent<ParralaxBackground>().enabled = true;
    //    _isDead = false;
    //}


    #endregion
}
