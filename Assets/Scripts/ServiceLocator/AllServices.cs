using System;

namespace FreedLOW.MLAgents.ServiceLocator
{
    public class AllServices
    {
        #region Singelton

        private static AllServices _instance;
        public static AllServices Container => _instance ??= new AllServices();

        #endregion

        public void RegisterSingle<TService>(TService implementation) where TService : IService => 
            Implementation<TService>.ServiceInstance = implementation;

        public TService Single<TService>() where TService : IService => 
            Implementation<TService>.ServiceInstance;

        public void RemoveSingle<TService>(TService dispose) where TService : IService
        {
            if (dispose is IDisposable disposableService) 
                disposableService.Dispose();

            Implementation<TService>.ServiceInstance = default;
        }

        private static class Implementation<TService> where TService : IService
        {
            public static TService ServiceInstance;
        }
    }
}