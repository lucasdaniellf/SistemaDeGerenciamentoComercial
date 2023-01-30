using Newtonsoft.Json;
using Vendas.Application.Events.Vendas;
using Vendas.Application.Events;

namespace AplicacaoGerenciamentoLoja.HostedServices.Consumers.Venda
{
    public class VendaReprovadaConsumer : BaseConsumer
    {
        public VendaReprovadaConsumer(IServiceProvider provider, IConfiguration configuration) : base(provider, configuration)
        {
        }

        protected override string QueueName => _configuration.GetSection("Queues").GetSection("VendaDomainSettings")["FilaVendaReprovada"];

        protected override async Task ProcessarMensagens(IEnumerable<string> mensagens, CancellationToken token)
        {
            using (IServiceScope scope = _provider.CreateScope())
            {
                VendaEventHandler handler = scope.ServiceProvider.GetRequiredService<VendaEventHandler>();
                foreach (var mensagem in mensagens)
                {
                    Console.WriteLine("EventoVendaReprovada: " + mensagem);
                    var deserialized = JsonConvert.DeserializeObject<VendaReprovadaEvent>(mensagem);
                    if (deserialized != null)
                    {
                        await handler.Handle(deserialized, token);
                    }
                }
            }
        }
    }
}
