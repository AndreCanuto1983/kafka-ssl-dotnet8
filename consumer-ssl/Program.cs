using Confluent.Kafka;
using System;

namespace consumer_ssl
{
    static class Program
    {
        private static readonly int numberOfRecords = 99;

        static void Main()
        {
            var baseDirectory = Environment.CurrentDirectory.Split("bin");
            Console.WriteLine(baseDirectory);

            var config = new ConsumerConfig
            {
                BootstrapServers = Consumer.BootstrapServers,
                GroupId = Consumer.GroupId,
                ClientId = Consumer.ClientId,
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = Consumer.SaslUsername,
                SaslPassword = Consumer.SaslPassword,
                SslEndpointIdentificationAlgorithm = SslEndpointIdentificationAlgorithm.None,
                SslCaLocation = baseDirectory[0] + Consumer.SslCaLocation,
                SslCertificateLocation = baseDirectory[0] + Consumer.SslCertificateLocation,
                SslKeyLocation = baseDirectory[0] + Consumer.SslKeyLocation,
            };

            var builder = new ConsumerBuilder<string, string>(config);

            using var consumer = builder.Build();
            consumer.Subscribe(Consumer.Topic);
            int init = 0;

            while (init <= numberOfRecords)
            {
                var record = consumer.Consume(TimeSpan.FromMilliseconds(1000));

                Console.WriteLine($"{init} - DateTime:{DateTime.Now} | Key:{record?.Message?.Key} | Value:{record?.Message?.Value}");

                init++;
            }
        }
    }
}