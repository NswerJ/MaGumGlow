using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicSwordPlayer : MonoBehaviour
{
    public MagicSwordStats swordStats;
    public TextMeshProUGUI playerName;  // 플레이어 이름 UI
    public TextMeshProUGUI damageViewer; // 데미지 표시 UI

    public LayerMask enemyLayer; // 적 레이어
    public float detectionRange = 2f;  // 적 탐지 범위
    public Slider hpUI;
    public float playerHealth;  // 플레이어 체력
    private Stat hpStat;

    public event Action<bool> AttackEvent;
    public event Action<bool> DieEvent;
    public bool criticalCheck = false;

    private bool enemyIsFront = false;  // 적이 앞에 있는지 여부
    private bool isDie = false;  // 죽음 상태 체크
    private Coroutine damageCoroutine;  // 적에게 데미지를 주기 위한 코루틴

    void Start()
    {
        hpStat = swordStats.stats.Find(stat => stat.statName == "생명력");

        InitializeSword();
        
    }

    void InitializeSword()
    {
        // GameManager에서 플레이어 이름을 가져와서 UI에 반영
        playerName.text = GameManager.Instance.playerData.playerName;
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

            // 적이 없을 경우 데미지 코루틴 중지
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;  // 코루틴 참조를 null로 설정
            }
        }

        if (isDie || Input.GetButtonDown("Jump"))
        {
            DieEvent?.Invoke(true);  // 죽음 이벤트 발생
        }
    }

    public void OnDamage(float damage)
    {
        Debug.Log("들어옴");
        hpStat.currentValue -= damage;
        PlayerHealthUpdate();

        if (playerHealth <= 0)
        {
            DieEvent?.Invoke(true); //쥭엇음
        }
    }

    public void PlayerHealthUpdate()
    {
        playerHealth = hpStat.currentValue;  // 플레이어 체력 업데이트

        hpUI.value = playerHealth;
    }

    private void CheckForEnemy()
    {
        Stat criticalPercentStat = swordStats.stats.Find(stat => stat.statName == "치명타확률");

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

                // 적에게 데미지를 주는 코루틴이 이미 실행 중이지 않으면 시작
                if (damageCoroutine == null)
                {
                    damageCoroutine = StartCoroutine(DealDamageAfterDelay(enemyHp));  // 딜레이 후 데미지 처리
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
        while (true)  // 반복적으로 데미지를 주기 위한 루프
        {

            Stat attackPowerStat = swordStats.stats.Find(stat => stat.statName == "공격력");
            Stat criticalPowerStat = swordStats.stats.Find(stat => stat.statName == "치명타데미지");
           

            if (attackPowerStat != null)
            {

                if (criticalCheck)
                {
                    enemyHp.OnDamage(attackPowerStat.currentValue * criticalPowerStat.currentValue);  // 적에게 데미지 적용
                    Debug.Log($"적에게 {attackPowerStat.currentValue * criticalPowerStat.currentValue} 데미지를 입혔습니다.");
                }
                else
                {
                    enemyHp.OnDamage(attackPowerStat.currentValue);  // 적에게 데미지 적용
                    Debug.Log($"적에게 {attackPowerStat.currentValue} 데미지를 입혔습니다.");
                }
            }
            else
            {
                Debug.LogWarning("공격력 스탯을 찾을 수 없습니다.");
            }
            yield return new WaitForSeconds(0.65f);  // 0.65f초 대기
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
