using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToDoNext.DTOs;
using ToDoNext.Events;
using ToDoNext.Extensions;
using ToDoNext.HttpClient;
using ToDoNext.Models;
using ToDoNext.Service;

namespace ToDoNext.ViewModels
{
   public class IndexUCViewModel:BindableBase,INavigationAware
    {
        private readonly IEventAggregator Aggregator;

        public readonly HttpRestClient Client;
        //导航待办页
        private readonly IRegionManager RegionManager;
        public IRegionNavigationJournal Journal { get; set; }
        public DelegateCommand<IndexItemInfo> NavigateCommand { get; private set; }
        private DialogHostService DialogHostService { get; set; }
        public IndexUCViewModel(IRegionManager regionManager, HttpRestClient _client,DialogHostService _dialogHostService,
            IRegionNavigationJournal journal, IEventAggregator _Aggregator)
        {
            Aggregator = _Aggregator;
            Aggregator.ResgiterMessage(ShowMsg, "Main");
             //Aggregator.GetEvent<MsgEvent>().Subscribe(Show);
             Journal = journal;
            DialogHostService= _dialogHostService;
            UpdateToDoCommand=new DelegateCommand<ToDoDTO>(UpdateToDo);
            Client = _client;
            IndexItems = new List<IndexItemInfo>();
            CompleteCommand=new DelegateCommand<ToDoDTO>(Complete);
            ShowAddMemoCommand=new DelegateCommand(ShowAddMemoDialog);
            Create();
            UpdateMemoCommand=new DelegateCommand<MemoDTO>(UpdateMemo);
            CreateToDoList();
            CreateMemoList();
            RegionManager = regionManager;
            NavigateCommand = new DelegateCommand<IndexItemInfo>(Navigate);
            AddToDoCommand=new DelegateCommand(ShowAddDialog);
            GetIndexData();
            GetMemoData();
            Refresh();
        }
        private SnackbarMessageQueue _MessageQueue=new SnackbarMessageQueue();

        public SnackbarMessageQueue MessageQueue
        {
            get => _MessageQueue;
            set => SetProperty(ref _MessageQueue, value);
        }

        private void ShowMsg(MsgModel model)
        {
            MessageQueue.Enqueue(model.Msg);
        }

       

        public IndexUCViewModel()
        {
            Refresh();
            CreateToDoList();
            CreateMemoList();
        }

        private void Navigate(IndexItemInfo item)
        {
            NavigationParameters parameters = new NavigationParameters();
            if(item.Name == "汇总")
            {
                parameters.Add("SearchText", 0);
            }
            if(item.Name == "已完成")
            {
                parameters.Add("SearchText", 2);
            }
            
           if(!String.IsNullOrEmpty(item.ViewName))
            RegionManager.RequestNavigate("ContentRegion", item.ViewName,callback => {Journal=callback.Context.NavigationService.Journal;},parameters);
        }

        private string _title = "Welcome to Next ToDo";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        #region Items
        List<IndexItemInfo> _IndexItems;
        public List<IndexItemInfo> IndexItems
        {
            get { return _IndexItems; }
            set { SetProperty(ref _IndexItems, value); }
        }

        
        /// <summary>
        /// 获取待办首页统计数据
        /// </summary>
        /// <returns></returns>
        public IndexItemDataDTO IndexItemData { get; set; }=new IndexItemDataDTO();
        public void GetIndexData()
           
        {
            //List<IndexItemInfo> indexItems = new List<IndexItemInfo>();
            ApiRequest request = new ApiRequest();
            request.Url = "ToDo/GetToDo";
            request.Method = Method.Get;
            try
            {
                var response = Client.Execute(request);

                if (response.Code == 1)
                {
                    //解析返回的Data中的Json数据
                    IndexItemData=JsonConvert.DeserializeObject<IndexItemDataDTO>(response.Data.ToString());

                }
                else
                {
                    Aggregator.SendMessage(response.Msg);
                }
            }
            catch (Exception ex)
            { 
            
               
            }
           
        }

        

