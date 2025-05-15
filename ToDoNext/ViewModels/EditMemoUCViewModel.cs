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
    public class EditMemoUCViewModel : BindableBase, IDialogHostAware
    {
        
        public MemoDTO Memo { get; set; }
        public string DialogHostName { get; set; } = "RootDialog";
        public DelegateCommand ConfirmCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        private readonly IEventAggregator Aggregator;
        public EditMemoUCViewModel(IEventAggregator eventAggregator)
        {
            Aggregator = eventAggregator;
            ConfirmCommand = new DelegateCommand(Confirm);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Confirm()
        {
            if (string.IsNullOrEmpty(Memo.Title) || string.IsNullOrEmpty(Memo.Content))
            {
                //MessageBox.Show("标题和内容不能为空!");
                Aggregator.SendMessage( "标题和内容不能为空!");
                return;
            }
            else
            {
                if(DialogHost.IsDialogOpen(DialogHostName))
                {
                    DialogParameters parameters = new DialogParameters();  
                    parameters.Add("info", Memo);
                    DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, parameters));
                }
            }
        }
    private void Cancel()
    {
          if (DialogHost.IsDialogOpen(DialogHostName))
             DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));    
        }
    public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Memo"))
            {
                this.Memo = parameters.GetValue<MemoDTO>("Memo");
            }
        }
    }
}
