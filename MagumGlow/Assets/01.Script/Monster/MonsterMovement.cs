using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;

    public LayerMask playerLayer;
    public float _detectRange = 3;

    private Rigidbody2D _rb;
    private float _currentSpeed;

    private BackgroundManager _backgroundManager;

    public void Initialize(Monster monster)
    {

        _monster = monster;
        _monster.GetCompo<MonsterHP>().Dead += HandleResetPos;

    }

    private void Awake()
    {

        _rb = GetComponent<Rigidbody2D>();
        _backgroundManager = GameObject.Find("LevelManager").GetComponent<BackgroundManager>();
    
    }

    private void HandleResetPos()
    {

        transform.position = transform.parent.position;
    
    }

    private void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.right, _detectRange, playerLayer);

        Debug.DrawRay(transform.position, -Vector2.right * _detectRange, Color.red);

        if (hit.collider != null)
        {
            _backgroundManager.Running(false);
            _currentSpeed = 0;
        }
        else
        {
            _backgroundManager.Running(true);
            _currentSpeed = -20;
        }
        _rb.velocity = new Vector2(_currentSpeed, 0);

    }
}
