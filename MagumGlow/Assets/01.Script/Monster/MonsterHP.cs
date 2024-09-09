using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHP : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;
    private MonsterGetSO _monsterGetSO;

    public float goldReward = 50f;  // 몬스터를 처치했을 때 주는 골드
    public List<Sprite> _monsterSprites;

    private float _hp;
    private float _maxHp;
    private float _LV = 1;
    private bool _isDead, _isDamage;

    public Action Hit;
    public Action Dead;

    public Transform _startPos;
    private MagicSwordStats magicSwordStats;  // 마검 스탯을 참조할 변수

    [SerializeField] private GameObject damageTextPrefab;

    public void Initialize(MagicSwordStats stats)
    {
        magicSwordStats = stats;  // 마검 스탯 할당
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

        if (!_isDamage)
            StartCoroutine(DamageDelay(dmg));
    }

    private IEnumerator DamageDelay(float dmg)
    {
        _isDamage = true;
        yield return new WaitForSeconds(.2f);
        _hp -= dmg;
        Hit?.Invoke();  // 몬스터가 데미지를 받을 때 실행될 액션 호출

        GameObject damageUI = Instantiate(damageTextPrefab) as GameObject;
        damageUI.transform.SetParent(transform.Find("DamageCanvas"), false);
        //damageUI.transform.position = new Vector3(-90, -150, 0);    

        if (_hp <= 0)
        {
            DeadProcess();
        }
        _isDamage = false;
    }

    private void DeadProcess()
    {
        _isDead = true;

        // 마검 스탯이 존재할 경우 골드 추가
        if (magicSwordStats != null)
        {
            magicSwordStats.AddGold(goldReward);  // 몬스터 처치 시 골드 획득
            Debug.Log($"골드 {goldReward} 추가됨. 총 골드: {magicSwordStats.playerGold}");
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

        yield return new WaitForSeconds(.25f);
        _monster.GetComponent<ParralaxBackground>().enabled = true;
        _isDead = false;
    }


    #endregion
}
