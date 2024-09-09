using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PoolManagerSO _monsterPoolManager;

    [SerializeField] private Transform _monsterTransform;

    private void Awake()
    {
        _monsterPoolManager.InitializePool(_monsterTransform);
    }
}
