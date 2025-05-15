using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoNext.Events;

namespace ToDoNext.Views
{
    /// <summary>
    /// Interaction logic for LoginUC.xaml
    /// </summary>
    public partial class LoginUC : UserControl
    {
        ///
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private readonly IEventAggregator Aggregator;
        public LoginUC(IEventAggregator _aggregator)
        {
            InitializeComponent();
            Aggregator = _aggregator;
            ///
            /// <summary>
            /// 发布订阅事件
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            Aggregator.GetEvent<MsgEvent>().Subscribe(Sub);
        }



        /// <summary>
        /// 接收订阅事件
        /// </summary>
        /// <param name="obj"></param>
        private void Sub(MsgModel model)
        {
            MsgBar.MessageQueue.Enqueue(model.Msg);//加入显示队列
        }


    }
}
