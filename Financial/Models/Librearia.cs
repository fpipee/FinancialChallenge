using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Financial.Models
{
    public class Librearia
    {
        public IConnection GetConnection()
        {

            var factory = new ConnectionFactory() { HostName = "localhost" };

            return factory.CreateConnection();
        }
        public bool send(IConnection con, string message, string traza)
        {
            try
            {
                IModel channel = con.CreateModel();
                channel.ExchangeDeclare("messageexchange", ExchangeType.Direct);
                channel.QueueDeclare(traza, true, false, false, null);
                channel.QueueBind(traza, "messageexchange", traza, null);
                var msg = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("messageexchange", traza, null, msg);

            }
            catch (Exception)
            {


            }
            return true;

        }
        public string receive(IConnection con, string myqueue)
        {
            try
            {
                string queue = myqueue;
                IModel channel = con.CreateModel();
                channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                BasicGetResult result = channel.BasicGet(queue: queue, autoAck: true);
                if (result != null)
                    return Encoding.UTF8.GetString(result.Body);
                else
                    return null;
            }
            catch (Exception)
            {
                return null;

            }

        }
    }
}