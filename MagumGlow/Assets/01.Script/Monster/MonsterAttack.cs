using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MonsterAttack : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;

    private MonsterSO _monsterSO;
    private MonsterStat ATK;
    private MonsterRaycast _monsterRay;

    private MagicSwordPlayer _player;

    private Coroutine damageCoroutine;  // 플레이어에게 데미지를 주기 위한 코루틴

    public event Action<bool> AtkEvent;

    public void Initialize(Monster monster)
    {

        _monster = monster;

        _monsterRay = _monster.GetCompo<MonsterRaycast>();

        _monsterSO = _monster.SO;
        ATK = _monsterSO.StatSO.Stats.Find(stat => stat.statName == "공격력");

        _player = GameObject.Find("Player").GetComponent<MagicSwordPlayer>();   //이런건 어떻게 찾는게 효율적일까


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
                AtkEvent?.Invoke(true);
            }
        }
        else
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;  // 코루틴 참조를 null로 설정
                AtkEvent?.Invoke(false);
            }
        }
    }

    private IEnumerator DealDamageAfterDelay()
    {
        while (true)  // 반복적으로 데미지를 주기 위한 루프
        {
            if (ATK != null)
            {
                _player.OnDamage(ATK.currentValue); // 플레이어에게 데미지 적용
                Debug.Log($"플레이어에게 {ATK.currentValue} 데미지를 입혔습니다.");
            }
            else
            {
                Debug.LogWarning("적의 공격력 스탯을 찾을 수 없습니다.");
            }
            yield return new WaitForSeconds(1f);  //공속을 따로 뺄까 말까
        }
    }
}
