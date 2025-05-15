using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using ToDoNext.DTOs;
using ToDoNext.Events;
using ToDoNext.Extensions;
using ToDoNext.HttpClient;

namespace ToDoNext.ViewModels
{
    public class ToDoUCViewModel : BindableBase,INavigationAware
    {
        private readonly IEventAggregator Aggregator;
        public DelegateCommand AddTodoCmd { get; private set; }
        private readonly HttpRestClient HttpRestClient;
        public ToDoUCViewModel(IEventAggregator _Aggregator)
        {
            
            Aggregator = _Aggregator;
            //Aggregator.GetEvent<MsgEvent>().Subscribe(ShowInfo);
            Aggregator.ResgiterMessage(ShowInfo, "ToDo");
            HttpRestClient = new HttpRestClient();
            ShowCmm=new DelegateCommand(ShowCmmExecute);
            SearchCmd = new DelegateCommand(GetToDoList);
            DeleteTodoCmd=new DelegateCommand<ToDoDTO>(DeleteTodo);
            ToDoList = new List<ToDoDTO>();
            AddTodoCmd = new DelegateCommand(AddTodo);
        }

        

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            int Status = 0;
            if (navigationContext.Parameters.ContainsKey("SearchText"))
            {
                Status = navigationContext.Parameters.GetValue<int>("SearchText");
            }

            FilterStatus = Status;

            CreateToDoList();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
      

        private string _content;
        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

        private int _status;
        public int Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }


        //消息显示框数据
        private SnackbarMessageQueue _messageQueue = new SnackbarMessageQueue();

        public SnackbarMessageQueue ToDoMessageQueue
        {
            get => _messageQueue;
            set => SetProperty(ref _messageQueue, value);
        }
        private void ShowInfo(MsgModel model)
        {
            ToDoMessageQueue.Enqueue(model.Msg.ToString());
        }
      
        private void AddTodo()
        {
            ToDoDTO Todo = new ToDoDTO() { Title = Title, Content = Content, Status = Status==0 ? false : true };
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = Method.Post;
            apiRequest.Url = "ToDo/AddToDo";
            apiRequest.Params = Todo;
            try
            {
                
                ApiResponse res = HttpRestClient.Execute(apiRequest);
                if (res.Code == 1)
                {
                    //Aggregator.GetEvent<MsgEvent>().Publish("待办事项添加成功！","ToDo");
                    //_msgEx.SendMessage(Aggregator,"待办事项添加成功！","ToDo");
                    Aggregator.SendMessage("待办事项添加成功！","ToDo");

                    //MessageQueue.Enqueue($"添加成功！");
                    IsRightDrawerOpen =false;
                    Title="";
                    Content="";
                    Status=0;
                    GetToDoList();
                }
                else
                {
                    //todo 显示错误信息
                    //MessageBox.Show(res.Msg.ToString());
                    //Aggregator.GetEvent<MsgEvent>().Publish(new MsgModel() { Msg = res.Msg.ToString(),Filter="ToDo" });
                    Aggregator.SendMessage(res.Msg.ToString(), "ToDo");
                }

            }
            catch (Exception ex)
            {

            }
        }

        public List<ToDoDTO> _ToDoList { get; set; }
        public List<ToDoDTO> ToDoList
        {
            get { return _ToDoList; }
            set
            {
                _ToDoList = value;
                RaisePropertyChanged();
            }
        }

        private void CreateToDoList()
        {
            GetToDoList();
            //ToDoList.Add(new ToDoDTO() { Title = "吃饭1", Content = "明天去吃饭" });
            //ToDoList.Add(new ToDoDTO() { Title = "吃饭2", Content = "吃饭" });
            //ToDoList.Add(new ToDoDTO() { Title = "吃饭", Content = "去吃饭" });
        }
        #region 右边弹窗显示
        private bool _IsRightDrawerOpen;

        public bool IsRightDrawerOpen
        {
            get { return _IsRightDrawerOpen; }
            set { SetProperty(ref _IsRightDrawerOpen , value); }
        }

        public DelegateCommand ShowCmm { get;  private set; }
        private void ShowCmmExecute()
        {
            IsRightDrawerOpen = !IsRightDrawerOpen;
        }
        #endregion
        #region 添加待办事项，刷新页面
        public string SearchText { get; set; }
        private int _FilterStatus;
        public int FilterStatus
        {
            get { return _FilterStatus; }
            set { SetProperty(ref _FilterStatus, value); }
         }
        
        public DelegateCommand SearchCmd { get; private set; }
        private Visibility _Visibility;
      

        public Visibility Visibility { get{return _Visibility;}
            set
            {
                SetProperty(ref _Visibility, value);
            }
        }
        private void GetToDoList()
        {
            ApiResponse res = new ApiResponse();
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method=Method.Get;
            if(FilterStatus==0)
            apiRequest.Url = $"ToDo/GetAllToDo?Title={SearchText}";
            else
            apiRequest.Url = $"ToDo/GetAllToDo?Title={SearchText}&Status={FilterStatus}";
            
            try
            {
                 res = HttpRestClient.Execute(apiRequest);
                if (res.Code==1)
                {
                    ToDoList = JsonConvert.DeserializeObject<List<ToDoDTO>>(res.Data.ToString());
                    Visibility=ToDoList.Count>0?Visibility.Hidden:Visibility.Visible;
                }
                else
                {
                    ToDoList=new List<ToDoDTO>();
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
        /// <summary>
        /// 删除待办事项
        /// </summary>
        /// 
       
        public DelegateCommand<ToDoDTO> DeleteTodoCmd { get; private set; }
        private void DeleteTodo(ToDoDTO ToDo)
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = Method.Delete;
            apiRequest.Url = $"ToDo/DeleteToDo?Id={ToDo.Id}";
            try
            {
                ApiResponse res = HttpRestClient.Execute(apiRequest);
                if (res.Code == 1)
                {
                    var result = MessageBox.Show("你确定删除该待办事项吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.OK)
                    {
                        GetToDoList();
                        Aggregator.SendMessage("待办事项删除成功！", "ToDo");
                    }
                }
                else
                {
                    //todo 显示错误信息
                    //MessageBox.Show();
                    //Aggregator.GetEvent<MsgEvent>().Publish(new MsgModel() { Msg = res.Msg.ToString(), Filter = "ToDo" });
                    Aggregator.SendMessage(res.Msg.ToString(), "ToDo");
                }
            }
            catch (Exception ex)
            { }
        }


    }
}
