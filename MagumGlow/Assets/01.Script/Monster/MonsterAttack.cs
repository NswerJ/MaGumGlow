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

    private Coroutine damageCoroutine;  // �÷��̾�� �������� �ֱ� ���� �ڷ�ƾ

    public event Action<bool> AtkEvent;

    public void Initialize(Monster monster)
    {

        _monster = monster;

        _monsterRay = _monster.GetCompo<MonsterRaycast>();

        _monsterSO = _monster.SO;
        ATK = _monsterSO.StatSO.Stats.Find(stat => stat.statName == "���ݷ�");

        _player = GameObject.Find("Player").GetComponent<MagicSwordPlayer>();   //�̷��� ��� ã�°� ȿ�����ϱ�


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
                AtkEvent?.Invoke(true);
            }
        }
        else
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;  // �ڷ�ƾ ������ null�� ����
                AtkEvent?.Invoke(false);
            }
        }
    }

    private IEnumerator DealDamageAfterDelay()
    {
        while (true)  // �ݺ������� �������� �ֱ� ���� ����
        {
            if (ATK != null)
            {
                _player.OnDamage(ATK.currentValue); // �÷��̾�� ������ ����
                Debug.Log($"�÷��̾�� {ATK.currentValue} �������� �������ϴ�.");
            }
            else
            {
                Debug.LogWarning("���� ���ݷ� ������ ã�� �� �����ϴ�.");
            }
            yield return new WaitForSeconds(1f);  //������ ���� ���� ����
        }
    }
}
