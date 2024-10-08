using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;

    private MonsterSO _monsterSO;
    private MonsterStat ATK;
    private MonsterRaycast _monsterRay;

    private Stat _playerHP;

    private Coroutine damageCoroutine;  // 플레이어에게 데미지를 주기 위한 코루틴

    public void Initialize(Monster monster)
    {

        _monster = monster;

        _monsterRay = _monster.GetCompo<MonsterRaycast>();

        _monsterSO = _monster.SO;
        ATK = _monsterSO.StatSO.Stats.Find(stat => stat.statName == "공격력");

        _playerHP = GameManager.Instance.playerData.stats.Find(stat => stat.statName == "생명력");        

    }

    private void Start()
    {
    }

    private void Update()
    {
        if (_monsterRay.IsDetected) /*어떻게공격을받을까*/
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DealDamageAfterDelay());  // 딜레이 후 데미지 처리
            }
        }
        else
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;  // 코루틴 참조를 null로 설정
            }
        }
    }

    private IEnumerator DealDamageAfterDelay()
    {
        while (true)  // 반복적으로 데미지를 주기 위한 루프
        {
            if (ATK != null)
            {
                _playerHP.currentValue -= (ATK.currentValue);  // 플레이어에게 데미지 적용
                Debug.Log($"플레이어에게 {ATK.currentValue} 데미지를 입혔습니다.");
            }
            else
            {
                Debug.LogWarning("적의 공격력 스탯을 찾을 수 없습니다.");
            }
            yield return new WaitForSeconds(0.65f);  // 0.65f초 대기 공속을 따로 뺄까 말까
        }
    }
}
