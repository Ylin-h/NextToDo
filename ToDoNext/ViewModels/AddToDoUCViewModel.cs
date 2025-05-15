using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
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
using ToDoNext.Service;

namespace ToDoNext.ViewModels
{
    public class AddToDoUCViewModel : BindableBase, IDialogHostAware
    {
       
        private readonly IEventAggregator Aggregator;
        
        private readonly HttpRestClient HttpRestClient;
        public ToDoDTO ToDoDTO
        {
            get;
            set;
        }=new ToDoDTO();
        


        public AddToDoUCViewModel(HttpRestClient httpRestClient, IEventAggregator _Aggregator)
        {
            Aggregator = _Aggregator;
            HttpRestClient=httpRestClient;
            ConfirmCommand = new DelegateCommand(OnSave);
            CancelCommand=new DelegateCommand(OnCancel);

        }

        private void OnCancel()
        {
           if(DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
            }
        }

        //public string Title{ get; set; }="添加待办事项";

        public event Action<IDialogResult> RequestClose;

        
        public DelegateCommand ConfirmCommand { get;  set; }
        public string DialogHostName { get; set; } = "RootDialog";
       
        public DelegateCommand CancelCommand { get; set; }
        

        public void OnSave()
        {
            if(string.IsNullOrEmpty(ToDoDTO.Title) || string.IsNullOrEmpty(ToDoDTO.Content))
            {
                //MessageBox.Show("标题和内容不能为空");
                Aggregator.SendMessage("标题和内容不能为空");
                return;
            }
            else
            {
                if(DialogHost.IsDialogOpen(DialogHostName))
                {
                    DialogParameters parameters = new DialogParameters();
                    ToDoDTO  info = new ToDoDTO();
                    info.Title = ToDoDTO.Title;
                    info.Content = ToDoDTO.Content;
                    info.Status = ToDoDTO.Status;
                    parameters.Add("info", info);
                    DialogHost.Close(DialogHostName,new DialogResult(ButtonResult.OK,parameters));
                }
                
            }
           
        }


        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}
