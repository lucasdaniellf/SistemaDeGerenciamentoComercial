using Newtonsoft.Json;
using Vendas.Application.Events;
using Vendas.Application.Events.Vendas;

namespace AplicacaoGerenciamentoLoja.HostedServices.Consumers.Venda
{
    public class VendaAprovadaConsumer : BaseConsumer
    {
        public VendaAprovadaConsumer(IServiceProvider provider, IConfiguration configuration) : base(provider, configuration)
        {
        }

        protected override string QueueName => _configuration.GetSection("Queues").GetSection("VendaDomainSettings")["FilaVendaAprovada"];

        protected override async Task ProcessarMensagens(IEnumerable<string> mensagens, CancellationToken token)
        {
            using (IServiceScope scope = _provider.CreateScope())
            {
                VendaEventHandler handler = scope.ServiceProvider.GetRequiredService<VendaEventHandler>();
                foreach (var mensagem in mensagens)
                {
                    Console.WriteLine("EventoVendaAprovada: " + mensagem);
                    var deserialized = JsonConvert.DeserializeObject<VendaAprovadaEvent>(mensagem);
                    if (deserialized != null)
                    {
                        await handler.Handle(deserialized, token);
                    }
                }
            }
        }
    }
}
