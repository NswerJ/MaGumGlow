using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHP : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;
    private MonsterGetSO _monsterGetSO;
    private MagicSwordStats magicSwordStats;  // ���� ������ ������ ����
    private PlayerAnimation _playerAnim; // ��� �ٲܱ�

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
        _isDead = false;

        _monsterGetSO = _monster.GetCompo<MonsterGetSO>();

        _maxHp = _monsterGetSO.SO.MonsterHP;
        _hp = _maxHp;
    }

    public void OnDamage(float dmg)
    {
        if (_isDead) return;

        _hp -= dmg;
        Hit?.Invoke();  // ���Ͱ� �������� ���� �� ����� �׼� ȣ��
    }

    public void SlashHit()
    {
        if (damageTextClone == null)
        {
            damageTextClone = Instantiate(damageTextPrefab);
            damageTextClone.transform.SetParent(transform.Find("DamageCanvas"), false);
            
            //�ڿ������� ���̰�
            if (_hp <= 0)
            {
                DeadProcess();
            }
        }
    }

    private void DeadProcess()
    {
        _isDead = true;

        // ���� ������ ������ ��� ��� �߰�
        if (magicSwordStats != null)
        {
            magicSwordStats.AddGold(_monsterGetSO.SO.DropGold);  // ���� óġ �� ��� ȹ��
            Debug.Log($"��� {_monsterGetSO.SO.DropGold} �߰���. �� ���: {magicSwordStats.playerGold}");
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

        yield return new WaitForSeconds(.5f);
        _monster.GetComponent<ParralaxBackground>().enabled = true;
        _isDead = false;
    }


    #endregion
}
