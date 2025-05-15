using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoNext.Models;

namespace ToDoNext.ViewModels
{
    public class SettingUCViewModel:BindableBase
    {
        public SettingUCViewModel(IRegionManager _regionManager, IRegionNavigationJournal regionNavigationJournal)
        {
            RegionNavigationJournal = regionNavigationJournal;
            SwitchCommand = new DelegateCommand<SettingMenu>(Switch);
            RegionManager = _regionManager;
            Create();
            
        }

        

        private List<SettingMenu> _settingMenus;

        public List<SettingMenu> SettingMenus
        {
            get { return _settingMenus; }
            set { _settingMenus = value; 
                RaisePropertyChanged(); }
        }
        private void Create()
        {
            SettingMenus = new List<SettingMenu>();
            SettingMenus.Add(new SettingMenu() { Icon="Palette",Name="个性化",ViewName="PersonnelUC" });
            SettingMenus.Add(new SettingMenu() { Icon="Cog",Name="设置",ViewName="SetUpUC" });
            SettingMenus.Add(new SettingMenu() { Icon="Palette",Name="关于我们",ViewName="AboutUC" });
        }
        #region 设置页切换
        //记录导航页
        public IRegionNavigationJournal RegionNavigationJournal;
        public readonly IRegionManager RegionManager;
        public DelegateCommand<SettingMenu> SwitchCommand { get; private set; }
        private void Switch(SettingMenu menu)
        {
            RegionManager.RequestNavigate("SettingContentRegion", menu.ViewName, callBack =>
            {
                RegionNavigationJournal=callBack.Context.NavigationService.Journal;
            });
        }
        #endregion

    }
}
