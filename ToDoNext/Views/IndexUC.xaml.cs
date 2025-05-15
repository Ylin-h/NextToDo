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
    /// Interaction logic for IndexUC.xaml
    /// </summary>
    public partial class IndexUC : UserControl
    {
        //private readonly IEventAggregator Aggregator;
        public IndexUC()
        {
            InitializeComponent();
            //Aggregator =aggregator;
            //Aggregator.GetEvent<MsgEvent>().Subscribe(ShowMsg);
           
        }

        //private void ShowMsg(MsgModel obj)
        //{
        //    info.MessageQueue.Enqueue(obj.Msg);
        //}
    }
}
