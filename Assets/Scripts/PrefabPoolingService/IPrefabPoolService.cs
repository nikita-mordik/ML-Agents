using System.Collections.Generic;
using FreedLOW.MLAgents.ServiceLocator;
using UnityEngine;

namespace FreedLOW.MLAgents.PrefabPoolingService
{
    public interface IPrefabPoolService : IService
    {
        List<GameObject> PoolObjects { get; }

        /// <summary>
        /// Initializing pool
        /// </summary>
        void InitializePool(List<PoolInfo> poolInfos);
        /// <summary>
        /// Get gameObject from Pool
        /// </summary>
        /// <param name="type">Type of GO which will be create</param>
        /// <returns></returns>
        GameObject GetObjectFromPool(ObjectType type);
        /// <summary>
        /// Return gameObject to Pool
        /// </summary>
        /// <param name="gameObject">GO which will be return</param>
        void BackObjectToPool(GameObject gameObject);
        /// <summary>
        /// Clean resources
        /// </summary>
        void CleanUp();
    }
}