using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;

    public LayerMask playerLayer;

    public float _detectRange= 3;

    private BackgroundManager _backgroundManager;

    public void Initialize(Monster monster)
    {
        _monster = monster;
        _backgroundManager = GameObject.Find("LevelManager").GetComponent<BackgroundManager>();
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.right, _detectRange, playerLayer);

        Debug.DrawRay(transform.position, -Vector2.right * _detectRange, Color.red);

        if (hit.collider != null)
        {
            _backgroundManager.Running(true);
        }
        else
        {
            _backgroundManager.Running(false);
        }
    }
}
