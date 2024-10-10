using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicSwordPlayer : MonoBehaviour
{
    public MagicSwordStats swordStats;
    public TextMeshProUGUI playerName;  // �÷��̾� �̸� UI
    public TextMeshProUGUI damageViewer; // ������ ǥ�� UI

    public LayerMask enemyLayer; // �� ���̾�
    public float detectionRange = 2f;  // �� Ž�� ����
    public Slider hpUI;
    public float playerHealth;  // �÷��̾� ü��
    private Stat hpStat;

    public event Action<bool> AttackEvent;
    public event Action<bool> DieEvent;
    public bool criticalCheck = false;

    private bool enemyIsFront = false;  // ���� �տ� �ִ��� ����
    private bool isDie = false;  // ���� ���� üũ
    private Coroutine damageCoroutine;  // ������ �������� �ֱ� ���� �ڷ�ƾ

    void Start()
    {
        hpStat = swordStats.stats.Find(stat => stat.statName == "�����");

        InitializeSword();
        
    }

    void InitializeSword()
    {
        // GameManager���� �÷��̾� �̸��� �����ͼ� UI�� �ݿ�
        playerName.text = GameManager.Instance.playerData.playerName;
    }

    private void Update()
    {
        CheckForEnemy();  // �� Ž��

        if (enemyIsFront)
        {
            Attacking(true);  // ���� ����
        }
        else
        {
            Attacking(false);  // ����� ����

            // ���� ���� ��� ������ �ڷ�ƾ ����
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;  // �ڷ�ƾ ������ null�� ����
            }
        }

        if (isDie || Input.GetButtonDown("Jump"))
        {
            DieEvent?.Invoke(true);  // ���� �̺�Ʈ �߻�
        }
    }

    public void OnDamage(float damage)
    {
        Debug.Log("����");
        hpStat.currentValue -= damage;
        PlayerHealthUpdate();

        if (playerHealth <= 0)
        {
            DieEvent?.Invoke(true); //������
        }
    }

    public void PlayerHealthUpdate()
    {
        playerHealth = hpStat.currentValue;  // �÷��̾� ü�� ������Ʈ

        hpUI.value = playerHealth;
    }

    private void CheckForEnemy()
    {
        Stat criticalPercentStat = swordStats.stats.Find(stat => stat.statName == "ġ��ŸȮ��");

        if (UnityEngine.Random.Range(0, 100) < criticalPercentStat.currentValue)
        {
            criticalCheck = true;
        }
        else
        {
            criticalCheck = false;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 1f, 0), Vector2.right, detectionRange, enemyLayer);

        if (hit.collider != null)
        {
            enemyIsFront = true;

            MonsterHP enemyHp = hit.collider.GetComponent<MonsterHP>();

            if (enemyHp != null)
            {
                enemyHp.Initialize(swordStats);

                // ������ �������� �ִ� �ڷ�ƾ�� �̹� ���� ������ ������ ����
                if (damageCoroutine == null)
                {
                    damageCoroutine = StartCoroutine(DealDamageAfterDelay(enemyHp));  // ������ �� ������ ó��
                }
            }
        }
        else
        {
            enemyIsFront = false;
        }
    }

    private IEnumerator DealDamageAfterDelay(MonsterHP enemyHp)
    {
        while (true)  // �ݺ������� �������� �ֱ� ���� ����
        {

            Stat attackPowerStat = swordStats.stats.Find(stat => stat.statName == "���ݷ�");
            Stat criticalPowerStat = swordStats.stats.Find(stat => stat.statName == "ġ��Ÿ������");
           

            if (attackPowerStat != null)
            {

                if (criticalCheck)
                {
                    enemyHp.OnDamage(attackPowerStat.currentValue * criticalPowerStat.currentValue);  // ������ ������ ����
                    Debug.Log($"������ {attackPowerStat.currentValue * criticalPowerStat.currentValue} �������� �������ϴ�.");
                }
                else
                {
                    enemyHp.OnDamage(attackPowerStat.currentValue);  // ������ ������ ����
                    Debug.Log($"������ {attackPowerStat.currentValue} �������� �������ϴ�.");
                }
            }
            else
            {
                Debug.LogWarning("���ݷ� ������ ã�� �� �����ϴ�.");
            }
            yield return new WaitForSeconds(0.65f);  // 0.65f�� ���
        }
    }

    private void Attacking(bool isAttacking)
    {
        AttackEvent?.Invoke(isAttacking);  // ���� �̺�Ʈ �߻�
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 origin = transform.position + new Vector3(0f, 1f);
        Vector2 direction = Vector2.right;

        Gizmos.DrawLine(origin, origin + direction * detectionRange);  // �� Ž�� ���� ǥ��

        if (enemyIsFront)
        {
            Gizmos.DrawSphere(origin + direction * detectionRange, 0.1f);
        }
    }

    public void LevelUpSword()
    {
        //swordStats.LevelUp();
        // ���� ������ ������ UI � �ݿ�
    }

    public void DisplaySwordStats()
    {
        // ������ ���� ������ UI � ǥ���ϴ� �Լ�
        foreach (var stat in swordStats.stats)
        {
            Debug.Log($"{stat.statName}: {stat.currentValue}");
        }
    }
}
