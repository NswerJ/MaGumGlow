using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRaycast : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;

    public LayerMask playerLayer;
    public float detectRange = 2;

    public bool IsDetected;

    public void Initialize(Monster monster)
    {
        _monster = monster;
    }

    void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.right, detectRange, playerLayer);

        Debug.DrawRay(transform.position, -Vector2.right * detectRange, Color.red);

        IsDetected = hit;

    }
}
