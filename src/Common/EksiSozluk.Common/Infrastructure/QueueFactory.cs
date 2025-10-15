using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EksiSozluk.Common.Infrastructure
{
    // Hem webApi hem de Projections kısmında kullancagım için Common'a ekledim.
    public static class QueueFactory
    {
        public static void SendMessageToExchange(string exchangeName, string exchangeType, string queueName, object obj)
        {
            var channel = CreateBasicConsumer().EnsureExchange(exchangeName, exchangeType).EnsureQueue(queueName, exchangeName).Model;

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));

            channel.BasicPublish(exchange: exchangeName, routingKey: queueName, basicProperties: null, body: body);

            #region Kısaltılmış hali
            /*
             * Sadece alttaki kodla da RabbitMQ'ya publish edebilirdik.
             var factory = new ConnectionFactory();
            factory.Uri = new("amqps://pwayvqwa:FH59f9X2QwhP9LJjHAs4a3v8XAA-W-id@woodpecker.rmq.cloudamqp.com/pwayvqwa");
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange:exchangeName, type: exchangeType);
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
            channel.BasicPublish(exchange: exchangeName, routingKey: queueName, basicProperties: null, body: body);
             */
            #endregion
        }

        public static EventingBasicConsumer CreateBasicConsumer()
        {
            var factory = new ConnectionFactory();
            factory.Uri = new(SozlukConstants.RabbitMQHost);
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            return new EventingBasicConsumer(channel);
        }

        public static EventingBasicConsumer EnsureExchange(this EventingBasicConsumer consumer, string exchangeName, string exchangeType = SozlukConstants.DefaultExchangeType) // exchange'in olusturulmus olmasından emin olmak için yazıyoruz.
        {
            consumer.Model.ExchangeDeclare(exchange: exchangeName, type: exchangeType, durable: false, autoDelete: false);
            return consumer;
        }

        public static EventingBasicConsumer EnsureQueue(this EventingBasicConsumer consumer, string queueName, string exchangeName)
        {
            consumer.Model.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);

            consumer.Model.QueueBind(queueName, exchangeName, queueName);
            return consumer;
        }

        public static EventingBasicConsumer Receive<T>(this EventingBasicConsumer consumer, Action<T> act)
        {
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine("Received message: " + message);

                T obj = JsonSerializer.Deserialize<T>(message);
                act(obj);

                consumer.Model.BasicAck(ea.DeliveryTag, false);
            };

            return consumer;
        }

        /// <summary>
        // Bu metod, belirtilen kuyruktan mesajların alınmasını başlatır.
        // consumer.Model.BasicConsume kullanılarak belirtilen kuyruktan mesajlar alınmaya başlanır.
        //autoAck: false ile manuel onaylama kullanılır, yani mesajlar işlendikten sonra elle onaylanır.
        //Metod sonunda, değişikliklerin zincirlenebilmesi için consumer nesnesi geri döndürülür.
        /// </summary>
        public static EventingBasicConsumer StartingConsuming(this EventingBasicConsumer consumer, string queueName)
        {
            consumer.Model.BasicConsume(queue: queueName,
                autoAck: false,
                consumer: consumer);
            return consumer;
        }

    }
}
