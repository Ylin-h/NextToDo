using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToDoNext.DTOs;
using ToDoNext.Extensions;
using ToDoNext.Service;

namespace ToDoNext.ViewModels
{
    public class EditToDoUCViewModel : BindableBase, IDialogHostAware
    {
        private readonly IEventAggregator Aggregator;
        public EditToDoUCViewModel(IEventAggregator eventAggregator)
        {
            Aggregator = eventAggregator;
            ConfirmCommand = new DelegateCommand(Confirm);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Confirm()
        {
            if(!string.IsNullOrEmpty(ToDo.Title) &&!string.IsNullOrEmpty(ToDo.Content))
            {
                if (DialogHost.IsDialogOpen(DialogHostName))
                {
                    DialogParameters parameters = new DialogParameters();
                    parameters.Add("info", ToDo);
                    DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, parameters));
                }
            }
            else
            {
                //MessageBox.Show("信息不能为空");
                Aggregator.SendMessage( "标题和内容不能为空！");
                return;
            }
           
        }

        private void Cancel()
        {
            if(DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
            }
        }
        public ToDoDTO ToDo { get; set; }=new ToDoDTO();
        public string DialogHostName { get; set; } = "RootDialog";
        public DelegateCommand ConfirmCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("ToDo"))
            {
                this.ToDo = parameters.GetValue<ToDoDTO>("ToDo");
            }
        }
    }
}
