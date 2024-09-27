using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralaxBackground : MonoBehaviour
{
    [SerializeField]
    [Range(-1f, 2f)]
    private float moveSpeed = .1f;
    private Material material;

    private Vector2 calcSpeed;

    public bool isStop;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        if (!isStop)
        {
            calcSpeed += moveSpeed * Time.deltaTime * Vector2.right;
        }

        material.SetTextureOffset("_MainTex", calcSpeed);
    }
}
