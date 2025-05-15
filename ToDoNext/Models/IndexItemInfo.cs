using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoNext.Models
{
    /// <summary>
    /// 首页列表项信息
    /// </summary>
    public class IndexItemInfo:BindableBase
    {
        public string Icon { get; set; }
        public string Name { get; set; }
        public string _Value { get; set; }
        /// <summary>
        /// 首页列表项的值要通知绑定
        /// </summary>
        public string Value
        {
            get { return _Value; }
            set { _Value=value; RaisePropertyChanged(); }
        }

        public string ViewName { get; set; }
        public string BackColor { get; set; }
        public string Hand
        {
            get { if (Name == "完成比例")
                    return "";
                else
                    return "Hand"; }
        }
         
    }
}
