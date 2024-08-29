using System.Collections.Generic;
using FreedLOW.MLAgents.PrefabPoolingService;
using FreedLOW.MLAgents.ServiceLocator;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FreedLOW.MLAgents.Avoidables
{
    public class AvoidablesSpawner : MonoBehaviour
    {
        [SerializeField] private List<PoolInfo> poolInfos;
        [SerializeField] private ObjectType currentObjectType;
        [SerializeField] private LayerMask avoidableLayerMask;
        [SerializeField] private float spawnRadius = 10f;

        private int _avoidablePoolIndex;
        private float _spawnDelay = 0.15f;
        private float _spawnTimer;
        private bool _isSpawning = true;
        private int _activeAvoidables = 0;
        private int _maxAvoidables;
        private GameObject _container;

        private IPrefabPoolService _prefabPoolService;

        private void Start()
        {
            _prefabPoolService = AllServices.Container.Single<IPrefabPoolService>();
            _maxAvoidables = poolInfos[0].Count;
            
            InitializePool();
        }

        private void Update()
        {
            if (!_isSpawning)
                return;

            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= _spawnDelay && _activeAvoidables < _maxAvoidables)
            {
                _spawnTimer = 0f;
                Spawn();
            }
        }

        public void ResetSpawning()
        {
            for (int i = 0; i < _maxAvoidables; i++)
            {
                _prefabPoolService.BackObjectToPool(_prefabPoolService.PoolObjects[i]);
            }

            _activeAvoidables = 0;
            _avoidablePoolIndex = 0;
        }

        private void Spawn()
        {
            bool validSpawnLocationFound = false;
            int maxAttempts = 10;
            RaycastHit hit;
            var obstructionRadius = 10f;

            while (!validSpawnLocationFound)
            {
                var spawnPosition = Random.insideUnitSphere * spawnRadius;
                spawnPosition.y = transform.position.y + poolInfos[0].Prefab.transform.localScale.y / 2f;
                spawnPosition = transform.TransformPoint(spawnPosition);

                var raycastOrigin = spawnPosition + (Vector3.up * 2f);
                raycastOrigin = _container.transform.TransformPoint(raycastOrigin);

                if (Physics.SphereCast(raycastOrigin, obstructionRadius, Vector3.down,
                        out hit, 4f, avoidableLayerMask))
                {
                    maxAttempts--;

                    if (maxAttempts <= 0)
                        break;
                    
                    continue;
                }

                var objectFromPool = _prefabPoolService.GetObjectFromPool(currentObjectType);
                objectFromPool.transform.position = spawnPosition;

                _avoidablePoolIndex++;
                _activeAvoidables++;

                if (_avoidablePoolIndex >= _maxAvoidables)
                    _avoidablePoolIndex = 0;

                validSpawnLocationFound = true;
            }
        }

        private void InitializePool()
        {
            _prefabPoolService.InitializePool(poolInfos);
            _container = GameObject.Find(currentObjectType.ToString());
        }
    }
}