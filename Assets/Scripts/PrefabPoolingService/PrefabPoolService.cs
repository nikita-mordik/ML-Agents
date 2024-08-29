using System.Collections.Generic;
using UnityEngine;

namespace FreedLOW.MLAgents.PrefabPoolingService
{
    public class PrefabPoolService : IPrefabPoolService
    {
        private readonly Dictionary<ObjectType, Pool> _pools = new();

        private GameObject _poolContainer;

        public List<GameObject> PoolObjects { get; } = new(50);
        
        public void InitializePool(List<PoolInfo> poolInfos)
        {
            var emptyGO = new GameObject();

            _poolContainer ??= new GameObject("Pool");
            
            foreach (var poolInfo in poolInfos)
            {
                var container = Object.Instantiate(emptyGO, _poolContainer.transform, false);
                container.name = poolInfo.ObjectType.ToString();

                _pools[poolInfo.ObjectType] = new Pool(container.transform);

                for (int i = 0; i < poolInfo.Count; i++)
                {
                    var go = Object.Instantiate(poolInfo.Prefab, container.transform);
                    PoolObjects.Add(go);
                    _pools[poolInfo.ObjectType].Objects.Enqueue(go);
                }
            }
            
            Object.Destroy(emptyGO);
        }

        public GameObject GetObjectFromPool(ObjectType type)
        {
            if (!_pools.TryGetValue(type, out var pool)) 
                return null;

            var obj = pool.Objects.Count > 0
                ? pool.Objects.Dequeue()
                : Object.Instantiate(PoolObjects[0], _pools[type].Container);
            obj.SetActive(true);
            return obj;
        }

        public void BackObjectToPool(GameObject gameObject)
        {
            if (!gameObject.activeInHierarchy) 
                return;
            
            _pools[gameObject.GetComponent<IPoolObject>().Type].Objects.Enqueue(gameObject);
            gameObject.transform.position = _pools[gameObject.GetComponent<IPoolObject>().Type].Container.position;
            gameObject.SetActive(false);
        }

        public void CleanUp()
        {
            Object.Destroy(_poolContainer);
            _pools.Clear();
            PoolObjects.Clear();
        }
        
        public void Dispose()
        {
            CleanUp();
        }
    }
}