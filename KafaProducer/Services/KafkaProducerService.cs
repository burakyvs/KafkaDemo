using Confluent.Kafka;

namespace Kafka.Producer.Services
{
    public class KafkaProducerService
    {
        private IProducer<Null, string> _producer;
        public IProducer<Null, string> Producer { get { return _producer; } private init { _producer = value; } }

        public KafkaProducerService()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
            };

            Producer = new ProducerBuilder<Null, string>(config).Build();
        }
    }
}
