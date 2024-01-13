using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    [SerializeField] private PoolingList _poolingList;

    private Dictionary<string, Pool> _pools;

    public override void Init()
    {
        base.Init();
        _pools = new Dictionary<string, Pool>();
        CreatePool();
    }

    private void CreatePool()
    {
        foreach (var pair in _poolingList.items)
        {
            _pools.Add(pair.prefab.name, new Pool(pair.prefab, transform, pair.cnt));
        }
    }

    public PoolableMono Pop(string key)
    {
        return _pools[key].Pop();
    }

    public void Push(PoolableMono obj)
    {
        _pools[obj.name].Push(obj);
    }
}