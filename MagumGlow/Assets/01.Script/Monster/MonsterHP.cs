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
    public float currentHP;
    public float maxHP;
    public int LV;

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

        maxHP = HP.currentValue;

    }

    private void Start()
    {

        //HP.currentValue = HP.baseValue;


        CalculateHP();

        Dead += CalculateHP;

    }


    private void CalculateHP()
    {

        LV = _monsterSO.MonsterLV;

        maxHP = HP.currentValue = Mathf.Min(maxHP + HP.baseValue * LV, HP.maxValue);
        //_maxHP = HP.currentValue = Mathf.Min(HP.currentValue + HP.baseValue * _LV, HP.maxValue);

    }

    public void OnDamage(float dmg)
    {

        if (IsDead) return;

        HP.currentValue -= dmg;
        Hit?.Invoke();  // ���Ͱ� �������� ���� �� ����� �׼� ȣ�� (�´� ����Ʈ / ������ ������ ��)


    }

    //���Ͱ� Į�� ���� �� ȣ��
    public void SlashHit()
    {

        if (damageTextClone == null)
        {
            damageTextClone = Instantiate(damageTextPrefab);

            //�ڿ������� ���̰�
            if (HP.currentValue <= 0)
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
            _monsterSO.DropGold += 10;
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
