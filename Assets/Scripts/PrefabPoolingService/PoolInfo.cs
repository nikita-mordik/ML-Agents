using System;
using UnityEngine;

namespace FreedLOW.MLAgents.PrefabPoolingService
{
    [Serializable]
    public class PoolInfo
    {
        public ObjectType ObjectType;
        public int Count;
        public GameObject Prefab;
    }
}