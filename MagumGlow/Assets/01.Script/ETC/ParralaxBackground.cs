using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGState
{
    environment,
    speedAffected
}

public class ParralaxBackground : MonoBehaviour
{
    private float _length, _startPos, test;
    public float parralaxEffect;

    public BGState bgState;

    public bool isStop;

    void Start()
    {
        _startPos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        if (bgState == BGState.environment)
            test += Time.deltaTime * -2 * parralaxEffect;
        else
        {
            if (!isStop)
            {
                test += Time.deltaTime * -100 * parralaxEffect;
            }
        }

        transform.position = new Vector3(_startPos + test, transform.position.y, transform.position.z);

        if (transform.position.x <= _startPos - _length || transform.position.x >= _startPos + 20) test = 0;
    }
}
