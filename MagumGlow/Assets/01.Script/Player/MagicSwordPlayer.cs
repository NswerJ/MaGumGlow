using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MagicSwordPlayer : MonoBehaviour
{
    public MagicSwordStats swordStats;
    public TextMeshProUGUI playerName;  // �÷��̾� �̸� UI
    public TextMeshProUGUI damageViewer; // ������ ǥ�� UI

    public LayerMask enemyLayer; // �� ���̾�
    public float detectionRange = 2f;  // �� Ž�� ����
    public float playerHealth;  // �÷��̾� ü��

    public event Action<bool> AttackEvent;
    public event Action<bool> DieEvent;

    private bool enemyIsFront = false;  // ���� �տ� �ִ��� ����
    private bool isDie = false;  // ���� ���� üũ

    void Start()
    {
        InitializeSword();
    }

    void InitializeSword()
    {
        // GameManager���� �÷��̾� �̸��� �����ͼ� UI�� �ݿ�
        playerName.text = GameManager.Instance.playerData.playerName;

        // ���� ���� �ʱ�ȭ (�ʿ��ϸ� ���)
        /*foreach (var stat in swordStats.stats)
        {
            stat.currentValue = stat.baseValue;
        }*/
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
        }

        if (isDie || Input.GetButtonDown("Jump"))
        {
            DieEvent?.Invoke(true);  // ���� �̺�Ʈ �߻�
        }
    }

    private void PlayerHealthUpdate(float health)
    {
        playerHealth = health;  // �÷��̾� ü�� ������Ʈ
    }

    private void CheckForEnemy()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 1f, 0), Vector2.right, detectionRange, enemyLayer);

        if (hit.collider != null)
        {
            enemyIsFront = true;

            MonsterHP enemyHp = hit.collider.GetComponent<MonsterHP>();

            if (enemyHp != null)
            {
                enemyHp.Initialize(swordStats);
                StartCoroutine(DealDamageAfterDelay(enemyHp));  // ������ �� ������ ó��
            }
        }
        else
        {
            enemyIsFront = false;
        }
    }

    private IEnumerator DealDamageAfterDelay(MonsterHP enemyHp)
    {
        yield return new WaitForSeconds(0.3f);  // ���� ������

        Stat attackPowerStat = swordStats.stats.Find(stat => stat.statName == "���ݷ�");

        if (attackPowerStat != null)
        {
            enemyHp.OnDamage(attackPowerStat.currentValue);  // ������ ������ ����
        }
        else
        {
            Debug.LogWarning("���ݷ� ������ ã�� �� �����ϴ�.");
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
