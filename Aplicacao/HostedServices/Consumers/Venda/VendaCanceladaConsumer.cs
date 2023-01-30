using Newtonsoft.Json;
using Core.MessageBroker;
using Vendas.Application.Events.Vendas;
using Produtos.Application.Commands.ProdutoEstoque;
using Produtos.Application.Commands;

namespace AplicacaoGerenciamentoLoja.HostedServices.Consumers.Venda
{
    public class VendaCanceladaConsumer : BaseConsumer
    {
        public VendaCanceladaConsumer(IServiceProvider provider, IConfiguration configuration) : base(provider, configuration)
        {
        }

        protected override string QueueName => _configuration.GetSection("Queues").GetSection("VendaDomainSettings")["FilaVendaCancelada"];

        protected override async Task ProcessarMensagens(IEnumerable<string> mensagens, CancellationToken token)
        {
            using (IServiceScope scope = _provider.CreateScope())
            {
                ProdutoCommandHandler handler = scope.ServiceProvider.GetRequiredService<ProdutoCommandHandler>();
                IMessageBrokerPublisher publisher = scope.ServiceProvider.GetRequiredService<IMessageBrokerPublisher>();

                foreach (var mensagem in mensagens)
                {
                    Console.WriteLine("EventoVendaCancelada: " + mensagem);
                    var eventoDesserializado = JsonConvert.DeserializeObject<VendaCanceladaEvent>(mensagem);

                    if (eventoDesserializado != null)
                    {
                        var comando = MapearEventoParaComando(eventoDesserializado.Produtos);
                        await handler.Handle(comando, token);
                    }
                }
            }
        }


        private ReporEstoqueProdutoCommand MapearEventoParaComando(IEnumerable<ProdutoVendaEventItem> produtosEvento)
        {
            IList<EstoqueProduto> produtos = new List<EstoqueProduto>();
            foreach (var produto in produtosEvento)
            {
                var p = new EstoqueProduto()
                {
                    ProdutoId = produto.ProdutoId,
                    Quantidade = produto.Quantidade
                };
                produtos.Add(p);
            }

            return new ReporEstoqueProdutoCommand()
            {
                Produtos = produtos,
            };
        }
    }
}
