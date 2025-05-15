using DryIoc;
using MyToDo.ViewModels;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Windows;
using ToDoNext.HttpClient;
using ToDoNext.Service;
using ToDoNext.ViewModels;
using ToDoNext.Views;

namespace ToDoNext
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// 
        /// 启动窗口
        /// </summary>
        /// <returns></returns>
        /// 
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        
            containerRegistry.RegisterDialog<LoginUC>();
            //注册HttpClient
            containerRegistry.GetContainer().Register<HttpRestClient>(made:Parameters.Of.Type<string>(serviceKey: "webUrl"));
            //注册各个菜单页面导航
            containerRegistry.RegisterForNavigation<IndexUC, IndexUCViewModel>();
            containerRegistry.RegisterForNavigation<ToDoUC, ToDoUCViewModel>();
            containerRegistry.RegisterForNavigation<MemoUC, MemoUCViewModel>();
            containerRegistry.RegisterForNavigation<SettingUC, SettingUCViewModel>();
            //注册设置页面的子页面
            containerRegistry.RegisterForNavigation<PersonnelUC, PersonnelUCViewModel>();
            containerRegistry.RegisterForNavigation<AboutUC, AboutUCViewModel>();
            containerRegistry.RegisterForNavigation<SetUpUC, SetUpUCViewModel>();
           //注册添加对话框
            containerRegistry.RegisterDialog<AddToDoUC>();
            //注册自定义对话框服务
            containerRegistry.Register<DialogHostService>();
            //注册添加备忘录对话框
            containerRegistry.RegisterForNavigation<AddMemoUC, AddMemoUCViewModel>();
            
            //注册编辑待办事项对话框
            containerRegistry.RegisterForNavigation<EditToDoUC, EditToDoUCViewModel>();
            //注册编辑备忘录对话框
            containerRegistry.RegisterForNavigation<EditMemoUC, EditMemoUCViewModel>();



        }
        //退出登录
        public static void Logout(IContainerProvider containerProvider)
        {
            Current.MainWindow.Hide();
            var dialog = containerProvider.Resolve<IDialogService>();
            dialog.ShowDialog("LoginUC", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Environment.Exit(0);
                    return;
                }
                
                 Current.MainWindow.Show();
                
            });
        }
        /// <summary>
        /// 初始化时检查是否登录，若未登录则弹出登录窗口
        /// </summary>
        protected override void OnInitialized()
        {
            var dialog = Container.Resolve<IDialogService>();
            dialog.ShowDialog("LoginUC", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Environment.Exit(0);
                    return;
                }
                //登录成功后初始化主窗口，从主界面获取数据上下文,
                //并跳转到主页
                var res=App.Current.MainWindow.DataContext as MainWindowViewModel;
                if(res!=null)
                {
                    if(callback.Parameters.ContainsKey("Name"))
                    {
                        string name = callback.Parameters.GetValue<string>("Name");
                        res.GotoIndex(name);
                    }
                    
                }
                base.OnInitialized();
            });

        }
    }
}
