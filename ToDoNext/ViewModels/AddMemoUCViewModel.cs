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
    public class AddMemoUCViewModel : BindableBase, IDialogHostAware
    {
        private readonly IEventAggregator Aggregator;
        public AddMemoUCViewModel(IEventAggregator aggregator)
        {
            Aggregator=aggregator;
            ConfirmCommand = new DelegateCommand(Confirm);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Cancel()
        {
            //md里面对话框关闭
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogHost.Close(DialogHostName,new DialogResult(ButtonResult.No));
            }
        }

        private void Confirm()
        {
            if(!string.IsNullOrEmpty(Memo.Title) &&!string.IsNullOrEmpty(Memo.Content))
            {
                DialogParameters parameters = new DialogParameters();
                parameters.Add("Memo", Memo);
                if(DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, parameters));
            }
            else
            {
                //MessageBox.Show("信息不完整!");
                Aggregator.SendMessage("信息不完整!","Main");
                return;
            }
        }

        public MemoDTO Memo { get; set; }=new MemoDTO();
        public string DialogHostName { get; set; }="RootDialog";
        public DelegateCommand ConfirmCommand { get ; set; }
        public DelegateCommand CancelCommand { get; set; }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}
