using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoNext.Events
{
    public class MsgModel
    {
        public string Msg { get; set; }
        public string Filter { get; set; }

    }
    class MsgEvent : PubSubEvent<MsgModel>
    {
       
    }
}
