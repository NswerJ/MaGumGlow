using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MagicSwordPlayer : MonoBehaviour
{
    public MagicSwordStats swordStats;
    public TextMeshProUGUI playerName;  // 플레이어 이름 UI
    public TextMeshProUGUI damageViewer; // 데미지 표시 UI

    public LayerMask enemyLayer; // 적 레이어
    public float detectionRange = 2f;  // 적 탐지 범위
    public float playerHealth;  // 플레이어 체력

    public event Action<bool> AttackEvent;
    public event Action<bool> DieEvent;

    private bool enemyIsFront = false;  // 적이 앞에 있는지 여부
    private bool isDie = false;  // 죽음 상태 체크

    void Start()
    {
        InitializeSword();
    }

    void InitializeSword()
    {
        // GameManager에서 플레이어 이름을 가져와서 UI에 반영
        playerName.text = GameManager.Instance.playerData.playerName;

        // 마검 스탯 초기화 (필요하면 사용)
        /*foreach (var stat in swordStats.stats)
        {
            stat.currentValue = stat.baseValue;
        }*/
    }

    private void Update()
    {
        CheckForEnemy();  // 적 탐지

        if (enemyIsFront)
        {
            Attacking(true);  // 공격 상태
        }
        else
        {
            Attacking(false);  // 비공격 상태
        }

        if (isDie || Input.GetButtonDown("Jump"))
        {
            DieEvent?.Invoke(true);  // 죽음 이벤트 발생
        }
    }

    private void PlayerHealthUpdate(float health)
    {
        playerHealth = health;  // 플레이어 체력 업데이트
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
                StartCoroutine(DealDamageAfterDelay(enemyHp));  // 딜레이 후 데미지 처리
            }
        }
        else
        {
            enemyIsFront = false;
        }
    }

    private IEnumerator DealDamageAfterDelay(MonsterHP enemyHp)
    {
        yield return new WaitForSeconds(0.3f);  // 공격 딜레이

        Stat attackPowerStat = swordStats.stats.Find(stat => stat.statName == "공격력");

        if (attackPowerStat != null)
        {
            enemyHp.OnDamage(attackPowerStat.currentValue);  // 적에게 데미지 적용
        }
        else
        {
            Debug.LogWarning("공격력 스탯을 찾을 수 없습니다.");
        }
    }

    private void Attacking(bool isAttacking)
    {
        AttackEvent?.Invoke(isAttacking);  // 공격 이벤트 발생
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 origin = transform.position + new Vector3(0f, 1f);
        Vector2 direction = Vector2.right;

        Gizmos.DrawLine(origin, origin + direction * detectionRange);  // 적 탐지 영역 표시

        if (enemyIsFront)
        {
            Gizmos.DrawSphere(origin + direction * detectionRange, 0.1f);
        }
    }

    public void LevelUpSword()
    {
        //swordStats.LevelUp();
        // 이후 마검의 스탯을 UI 등에 반영
    }

    public void DisplaySwordStats()
    {
        // 마검의 현재 스탯을 UI 등에 표시하는 함수
        foreach (var stat in swordStats.stats)
        {
            Debug.Log($"{stat.statName}: {stat.currentValue}");
        }
    }
}
