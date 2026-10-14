using System;
using UnityEngine;

namespace _Project.Scripts
{
    [CreateAssetMenu]
    public class GlobalServicesRef : ScriptableObject
    {
        public GlobalServices GlobalServices { get; private set; }

        public void Init(
            SceneLoaderService sceneLoaderService, 
            SaveLoadService saveLoadService,
            MetaCurrencyWallet metaWallet)
        {
            GlobalServices = new GlobalServices(sceneLoaderService, saveLoadService, metaWallet);
        }

        private void OnDestroy()
        {
            GlobalServices = null;
        }
    }
}