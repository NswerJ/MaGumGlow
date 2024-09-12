using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHP : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;
    private MonsterGetSO _monsterGetSO;
    private MagicSwordStats magicSwordStats;  // 마검 스탯을 참조할 변수
    private PlayerAnimation _playerAnim; // 어떻게 바꿀까

    #region Absolutely needs to be fixed
    public List<Sprite> _monsterSprites;
    public Transform _startPos;
    #endregion


    #region Stat
    private float _hp;
    private float _maxHp;
    private float _LV = 1;
    private bool _isDead, _isDamage;
    #endregion


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
        _isDead = false;

        _monsterGetSO = _monster.GetCompo<MonsterGetSO>();

        _maxHp = _monsterGetSO.SO.MonsterHP;
        _hp = _maxHp;
    }

    public void OnDamage(float dmg)
    {
        if (_isDead) return;

        _hp -= dmg;
        Hit?.Invoke();  // 몬스터가 데미지를 받을 때 실행될 액션 호출
    }

    public void SlashHit()
    {
        if (damageTextClone == null)
        {
            damageTextClone = Instantiate(damageTextPrefab);
            damageTextClone.transform.SetParent(transform.Find("DamageCanvas"), false);
            
            //자연스럽게 보이게
            if (_hp <= 0)
            {
                DeadProcess();
            }
        }
    }

    private void DeadProcess()
    {
        _isDead = true;

        // 마검 스탯이 존재할 경우 골드 추가
        if (magicSwordStats != null)
        {
            magicSwordStats.AddGold(_monsterGetSO.SO.DropGold);  // 몬스터 처치 시 골드 획득
            Debug.Log($"골드 {_monsterGetSO.SO.DropGold} 추가됨. 총 골드: {magicSwordStats.playerGold}");
        }
        else
        {
            Debug.LogWarning("MagicSwordStats가 할당되지 않았습니다.");
        }

        Dead?.Invoke();  // 몬스터가 죽을 때 실행될 액션 호출
        TempResetMonster();  // 몬스터를 리셋하는 임시 처리
    }

    #region 임시
    private void TempResetMonster()
    {
        _monster.GetComponent<ParralaxBackground>().enabled = false;
        StartCoroutine(DelayReset());
    }

    private IEnumerator DelayReset()
    {
        transform.position = _startPos.position;
        _LV++;  // 몬스터 레벨 증가
        _maxHp += _LV * 10000;  // 몬스터 체력 증가
        _hp = _maxHp;

        // 몬스터의 체력에 따라 스프라이트 변경
        int index = Math.Min((int)(_maxHp / 1000000), _monsterSprites.Count - 1);
        _monsterGetSO.SO.Sprite = _monsterSprites[index];
        _monster.GetCompo<MonsterVisual>().UpdateSprite();

        _monster.GetComponent<ParralaxBackground>().monsterSpeed = 0;

        yield return new WaitForSeconds(.5f);
        _monster.GetComponent<ParralaxBackground>().enabled = true;
        _isDead = false;
    }


    #endregion
}
