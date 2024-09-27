using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;

    private MonsterRaycast _monsterRay;

    private Rigidbody2D _rb;
    private float _currentSpeed;

    private BackgroundManager _backgroundManager;

    public void Initialize(Monster monster)
    {

        _monster = monster;

        _monster.GetCompo<MonsterHP>().Dead += HandleResetPos;
        _monsterRay = _monster.GetCompo<MonsterRaycast>();

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

        _backgroundManager.Running(!_monsterRay.IsDetected);

        if (_monsterRay.IsDetected)
            _currentSpeed = 0;
        else
            _currentSpeed = -20;

        _rb.velocity = new Vector2(_currentSpeed, 0);

    }
}
