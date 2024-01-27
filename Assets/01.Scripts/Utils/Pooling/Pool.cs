using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private Stack<PoolableMono> _pools;

    private readonly PoolableMono _prefab;
    private readonly Transform _parent;

    public Pool(PoolableMono prefab, Transform parent, int cnt)
    {
        if (prefab == null || parent == null)
        {
            Debug.LogError("[Pool] Invalid value exception");
            return;
        }

        _pools = new Stack<PoolableMono>();
        
        _prefab = prefab;
        _parent = parent;
        
        InitPoolSetting(cnt);
    }

    private void InitPoolSetting(int cnt)
    {
        for (var i = 0; i < cnt; i++)
        {
            var obj = Object.Instantiate(_prefab, _parent, true);
            obj.gameObject.name = obj.gameObject.name.Replace("(Clone)", "");
            obj.gameObject.SetActive(false);
            _pools.Push(obj);
        }
    }

    public PoolableMono Pop()
    {
        if (_pools.Count > 0)
        {
            var obj = _pools.Pop();
            obj.gameObject.SetActive(true);
            obj.OnPop();
            return obj;
        }
        else
        {
            var obj = Object.Instantiate(_prefab, _parent, true);
            obj.gameObject.name = obj.gameObject.name.Replace("(Clone)", "");
            obj.OnPop();
            return obj;
        }
    }

    public void Push(PoolableMono obj)
    {
        obj.OnPush();
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(_parent);
        _pools.Push(obj);
    }
}