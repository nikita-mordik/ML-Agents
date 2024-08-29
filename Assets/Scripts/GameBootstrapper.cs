using FreedLOW.MLAgents.PrefabPoolingService;
using FreedLOW.MLAgents.ServiceLocator;
using UnityEngine;

namespace FreedLOW.MLAgents
{
    public class GameBootstrapper : MonoBehaviour
    {
        private static AllServices _allServices;

        private void Awake()
        {
            _allServices = AllServices.Container;

            BindServices();
        }

        private static void BindServices()
        {
            _allServices.RegisterSingle<IPrefabPoolService>(new PrefabPoolService());
        }
    }
}