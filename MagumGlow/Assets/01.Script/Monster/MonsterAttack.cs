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

    private Coroutine damageCoroutine;  // �÷��̾�� �������� �ֱ� ���� �ڷ�ƾ

    public void Initialize(Monster monster)
    {

        _monster = monster;

        _monsterRay = _monster.GetCompo<MonsterRaycast>();

        _monsterSO = _monster.SO;
        ATK = _monsterSO.StatSO.Stats.Find(stat => stat.statName == "���ݷ�");

        _playerHP = GameManager.Instance.playerData.stats.Find(stat => stat.statName == "�����");        

    }

    private void Start()
    {
    }

    private void Update()
    {
        if (_monsterRay.IsDetected) /*��԰�����������*/
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DealDamageAfterDelay());  // ������ �� ������ ó��
            }
        }
        else
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;  // �ڷ�ƾ ������ null�� ����
            }
        }
    }

    private IEnumerator DealDamageAfterDelay()
    {
        while (true)  // �ݺ������� �������� �ֱ� ���� ����
        {
            if (ATK != null)
            {
                _playerHP.currentValue -= (ATK.currentValue);  // �÷��̾�� ������ ����
                Debug.Log($"�÷��̾�� {ATK.currentValue} �������� �������ϴ�.");
            }
            else
            {
                Debug.LogWarning("���� ���ݷ� ������ ã�� �� �����ϴ�.");
            }
            yield return new WaitForSeconds(0.65f);  // 0.65f�� ��� ������ ���� ���� ����
        }
    }
}
