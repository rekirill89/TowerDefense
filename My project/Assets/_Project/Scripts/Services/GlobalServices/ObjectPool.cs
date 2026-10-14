using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    public sealed class ObjectPool<T> where T : MonoBehaviour
    {
        private readonly Stack<T> _pool;
        private readonly T _prefab;
        
        private readonly int _capacity;

        public ObjectPool(T prefab, int capacity = 8)
        {
            _capacity = capacity;
            _prefab = prefab;
            _pool = new Stack<T>(_capacity);
        }

        public T Get()
        {
            if (_pool.Count <= 0)
            {
                for (int i = 0; i < _capacity; i++)
                {
                    var x = Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity);
                    x.gameObject.SetActive(false);
                    _pool.Push(x);
                }
            }
            var obj = _pool.Pop();
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            _pool.Push(obj);
        }
    }
}