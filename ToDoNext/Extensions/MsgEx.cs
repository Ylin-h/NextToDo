using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoNext.Events;

namespace ToDoNext.Extensions
{
    public static class MsgEx
    {
        public static void ResgiterMessage(this IEventAggregator aggregator,
            Action<MsgModel> action, string filterName = "Main")
        {
            aggregator.GetEvent<MsgEvent>().Subscribe(action,
                ThreadOption.PublisherThread, true, (m) =>
                {
                    return m.Filter.Equals(filterName);
                });
        }

        /// <summary>
        /// 发送提示消息
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="message"></param>
        public static void SendMessage(this IEventAggregator aggregator, string message, string filterName = "Main")
        {
            aggregator.GetEvent<MsgEvent>().Publish(new MsgModel()
            {
                Filter = filterName,
                Msg = message,
            });
        }
    }
}
