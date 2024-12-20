using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//보스랑도 같이 쓰려면 어떻게 구성해야될까;;; 어렵다 증말.
public class MonsterHP : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;

    private MonsterSO _monsterSO;
    private MonsterStat HP;

    private MagicSwordStats magicSwordStats;  // 마검 스탯을 참조할 변수
    private PlayerAnimation _playerAnim; // 어떻게 바꿀까


    #region Stat
    public float currentHP;
    public float maxHP;
    public int LV;

    public bool IsDead;
    public bool IsDropItem;
    #endregion

    [SerializeField] private GameObject chestPrefab;    

    #region Event
    public event Action Hit;
    public event Action Dead;
    //쓸진 모르겟다
    #endregion


    [SerializeField] private GameObject damageTextPrefab;
    private GameObject damageTextClone;

    public void Initialize(MagicSwordStats stats)
    {

        magicSwordStats = stats;  // 마검 스탯 할당
        _playerAnim = GameObject.Find("Player").GetComponentInChildren<PlayerAnimation>();

        _playerAnim.DamageTextEvent += SlashHit;

    }

    public void Initialize(Monster monster)
    {

        _monster = monster;
        IsDead = false;


        _monsterSO = _monster.SO;

        HP = _monsterSO.StatSO.Stats.Find(stat => stat.statName == "체력");

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
        Hit?.Invoke();  // 몬스터가 데미지를 받을 때 실행될 액션 호출 (맞는 이펙트 / 빨갛게 깜빡임 등)


    }

    //몬스터가 칼에 맞을 때 호출
    public void SlashHit()
    {

        if (damageTextClone == null)
        {
            damageTextClone = Instantiate(damageTextPrefab);

            //자연스럽게 보이게
            if (HP.currentValue <= 0)
            {
                DeadProcess();
            }
        }

    }

    private void DeadProcess()
    {

        IsDead = true;

        if(IsDropItem)
        {
            //Item Drop
            Instantiate(chestPrefab, null);
        }

        // 마검 스탯이 존재할 경우 골드 추가
        if (magicSwordStats != null)
        {
            magicSwordStats.AddGold(_monsterSO.DropGold);  // 몬스터 처치 시 골드 획득
            _monsterSO.DropGold += 10;
            //Debug.Log($"골드 {_monsterSO.DropGold} 추가됨. 총 골드: {magicSwordStats.playerGold}");
        }
        else
        {
            Debug.LogWarning("MagicSwordStats가 할당되지 않았습니다.");
        }

        Dead?.Invoke();  // 몬스터가 죽을 때 실행될 액션 호출

        Invoke(nameof(Respawn), 1);

    }

    private void Respawn()
    {
        IsDead = false;
    }
}
