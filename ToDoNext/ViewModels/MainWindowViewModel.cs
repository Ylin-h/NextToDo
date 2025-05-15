using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using System;
using System.Collections.Generic;
using ToDoNext.Models;
using ToDoNext.Test;
using ToDoNext.Views;

namespace ToDoNext.ViewModels
{
    
    public class MainWindowViewModel : BindableBase
    {
        //记录导航
        public IRegionNavigationJournal Journal;



        private string _title = "ToDoNext";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private List<MenuInfo> _MenuInfos;      

        public List<MenuInfo> MenuInfos
        {
            get { return _MenuInfos; }
            set { SetProperty(ref _MenuInfos, value); }
        }
        #region 回到首页
       
        public DelegateCommand<string> GoHomeCommand { get; private set; }
        #endregion
        private readonly IContainerProvider ContainerProvider;
        public MainWindowViewModel(IRegionManager _regionManager, IRegionNavigationJournal _journal, IContainerProvider containerProvider)
        {
            ContainerProvider=containerProvider;
            RegionManager = _regionManager;
            NavigateCommand = new DelegateCommand<MenuInfo>(Navigate);
            LoginOutCommand=new DelegateCommand(LoginOut);
            Journal = _journal;
            MenuInfos = new List<MenuInfo>();
            Create();
            GoBackCommand = new DelegateCommand(GoBack);
            GoForwardCommand = new DelegateCommand(GoForward);
            GoHomeCommand = new DelegateCommand<string>(GotoIndex);
            //RegionManager.RequestNavigate("ContentRegion", "IndexUC");
            //TestRest.Test();
        }

        private void LoginOut()
        {
            
            App.Logout(ContainerProvider);
        }

        //退出登录            
        public DelegateCommand LoginOutCommand { get; private set; }

        private void Create()
        {
            MenuInfos.Add(new MenuInfo() { Icon = "Home", Title = "首页", ViewName = "IndexUC" });
            MenuInfos.Add(new MenuInfo() { Icon = "NoteBookOutline", Title = "待办事项", ViewName = "ToDoUC" });
            MenuInfos.Add(new MenuInfo() { Icon = "NoteBookPlus", Title = "备忘录", ViewName = "MemoUC" });
            MenuInfos.Add(new MenuInfo() { Icon = "Cog", Title = "设置", ViewName = "SettingUC" });

        }
        #region 区域导航
        private readonly IRegionManager RegionManager;
        public DelegateCommand<MenuInfo> NavigateCommand { get; private set; }

        private void Navigate(MenuInfo obj)
        {
            if (string.IsNullOrEmpty(obj.ViewName))
            {
                return;
            }

            else
            {
                if (obj.ViewName == "SettingUC")
                {
                    RegionManager.RequestNavigate("ContentRegion", obj.ViewName, callback =>
                    {
                        Journal = callback.Context.NavigationService.Journal;
                    });
                    RegionManager.RequestNavigate("SettingContentRegion", "PersonnelUC", callback =>
                    {

                        Journal = callback.Context.NavigationService.Journal;
                    }

                        );//默认显示个性化
                }
                else
                {
                    NavigationParameters p = new NavigationParameters();
                    p.Add("Status", 0);
                    RegionManager.RequestNavigate("ContentRegion", obj.ViewName, callback =>
                    {
                        Journal = callback.Context.NavigationService.Journal;
                    }, p);

                }
                //RegionManager.Regions["ContentRegion"].RequestNavigate(obj.ViewName);
            }
        }
        #endregion
        //回退
        public DelegateCommand GoBackCommand { get; private set; }
        private void GoBack()
        {
            if (Journal.CanGoBack)
            {
                Journal.GoBack();
            }
        }
        //向前
        public DelegateCommand GoForwardCommand { get; private set; }
        private void GoForward()
        {
            if (Journal.CanGoForward)
            {
                Journal.GoForward();
            }
        }
        //登录进入首页面
        public void GotoIndex(string LoginName)
        {
            //从登录页接收用户名，并通过导航参数传递给首页
            NavigationParameters p = new NavigationParameters();
            p.Add("LoginName", LoginName);
            //RegionManager.RequestNavigate("ContentRegion", "IndexUC" );
            RegionManager.Regions["ContentRegion"].RequestNavigate("IndexUC", p);

        }
    }
}
