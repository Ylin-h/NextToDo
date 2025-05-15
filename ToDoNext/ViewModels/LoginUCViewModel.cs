using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoNext.Views;
using Prism.Mvvm;
using Prism.Events;
using ToDoNext.Events;
using ToDoNext.HttpClient;
using ToDoNext.DTOs;
using RestSharp;
using Prism.Services.Dialogs;
namespace ToDoNext.ViewModels
{
    internal class LoginUCViewModel : BindableBase, IDialogAware
    {
        #region 订阅事件
        private readonly IEventAggregator Aggregator;
        #endregion
        //客服端请求
        private readonly HttpRestClient Client;
        public string Title { get;set; }="ToDoNext";
        private int _SelectedIndex;

        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set { _SelectedIndex = value; RaisePropertyChanged(); }
        }


        public event Action<IDialogResult> RequestClose;
        public DelegateCommand LoginOrRegisterCommand { get; set; }
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }
        public DelegateCommand LoginCommand { get; private set; }
        public DelegateCommand RegisterCommand { get; private set; }
        /// <summary>
        /// Initialize the view model
        /// </summary>
        public LoginUCViewModel(IEventAggregator aggregator, HttpRestClient client)
        {
            // Initialize the command
            LoginCommand = new DelegateCommand(Login);
            LoginOrRegisterCommand=new DelegateCommand(LoginOrRegister);
            RegisterCommand = new DelegateCommand(Register);
            Aggregator = aggregator;
            Client = client;
            MyVar=new AccRegDTO();
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="dTO"></param>
        private AccRegDTO _myVar;

        public AccRegDTO MyVar
        {
            get { return _myVar; }
            set { _myVar = value; 
            RaisePropertyChanged(); }
        }

        private void Register()
        {
            if(string.IsNullOrEmpty(MyVar.NickName) || string.IsNullOrEmpty(MyVar.Password) || string.IsNullOrEmpty(MyVar.ConfirmPassword))
            {
                Aggregator.GetEvent<MsgEvent>().Publish(new MsgModel() { Msg = "昵称、密码、确认密码不能为空", Filter = "Register" });
                return;
            }
            if(MyVar.Password!=MyVar.ConfirmPassword)
            {
                Aggregator.GetEvent<MsgEvent>().Publish(new MsgModel() { Msg = "密码和确认密码不一致", Filter = "Register" });
                return;
            }
            RegDTO dTO = new RegDTO()
            {
                AccountName = MyVar.NickName,
                NickName = MyVar.NickName,
                Password = MyVar.Password,
                
            };
            //RestClient client = new RestClient("http://localhost:24446/api/");
            //RestRequest request = new RestRequest("Account/Register", Method.Post);
            //request.AddHeader("Content-Type", "application/json");
            //request.AddJsonBody(dTO);
            //var response = client.Execute(request);
            ApiRequest request = new ApiRequest();
            request.Method = RestSharp.Method.Post;
            request.Url = "Account/Register";
            request.Params = dTO;
            var response = Client.Execute(request);
            if (response.Code == 1)
            {
                Aggregator.GetEvent<MsgEvent>().Publish(new MsgModel() { Msg = "注册成功", Filter = "Register" });
                SelectedIndex = 0;
                //清空注册信息
                MyVar = null;
            }
            
            else
            {
                Aggregator.GetEvent<MsgEvent>().Publish(new MsgModel() { Msg = response.Msg, Filter = "Register" });
                MyVar = null;
            }
        }

        private void LoginOrRegister()
        {
            if(SelectedIndex == 0){
                SelectedIndex=1;
            }
            else
            SelectedIndex=0;
        }

        private void Login()
        {

            if (RequestClose!= null&&!string.IsNullOrEmpty(Username) &&!string.IsNullOrEmpty(Password))
            {
                AccInfoDTO dTO = new AccInfoDTO()
                {
                    NickName = Username,
                    Password = Password
                };
                ApiRequest request = new ApiRequest();
                request.Method = RestSharp.Method.Post;
                request.Url = "Account/Login";
                request.Params = dTO;
                var re = Client.Execute(request);
                if (re.Code == 1)
                {
                    DialogParameters param = new DialogParameters();
                    param.Add("Name", dTO.NickName);
                    RequestClose(new DialogResult(ButtonResult.OK, param));
                
                }
                    
                else
                    Aggregator.GetEvent<MsgEvent>().Publish(new MsgModel() { Msg = re.Msg,Filter="Login" });
            }
            else
            {
                Aggregator.GetEvent<MsgEvent>().Publish(new MsgModel() { Msg = "用户名或密码不能为空", Filter = "Login" });
            }
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
        #region 密码
        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { SetProperty(ref _Password, value); 
                }
        }
        private string _Username;

        public string Username
        {
            get { return _Username; }
            set { SetProperty(ref _Username, value); }
        }



        #endregion

    }
}
