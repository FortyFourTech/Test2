using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dimar.Pools
{
    [Serializable]
    public class ObjectPoolItem
    {
        public GameObject objectToPool;
        public int amountToPool;
        // public bool shouldExpand;
    }

    public class PrefabPooler : Singleton<PrefabPooler>
    {
        public Action onFilled;
        private bool _filled = false;
        public bool IsFilled => _filled;

        public List<ObjectPoolItem> itemsToPool;
        private Dictionary<GameObject, List<Poolable>> _pools = new Dictionary<GameObject, List<Poolable>>();
        private List<Poolable> _queued = new List<Poolable>();

        public bool loadAsync = false;

        private void Start()
        {
            StartCoroutine(spawnPool());
        }

        private IEnumerator spawnPool()
        {
            foreach (var item in itemsToPool)
            {
                for (int i = 0; i < item.amountToPool; i++)
                {
                    if (loadAsync)
                        yield return null;
                    
                    _SpawnNewItem(item.objectToPool);
                }
            }

            _filled = true;
            onFilled?.Invoke();

            yield break;
        }

        // private GameObject spawnAdditional () {   
        // 	return spawnObject (tilePrefab);
        // }

        private Poolable _SpawnNewItem(GameObject itemPrefab)
        {
            var newTile = Instantiate(itemPrefab);

            var poolableCompo = newTile.GetOrAddComponent<Poolable>();
            poolableCompo.m_pool = this;
            poolableCompo.m_prefab = itemPrefab;
            poolableCompo.ReturnToPool();

            return poolableCompo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="pos">position to set before set object active</param>
        /// <param name="rot">rotation to set before set object active</param>
        /// <returns></returns>
        public Poolable QueueObject(
            GameObject prefab,
            Vector3 pos = default(Vector3),
            Quaternion rot = default(Quaternion)
        )
        {
            List<Poolable> pool;
            Poolable pooled;
            if (_pools.TryGetValue(prefab, out pool) && pool.Count > 0)
            {
                pooled = pool[pool.Count - 1];
                pool.RemoveAt(pool.Count - 1);
            }
            else
            {
                pooled = _SpawnNewItem(prefab);
                pool = _pools[prefab];
                pool.RemoveAt(pool.Count - 1);
            }

            if (pos != default(Vector3))
                pooled.transform.position = pos;
            if (rot != default(Quaternion))
                pooled.transform.rotation = rot;

            pooled.QueueFromPool();
            _queued.Add(pooled);

            return pooled;
        }

        public void ReturnObject(Poolable used)
        {
            List<Poolable> pool;

            if (_pools.ContainsKey(used.m_prefab))
            {
                pool = _pools[used.m_prefab];
            }
            else
            {
                pool = new List<Poolable>();
                _pools[used.m_prefab] = pool;
            }

            pool.Add(used);

            var parentName = used.m_prefab.name;
            var itemParent = transform.Find(parentName);
            if (itemParent == null)
            {
                var itemParentGo = new GameObject(parentName);
                itemParentGo.transform.SetParent(transform);
                itemParent = itemParentGo.transform;
            }
            used.transform.SetParent(itemParent);

            _queued.Remove(used);
        }

        public void returnAll() {
        	for (var i = _queued.Count-1; i >= 0; i--)
            {
        		var obj = _queued[i];
        		obj.ReturnToPool();
        	}
        }
    }
}
