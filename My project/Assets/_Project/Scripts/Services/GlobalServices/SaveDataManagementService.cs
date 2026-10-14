using System;

namespace _Project.Scripts
{
    public class SaveDataManagementService
    {
        private readonly SaveLoadService _saveLoadService;
        private readonly MetaCurrencyWallet _metaWallet;

        public SaveDataManagementService(SaveLoadService saveLoadService, MetaCurrencyWallet metaWallet)
        {
            _saveLoadService = saveLoadService;
            _metaWallet = metaWallet;
        }

        public void Init()
        {
            var tempData = _saveLoadService.Load(); 
            _metaWallet.SetCurrentBalance(tempData.MetaCurrencyBalance);
        }
    }
}