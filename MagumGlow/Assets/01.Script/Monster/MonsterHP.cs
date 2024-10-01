using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�������� ���� ������ ��� �����ؾߵɱ�;;; ��ƴ� ����.
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

    public bool IsDead;
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
        IsDead = false;


        _monsterSO = _monster.SO;

        HP = _monsterSO.StatSO.Stats.Find(stat => stat.statName == "ü��");
        _LV = _monsterSO.MonsterLV;

        //���� ������ �ؾ��ҵ�?
        CalculateHP();

        Dead += CalculateHP;

    }

    private void OnDisable()
    {
        //���� ���
        _playerAnim.DamageTextEvent -= SlashHit;
        Dead -= CalculateHP;
    }

    private void CalculateHP()
    {

        //�ӽ÷� ������ (�������� �ܰ迡 ���� �ø� ����)
        _monsterSO.LevelUP();

        _maxHP = HP.currentValue = Mathf.Min(HP.currentValue + HP.baseValue * _LV, HP.maxValue);

        _currentHP = _maxHP;

    }

    public void OnDamage(float dmg)
    {

        if (IsDead) return;

        _currentHP -= dmg;
        Hit?.Invoke();  // ���Ͱ� �������� ���� �� ����� �׼� ȣ�� (�´� ����Ʈ / ������ ������ ��)


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

        IsDead = true;

        // ���� ������ ������ ��� ��� �߰�
        if (magicSwordStats != null)
        {
            magicSwordStats.AddGold(_monsterSO.DropGold);  // ���� óġ �� ��� ȹ��
            //Debug.Log($"��� {_monsterSO.DropGold} �߰���. �� ���: {magicSwordStats.playerGold}");
        }
        else
        {
            Debug.LogWarning("MagicSwordStats�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        Dead?.Invoke();  // ���Ͱ� ���� �� ����� �׼� ȣ��

        Invoke(nameof(Respawn), 1);

    }

    private void Respawn()
    {
        IsDead = false;
    }
}
