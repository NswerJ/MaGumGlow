using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHP : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;
    private MonsterGetSO _monsterGetSO;

    public float goldReward = 50f;  // ���͸� óġ���� �� �ִ� ���
    public List<Sprite> _monsterSprites;

    private float _hp;
    private float _maxHp;
    private float _LV = 1;
    private bool _isDead, _isDamage;

    public Action Hit;
    public Action Dead;

    public Transform _startPos;
    private MagicSwordStats magicSwordStats;  // ���� ������ ������ ����

    [SerializeField] private GameObject damageTextPrefab;

    public void Initialize(MagicSwordStats stats)
    {
        magicSwordStats = stats;  // ���� ���� �Ҵ�
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
        Hit?.Invoke();  // ���Ͱ� �������� ���� �� ����� �׼� ȣ��

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

        // ���� ������ ������ ��� ��� �߰�
        if (magicSwordStats != null)
        {
            magicSwordStats.AddGold(goldReward);  // ���� óġ �� ��� ȹ��
            Debug.Log($"��� {goldReward} �߰���. �� ���: {magicSwordStats.playerGold}");
        }
        else
        {
            Debug.LogWarning("MagicSwordStats�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        Dead?.Invoke();  // ���Ͱ� ���� �� ����� �׼� ȣ��
        TempResetMonster();  // ���͸� �����ϴ� �ӽ� ó��
    }

    #region �ӽ�
    private void TempResetMonster()
    {
        _monster.GetComponent<ParralaxBackground>().enabled = false;
        StartCoroutine(DelayReset());
    }

    private IEnumerator DelayReset()
    {
        transform.position = _startPos.position;
        _LV++;  // ���� ���� ����
        _maxHp += _LV * 10000;  // ���� ü�� ����
        _hp = _maxHp;

        // ������ ü�¿� ���� ��������Ʈ ����
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