        /// <summary>
        /// 获取首页备忘录统计数据
        /// </summary>
        public int Memo { get; set; }
        private void GetMemoData()
        {
            //List<IndexItemInfo> indexItems = new List<IndexItemInfo>();
            ApiRequest request = new ApiRequest();
            request.Url = "Memo/Get";
            request.Method = Method.Get;
            try
            {
                var response = Client.Execute(request);

                if (response.Code == 1)
                {
                    //解析返回的Data中的Json数据
                    Memo = Convert.ToInt32(response.Data);
                }
            }
            catch (Exception ex)
            {
            }       

        }
        private void Refresh()
        {
            IndexItems[0].Value = IndexItemData.Total.ToString();
            IndexItems[1].Value = IndexItemData.Completed.ToString();
            IndexItems[2].Value = IndexItemData.FinishedRate;
            IndexItems[3].Value = Memo.ToString();
        }
        private void Create()
        {
            IndexItems.Add(new IndexItemInfo() { Icon = "ClockFast", Name = "汇总", Value ="10", ViewName = "ToDoUC", BackColor = "#FF4081" });
            IndexItems.Add(new IndexItemInfo() { Icon = "ClockCheckOutline", Name = "已完成", Value = "8", ViewName = "ToDoUC", BackColor = "#FF4081" });
            IndexItems.Add(new IndexItemInfo() { Icon = "ChartLineVariant", Name = "完成比例", Value = "100%", ViewName = "IndexUC", BackColor = "#FF4081" });
            IndexItems.Add(new IndexItemInfo() { Icon = "PlaylistStar", Name = "备忘录", Value = "12", ViewName = "MemoUC", BackColor = "#FF4081" });  
    }
        #endregion

        #region 待办事项

        public List<ToDoDTO> _ToDoList { get; set; }
        public List<ToDoDTO> ToDoList
        {
            get { return _ToDoList; }
            set { _ToDoList = value; 
                RaisePropertyChanged();}
        }

        private void CreateToDoList()
        {
            //ToDoList = new List<ToDoDTO>();
            //ToDoList.Add(new ToDoDTO() { Title = "吃饭", Content = "明天去吃饭" });
            //ToDoList.Add(new ToDoDTO() { Title = "吃饭", Content = "吃饭" });
            //ToDoList.Add(new ToDoDTO() { Title = "吃饭", Content = "去吃饭" });
            GetToDoList();
        }
        #endregion
        #region 备忘录
        public List<MemoDTO> _MemoList;
        public List<MemoDTO> MemoList
        {
            get { return _MemoList; }
            set { _MemoList = value;
                RaisePropertyChanged(); }
        }

