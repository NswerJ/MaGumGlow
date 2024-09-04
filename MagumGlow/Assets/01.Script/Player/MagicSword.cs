using System;
using TMPro;
using UnityEngine;

public class MagicSword : MonoBehaviour
{
    public MagicSwordStats swordStats;  
    public TextMeshProUGUI playerName;

    public LayerMask enemyLayer; 
    public float detectionRange = 2f; 

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

    private void CheckForEnemy()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 1f, 0), Vector2.right , detectionRange, enemyLayer);

        enemyIsFront = hit.collider != null;
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
