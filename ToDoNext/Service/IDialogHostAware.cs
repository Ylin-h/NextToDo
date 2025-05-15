using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoNext.Service
{
    /// <summary>
    /// 自定义对话框接口
    /// </summary>
    public interface IDialogHostAware
    {
        /// <summary>
        /// DialoHost名称
        /// </summary>
        string DialogHostName { get; set; }
        public DelegateCommand ConfirmCommand { get; set; }
        public DelegateCommand CancelCommand { get;  set; }
        public void OnDialogOpened(IDialogParameters parameters);
       
    
    }
}
