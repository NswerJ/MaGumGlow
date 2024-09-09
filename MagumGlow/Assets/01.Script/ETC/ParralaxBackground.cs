using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGState
{
    environment,
    speedAffected,
    monster
}

public class ParralaxBackground : MonoBehaviour
{
    private float _length, _startPos, _td, _monsterSpeed;
    public float parralaxEffect;

    public BGState bgState;

    public bool isStop;

    void Start()
    {
        _startPos = transform.position.x;
        if (bgState != BGState.monster)
            _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        if (bgState == BGState.environment)
            _td += Time.deltaTime * -2 * parralaxEffect;
        else
        {
            if (!isStop)
            {
                _td += Time.deltaTime * -100 * parralaxEffect;

                if (bgState == BGState.monster)
                {
                    if (_monsterSpeed < -1)
                        _monsterSpeed = 0;

                    _monsterSpeed += Time.deltaTime * parralaxEffect;
                    transform.position = new Vector3(transform.position.x + _monsterSpeed, transform.position.y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(_startPos + _td, transform.position.y, transform.position.z);

                    if (transform.position.x <= _startPos - _length || transform.position.x >= _startPos + _length) _td = 0;
                }
            }
        }
    }
}
