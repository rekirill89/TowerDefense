namespace _Project.Scripts
{
    public class GlobalServices
    {
        public MetaCurrencyWallet MetaWallet { get; private set; }
        public SceneLoaderService SceneLoaderService { get; private set; }
        public SaveLoadService SaveLoadService { get; private set; }

        public GlobalServices(
            SceneLoaderService sceneLoaderService, 
            SaveLoadService saveLoadService,
            MetaCurrencyWallet metaWallet)
        {
            SceneLoaderService = sceneLoaderService;
            SaveLoadService = saveLoadService;
            MetaWallet = metaWallet;
        }
    }
}