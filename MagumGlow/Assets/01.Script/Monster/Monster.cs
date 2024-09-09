using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IPoolable
{
    private Dictionary<Type, IMonsterComponent> _components;

    [SerializeField] private PoolTypeSO _poolType;
    public PoolTypeSO PoolType => _poolType;

    public GameObject GameObject => gameObject;

    private void Awake()
    {
        _components = new Dictionary<Type, IMonsterComponent>();

        IMonsterComponent[] compoArr = GetComponentsInChildren<IMonsterComponent>();
        foreach(var component in compoArr)
        {
            _components.Add(component.GetType(), component);
        }

        foreach(IMonsterComponent compo in _components.Values)
        {
            compo.Initialize(this);
        }
    }   

    public T GetCompo<T>() where T : class
    {
        if (_components.TryGetValue(typeof(T), out IMonsterComponent compo))
            return compo as T;
        return default;
    }

    public void SetUpPool(Pool pool)
    {
        throw new NotImplementedException();
    }

    public void ResetItem()
    {
        throw new NotImplementedException();
    }
}
