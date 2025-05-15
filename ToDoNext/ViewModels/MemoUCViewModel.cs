using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToDoNext.DTOs;
using ToDoNext.Events;
using ToDoNext.Extensions;
using ToDoNext.HttpClient;

namespace ToDoNext.ViewModels
{
    
    public class MemoUCViewModel:BindableBase
    {
        private readonly IEventAggregator Aggregator;
        public DelegateCommand<MemoDTO> DeleteMemoCommand { get; set; }
        private readonly HttpRestClient HttpRestClient;
        public DelegateCommand AddMemoCommand { get; set; }
        public MemoUCViewModel(IEventAggregator _Aggregator)
        {
            Aggregator = _Aggregator;
            //Aggregator.GetEvent<MsgEvent>().Subscribe(ShowInfo);
            Aggregator.ResgiterMessage(ShowInfo, "Memo");
            HttpRestClient=new HttpRestClient();
            DeleteMemoCommand = new DelegateCommand<MemoDTO>(DeleteMemo);
            AddMemoCommand =new DelegateCommand(AddMemo);
            CloseRightDrawerCommand=new DelegateCommand(CloseRightDrawer);
            SearchCommand=new DelegateCommand(GetMemoList);
            CreateMemoList();
            GetMemoList();
            RefreshMemo();
        }

        //消息显示框数据
        private SnackbarMessageQueue _messageQueue = new SnackbarMessageQueue();
        public SnackbarMessageQueue MemoMessageQueue
        {
            get => _messageQueue;
            set => SetProperty(ref _messageQueue, value);
        }
        private void ShowInfo(MsgModel obj)
        {
            MemoMessageQueue.Enqueue(obj.Msg);
        }
        private void DeleteMemo(MemoDTO dTO)
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = Method.Delete;
            apiRequest.Url = $"Memo/Delete?id={dTO.Id}";

            try
            {
                ApiResponse res = HttpRestClient.Execute(apiRequest);
                if (res.Code == 1)
                {
                    var result = MessageBox.Show("你确定要删除该备忘录吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.OK)
                    {
                        MemoList.Remove(dTO);
                        GetMemoList();
                        Aggregator.SendMessage("删除备忘录成功", "Memo");
                    }

                    else
                    {
                        return;
                    }
                }
                else
                {
                    //todo 显示错误信息
                    //MessageBox.Show(res.Msg.ToString());
                    //Aggregator.GetEvent<MsgEvent>().Publish(new MsgModel() { Msg = res.Msg.ToString(), Filter = "Memo" });
                    Aggregator.SendMessage(res.Msg.ToString(), "Memo");
                      
                }
            }
            catch (Exception ex)
            {
            }

        }
        #region 备忘录
        public List<MemoDTO> _MemoList { get; set; }
        public List<MemoDTO> MemoList
        {
            get { return _MemoList; }
            set
            {
                _MemoList = value;
                RaisePropertyChanged();
            }
        }
        private Visibility _Visibility;

        public Visibility Visibility
        {
            get { return _Visibility; }
            set { SetProperty(ref _Visibility, value); }
        }

        private void CreateMemoList()
        {
            GetMemoList();
            //MemoList = new List<MemoDTO>();
            //MemoList.Add(new MemoDTO() { Title = "吃1", Content = "明天去吃饭" });
            //MemoList.Add(new MemoDTO() { Title = "吃2", Content = "吃饭" });
            //MemoList.Add(new MemoDTO() { Title = "吃3", Content = "去吃饭" });
            //MemoList.Add(new MemoDTO() { Title = "吃4", Content = "吃饭" });
            //MemoList.Add(new MemoDTO() { Title = "吃5", Content = "吃饭" });   
            //MemoList.Add(new MemoDTO() { Title = "吃6", Content = "吃饭" });
          
        }
        #endregion
        #region 新增备忘录
       
        private bool _IsRightDrawerOpen;
        public bool IsRightDrawerOpen
        {
            get { return _IsRightDrawerOpen; }
            set
            {
                SetProperty(ref _IsRightDrawerOpen, value);
                
            }
        }
        /// <summary>
        /// 新增备忘录
        /// </summary>
        private void RefreshMemo()
        {
            MemoList.Add(new MemoDTO() { Title = this.Title, Content =this.Content });
            CreateMemoList();
            IsRightDrawerOpen = false;
        }
        /// <summary>
        /// 右侧抽屉关闭
        /// </summary>
        public DelegateCommand CloseRightDrawerCommand { get; set; }
        public void CloseRightDrawer()
        {
            IsRightDrawerOpen = !IsRightDrawerOpen;
        }
        #endregion
        public DelegateCommand SearchCommand { get; set; }
        public string? SearchTitle { get; set; }
        private void GetMemoList()
        {
            ApiResponse res = new ApiResponse();
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = Method.Get;
            apiRequest.Url = $"Memo/GetMemoList?Title={SearchTitle}";
            try
            {
                res = HttpRestClient.Execute(apiRequest);
                if (res.Code == 1)
                {
                    MemoList = JsonConvert.DeserializeObject<List<MemoDTO>>(res.Data.ToString());
                    Visibility = MemoList.Count > 0 ? Visibility.Hidden : Visibility.Visible;//当备忘录列表不为空时，隐藏空白提示
                }
                else
                {
                    MemoList = new List<MemoDTO>();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }
        private string _Content;
        public string Content
        {
            get { return _Content; }
            set { SetProperty(ref _Content, value); }
        }

        //public IEventAggregator Aggregator1 => Aggregator;

        private void AddMemo()
        {
            MemoDTO Todo = new MemoDTO();
            Todo.Title = Title;
            Todo.Content = Content;
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = Method.Post;
            apiRequest.Url = "Memo/Add";
            apiRequest.Params = Todo;
            try
            {
                ApiResponse res = HttpRestClient.Execute(apiRequest);
                if (res.Code == 1)
                {
                    //Aggregator.GetEvent<MsgEvent>().Publish(new MsgModel() { Msg = "添加备忘录成功" ,Filter="Memo"});
                    Aggregator.SendMessage("添加备忘录成功", "Memo");
                    IsRightDrawerOpen = false;
                    GetMemoList();
                    Title = "";
                    Content = "";
                }
                else
                {
                    Title = "";
                    Content = "";
                    //todo 显示错误信息
                    //MessageBox.Show(res.Msg.ToString());
                    Aggregator.SendMessage(res.Msg.ToString(), "Memo");
                    //Aggregator.GetEvent<MsgEvent>().Publish(new MsgModel() { Msg = res.Msg.ToString(), Filter = "Memo" });
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
