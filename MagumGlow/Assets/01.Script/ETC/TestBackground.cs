using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBackground : MonoBehaviour
{
    [SerializeField]
    [Range(-1f, 2f)]
    private float moveSpeed = .1f;
    private Material material;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        material.SetTextureOffset("_MainTex", moveSpeed * Time.time * Vector2.right);
    }
}
