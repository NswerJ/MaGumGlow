using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MagicSword : MonoBehaviour
{
    public MagicSwordStats swordStats;  
    public TextMeshProUGUI playerName;

    public LayerMask enemyLayer; 
    public float detectionRange = 2f;  
    public float playerHealth;  

    public event Action<bool> AttackEvent;
    public event Action<bool> DieEvent;

    private bool enemyIsFront = false;
    private bool isDie = false;

    void Start()
    {
        InitializeSword();
    }

    void InitializeSword()
    {
        playerName.text = swordStats.playerName;
        foreach (var stat in swordStats.stats)
        {
            stat.currentValue = stat.baseValue;
        }
    }

    private void Update()
    {
        CheckForEnemy();

        if (enemyIsFront)
        {
            Attacking(true);
        }
        else
        {
            Attacking(false);
        }

        if (isDie || Input.GetButtonDown("Jump"))
        {
            DieEvent?.Invoke(true);
        }


    }

    private void PlayerHealthUpdate(float health)
    {
        playerHealth = health;
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
                StartCoroutine(DealDamageAfterDelay(enemyHp));
            }
        }
        else
        {
            enemyIsFront = false;
        }
    }

    private IEnumerator DealDamageAfterDelay(MonsterHP enemyHp)
    {
        yield return new WaitForSeconds(0.3f);

        Stat attackPowerStat = swordStats.stats.Find(stat => stat.statName == "���ݷ�");

        if (attackPowerStat != null)
        {
            enemyHp.OnDamage(attackPowerStat.currentValue);
            Debug.Log($"���ݷ��� {attackPowerStat.currentValue}��ŭ �����.");
        }
        else
        {
            Debug.LogWarning("���ݷ� ������ ã�� �� �����ϴ�.");
        }
    }




    private void Attacking(bool isAttacking)
    {
        AttackEvent?.Invoke(isAttacking);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector2 origin = transform.position + new Vector3(0f, 1f);
        Vector2 direction = Vector2.right;

        Gizmos.DrawLine(origin, origin + direction * detectionRange);

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
