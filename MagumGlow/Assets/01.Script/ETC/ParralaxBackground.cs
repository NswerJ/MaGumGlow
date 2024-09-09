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
    private float _length, _startPos, _td;
    
    public float parralaxEffect, monsterSpeed;

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

                if (bgState == BGState.monster)
                {
                    monsterSpeed += Time.deltaTime * parralaxEffect;
                    transform.position = new Vector3(transform.position.x + monsterSpeed, transform.position.y, transform.position.z);
                }
                else
                {
                    _td += Time.deltaTime * -75 * parralaxEffect;
                    transform.position = new Vector3(_startPos + _td, transform.position.y, transform.position.z);

                    if (transform.position.x <= _startPos - _length || transform.position.x >= _startPos + _length) _td = 0;
                }
            }
        }
    }
}
