using Confluent.Kafka;
using System;

namespace producer_ssl
{
    static class Program
    {
        private static readonly int numberOfRecords = 99;

        static void Main()
        {
            var baseDirectory = Environment.CurrentDirectory.Split("bin");
            Console.WriteLine(baseDirectory[0]);

            var config = new ProducerConfig
            {
                BootstrapServers = Producer.BootstrapServers,
                ClientId = Producer.ClientId,
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = Producer.SaslUsername,
                SaslPassword = Producer.SaslPassword,
                SslEndpointIdentificationAlgorithm = SslEndpointIdentificationAlgorithm.None,
                SslCaLocation = baseDirectory[0] + Producer.SslCaLocation,
                SslCertificateLocation = baseDirectory[0] + Producer.SslCertificateLocation,
                SslKeyLocation = baseDirectory[0] + Producer.SslKeyLocation
            };

            var builder = new ProducerBuilder<string, string>(config);

            using var producer = builder.Build();

            int init = 0;

            while (init <= numberOfRecords)
            {
                producer.Produce(Producer.Topic, new Message<string, string>
                {
                    Key = $"{init}",
                    Value = $"Andre 198{init}"
                });

                init++;
            }
        }
    }
}