        private void CreateMemoList()
        {
            MemoList = new List<MemoDTO>();
            getMemoList();
            //MemoList.Add(new MemoDTO() { Title = "吃1", Content = "明天去吃饭" });
            //MemoList.Add(new MemoDTO() { Title = "吃2", Content = "吃饭" });
            //MemoList.Add(new MemoDTO() { Title = "吃3", Content = "去吃饭" });
        }
        private void getMemoList()
        {
            ApiRequest request = new ApiRequest();
            request.Url = "Memo/GetMemoList";
            request.Method = Method.Get;
            try
            {
                var response = Client.Execute(request);
                if (response.Code == 1)
                {
                    //解析返回的Data中的Json数据
                    MemoList = JsonConvert.DeserializeObject<List<MemoDTO>>(response.Data.ToString());
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
        #region 待办事项
        /// <summary>
        /// 待办事项获取
        /// </summary>
        private void GetToDoList()
        {
            ApiRequest request = new ApiRequest();
            request.Url = "ToDo/GetWaitingToDo";
            request.Method = Method.Get;
            try
            {
                var response = Client.Execute(request);
                if (response.Code == 1)
                {
                    //解析返回的Data中的Json数据
                    ToDoList = JsonConvert.DeserializeObject<List<ToDoDTO>>(response.Data.ToString());
                }     
                else
                {
                    //MessageBox.Show(response.Msg);
                    Aggregator.SendMessage(response.Msg);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                Aggregator.SendMessage(ex.Message);
            }
        }
        #endregion
        #region 添加待办事项对话框显示
     

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand AddToDoCommand { get; private set; }
        private async void ShowAddDialog()
        {
           var result = await DialogHostService.ShowDialog("AddToDoUC", null);
            if(result.Result==ButtonResult.OK)
            {
                if(result.Parameters.ContainsKey("info"))
                {
                    ToDoDTO toDo = result.Parameters.GetValue<ToDoDTO>("info");
                    ToDoList.Add(toDo);
                    ApiRequest apiRequest = new ApiRequest();
                    apiRequest.Method = Method.Post;
                    apiRequest.Url = "ToDo/AddToDo";
                    apiRequest.Params = toDo;
                    var response = Client.Execute(apiRequest);
                    if (response.Code == 1)
                    {
                        //MessageBox.Show("添加成功");
                        //Aggregator.GetEvent<MsgEvent>().Publish(new MsgModel() { Msg = "待办事项添加成功" });
                        Aggregator.SendMessage("待办事项添加成功");
                        GetToDoList();
                        GetIndexData();
                        Refresh();
                    }
                    else
                    {
                        Aggregator.SendMessage(response.Msg);
                        //MessageBox.Show("添加失败");
                        //Aggregator.GetEvent<MsgEvent>().Publish(response.Msg);

                    }
                }
            }

            //if (callback.Result == ButtonResult.OK)
            //{
            
            //}

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            GetIndexData();
            GetToDoList();
            GetMemoData();
            getMemoList();
            Refresh();
            if (navigationContext.Parameters.ContainsKey("LoginName"))
            {
                string loginName = navigationContext.Parameters.GetValue<string>("LoginName");
                string[] week = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
                string dateTime = DateTime.Now.ToString("yyyy-MM-dd");
                Title = $"欢迎{loginName}，今天是{dateTime}，{week[(int)DateTime.Now.DayOfWeek]}。";
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        //public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        //{

        //}

        //public void OnNavigatedTo(NavigationContext navigationContext)
        //{
        //    if(navigationContext.Parameters.ContainsKey("LoginName"))
        //    {
        //        string loginName = navigationContext.Parameters.GetValue<string>("LoginName");
        //        string[] week=new string[] { "星期日","星期一", "星期二", "星期三", "星期四", "星期五", "星期六"};
        //        string dateTime = DateTime.Now.ToString("yyyy-MM-dd");
        //        Title = $"欢迎{loginName}，今天是{dateTime}，{week[(int)DateTime.Now.DayOfWeek]}。";
        //    }
        //}

        //public bool IsNavigationTarget(NavigationContext navigationContext)
        //{
        //  return true;
        //}

        //public void OnNavigatedFrom(NavigationContext navigationContext)
        //{

        //}


        #endregion
        public DelegateCommand<ToDoDTO> CompleteCommand { get; private set; }
        private void Complete(ToDoDTO dTO)
        {
            
            ApiRequest request = new ApiRequest();
            request.Url = "ToDo/UpdateToDo";
            request.Method = Method.Put;
            request.Params = dTO;
            try
            {
                var res = Client.Execute(request);
                if (res.Code == 1)
                {
                    //MessageBox.Show("操作成功");
                    //Aggregator.GetEvent<MsgEvent>().Publish(new MsgModel() { Msg = "待办事项完成成功" });
                    Aggregator.SendMessage("待办事项完成成功");
                    GetToDoList();
                    GetIndexData();
                    Refresh();
                }
                else
                {
                    //MessageBox.Show(res.Msg);
                    Aggregator.SendMessage(res.Msg);
                    //Aggregator.GetEvent<MsgEvent>().Publish(res.Msg);

                }
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 添加备忘录
        /// </summary>
        public DelegateCommand ShowAddMemoCommand { get; private set; }
        private async void ShowAddMemoDialog()
        {
            var result = await DialogHostService.ShowDialog("AddMemoUC", null);
            if(result.Result==ButtonResult.OK)
            {
                if(result.Parameters.ContainsKey("Memo"))
                {
                    MemoDTO memo = result.Parameters.GetValue<MemoDTO>("Memo");
                    MemoList.Add(memo);
                    ApiRequest request = new ApiRequest();
                    request.Url = "Memo/Add";
                    request.Method = Method.Post;
                    request.Params = memo;
                    try
                    {
                        var res = Client.Execute(request);
                        if (res.Code == 1)
                        {
                            //MessageBox.Show("添加成功");
                            //Aggregator.GetEvent<MsgEvent>().Publish(new MsgModel() { Msg = "备忘录添加成功" });
                            Aggregator.SendMessage("备忘录添加成功");

                           
                            GetMemoData();
                           getMemoList();
                            Refresh();
                        }
                        else
                        {
                            //MessageBox.Show(res.Msg);
                            //Aggregator.GetEvent<MsgEvent>().Publish(res.Msg);
                            Aggregator.SendMessage(res.Msg);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
        
                }
            }
        }
        public DelegateCommand<ToDoDTO> UpdateToDoCommand { get; private set; }
        public DelegateCommand<MemoDTO> UpdateMemoCommand { get; private set; }
        public async void UpdateMemo(MemoDTO dTO)
        {
            int id = dTO.Id;
            DialogParameters parameters = new DialogParameters();
            parameters.Add("Memo", dTO);
            var result = await DialogHostService.ShowDialog("EditMemoUC", parameters);
            if (result.Result == ButtonResult.OK)
            {
                if (result.Parameters.ContainsKey("info"))
                {
                    MemoDTO memo = result.Parameters.GetValue<MemoDTO>("info");
                }
                ApiRequest request = new ApiRequest();
                request.Url = $"Memo/Update?id={id}";
                request.Method = Method.Put;
                request.Params = dTO;
                try
                {
                    var res = Client.Execute(request);
                    if (res.Code == 1)
                    {
                        //MessageBox.Show("操作成功");
                        //Aggregator.GetEvent<MsgEvent>().Publish(new MsgModel() { Msg = "备忘录更新成功" });
                        Aggregator.SendMessage("备忘录更新成功");


                        getMemoList();
                       
                    }
                    else
                    {
                        //MessageBox.Show(res.Msg);
                        //Aggregator.GetEvent<MsgEvent>().Publish(new MsgModel() { Msg = res.Msg });
                        Aggregator.SendMessage(res.Msg);

                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        private async void UpdateToDo(ToDoDTO dTO)
        {
            int id = dTO.Id;
            DialogParameters parameters = new DialogParameters();
            parameters.Add("ToDo", dTO);
            var result = await DialogHostService.ShowDialog("EditToDoUC", parameters);
           if(result.Result==ButtonResult.OK)
            {
                if(result.Parameters.ContainsKey("info"))
                {
                    ToDoDTO toDo = result.Parameters.GetValue<ToDoDTO>("info");
                }
                ApiRequest request = new ApiRequest();
                request.Url = $"ToDo/UpdateToDo?id={id}";
                request.Method = Method.Put;
                request.Params = dTO;
                try
                {
                    var res = Client.Execute(request);
                    if (res.Code == 1)
                    {
                        //MessageBox.Show("操作成功");
                        //Aggregator.GetEvent<MsgEvent>().Publish(new MsgModel() { Msg = "待办事项更新成功" });
                        Aggregator.SendMessage("待办事项更新成功");

                        GetToDoList();
                        GetIndexData();
                        Refresh();
                        
                    }
                    else
                    {
                        //MessageBox.Show(res.Msg);
                        //Aggregator.GetEvent<MsgEvent>().Publish(res.Msg);
                        Aggregator.SendMessage(res.Msg);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
