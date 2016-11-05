using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Common
{
    public class MsgQueue
    {
        MessageQueue queue;
        public MsgQueue(string queuePath)
        {
            if (!MessageQueue.Exists(queuePath))
            {
                queue = MessageQueue.Create(queuePath);
            }
            else
            {
                queue = new MessageQueue(queuePath);
            }

        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="label">标识码</param>
        /// <param name="json">json数据</param>
        /// <param name="Priority">级别</param>
        public void SendMessage(string queuePath, string label, string json)
        {
            System.Messaging.Message message = new System.Messaging.Message();
            //为了避免存放消息队列的计算机重新启动而丢失消息，可以通过设置消息对象的Recoverable属性为true，
            //在消息传递过程中将消息保存到磁盘上来保证消息的传递，默认为false。 
            message.Recoverable = true;
            message.Label = label.ToString();
            message.Body = json;
            message.Formatter = new System.Messaging.XmlMessageFormatter(new Type[] { typeof(string) });
            queue.Send(message);
        }

        /// <summary>
        /// 连接消息队列并从队列中接收消息
        /// </summary>
        public string ReceiveMessage()
        {
            //连接到本地队列
            queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            //从队列中接收消息
            Message myMessage = queue.Receive(MessageQueue.InfiniteTimeout);
            var body = (string)myMessage.Body; //获取消息的内容
            return body;
        }

    }
}

