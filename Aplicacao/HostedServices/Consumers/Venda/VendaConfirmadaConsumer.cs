using Newtonsoft.Json;
using Core.MessageBroker;
using Vendas.Application.Events.Vendas;
using Produtos.Application.Commands.ProdutoEstoque;
using Produtos.Application.Commands;

namespace AplicacaoGerenciamentoLoja.HostedServices.Consumers.Venda
{
    public class VendaConfirmadaConsumer : BaseConsumer
    {
        public VendaConfirmadaConsumer(IServiceProvider provider, IConfiguration configuration) : base(provider, configuration)
        {
        }

        protected override string QueueName => _configuration.GetSection("Queues").GetSection("VendaDomainSettings")["FilaVendaConfirmada"];
        private string FilaVendaAprovada => _configuration.GetSection("Queues").GetSection("VendaDomainSettings")["FilaVendaAprovada"];
        private string FilaVendaReprovada => _configuration.GetSection("Queues").GetSection("VendaDomainSettings")["FilaVendaReprovada"];

        protected override async Task ProcessarMensagens(IEnumerable<string> mensagens, CancellationToken token)
        {
            using (IServiceScope scope = _provider.CreateScope())
            {
                ProdutoCommandHandler handler = scope.ServiceProvider.GetRequiredService<ProdutoCommandHandler>();
                IMessageBrokerPublisher publisher = scope.ServiceProvider.GetRequiredService<IMessageBrokerPublisher>();

                foreach (var mensagem in mensagens)
                {
                    Console.WriteLine("EventoVendaConfirmada: " + mensagem);
                    var eventoDesserializado = JsonConvert.DeserializeObject<VendaConfirmadaEvent>(mensagem);

                    if (eventoDesserializado != null)
                    {
                        var comando = MapearEventoParaComando(eventoDesserializado.Produtos);
                        var success = await handler.Handle(comando, token);
                        if (success)
                        {
                            var mensagemConfirmacao = new VendaAprovadaEvent() { Id = eventoDesserializado.Id }.Serialize();
                            await publisher.Enqueue(FilaVendaAprovada, mensagemConfirmacao);
                            //await vHandler.Handle(mensagemConfirmacao, token);
                        }
                        else
                        {
                            var mensagemConfirmacao = new VendaReprovadaEvent() { Id = eventoDesserializado.Id }.Serialize();
                            await publisher.Enqueue(FilaVendaReprovada, mensagemConfirmacao);
                            //await vHandler.Handle(mensagemConfirmacao, token);
                        }
                    }
                }
            }
        }


        private BaixarEstoqueProdutoCommand MapearEventoParaComando(IEnumerable<ProdutoVendaEventItem> produtosEvento)
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

            return new BaixarEstoqueProdutoCommand()
            {
                Produtos = produtos,
            };
        }
    }
}
