using Core.Commands;
using Core.Infrastructure;
using Core.MessageBroker;
using Microsoft.Extensions.Options;
using Vendas.Application.Events.Vendas;
using Vendas.Domain;
using Vendas.Domain.Model;
using Vendas.Domain.Repository;
using static Vendas.Domain.Model.StatusVenda;

namespace Vendas.Application.Commands
{
    public class VendaCommandHandler : ICommandHandler<CriarVendaCommand, bool>,
                                         ICommandHandler<AtualizarVendaCommand, bool>,
                                         ICommandHandler<CancelarVendaCommand, bool>,
                                         ICommandHandler<ProcessarVendaCommand, bool>,
                                         ICommandHandler<AdicionarItemVendaCommand, bool>,
                                         ICommandHandler<RemoverItemVendaCommand, bool>,
                                         ICommandHandler<AtualizarItemVendaCommand, bool>
    {

        private readonly IUnitOfWork<Venda> _unitOfWork;
        private readonly IVendaRepository _repository;
        private readonly IMessageBrokerPublisher _publisher;
        private readonly VendaDomainSettings _settings;
        public VendaCommandHandler(IUnitOfWork<Venda> unitOfWork, IVendaRepository repository, IMessageBrokerPublisher publisher, IOptions<VendaDomainSettings> settings)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _publisher = publisher;
            _settings = settings.Value;
        }

        public async Task<bool> Handle(CriarVendaCommand command, CancellationToken token)
        {
            int row = 0;
            _unitOfWork.Begin();
            var clientes = await _repository.BuscarClientePorId(command.ClienteId, token);
            if (clientes.Any())
            {
                var cliente = clientes.First();
                var venda = Venda.CriarVenda(cliente);
                command.Id = venda.Id;

                try
                {
                    row = await _repository.CadastrarVenda(venda, token);
                }
                catch (Exception)
                {
                    _unitOfWork.CloseConnection();
                    throw;
                }

            }

            return row > 0;
        }

        public async Task<bool> Handle(AtualizarVendaCommand command, CancellationToken token)
        {
            int row = 0;
            _unitOfWork.Begin();
            var vendas = await _repository.BuscarVendaPorId(command.Id, token);
            if (vendas.Any())
            {
                var venda = vendas.First();
                venda.AtualizarDadosVenda(command.Desconto, command.FormaDePagamento);
                try
                {
                    row = await _repository.AtualizarVenda(venda, token);
                }
                catch (Exception)
                {
                    _unitOfWork.CloseConnection();
                    throw;
                }
            }
            return row > 0;
        }

        public async Task<bool> Handle(CancelarVendaCommand command, CancellationToken token)
        {
            int row = 0;
            _unitOfWork.Begin();
            var vendas = await _repository.BuscarVendaPorId(command.Id, token);
            if (vendas.Any())
            {
                try
                {
                    var venda = vendas.First();
                    venda.CancelarVenda();

                    row = await _repository.AtualizarVenda(venda, token);
                    if (row > 0)
                    {
                        var produtos = new List<ProdutoVendaEventItem>();
                        foreach (var item in venda.Items)
                        {
                            produtos.Add(new ProdutoVendaEventItem(item.Produto.Id, item.Quantidade));
                        }

                        var mensagem = new VendaCanceladaEvent(venda.Id, produtos);
                        await _publisher.Enqueue(_settings.FilaVendaCancelada, mensagem.Serialize());
                    }
                }
                catch (Exception)
                {
                    _unitOfWork.CloseConnection();
                    throw;
                }
            }
            return row > 0;
        }

        public async Task<bool> Handle(AdicionarItemVendaCommand command, CancellationToken token)
        {
            int row = 0;
            _unitOfWork.Begin();
            var vendas = await _repository.BuscarVendaPorId(command.VendaId, token);
            var produtos = await _repository.BuscarProdutoPorId(command.ProdutoId, token);
            if (vendas.Any() && produtos.Any())
            {
                var item = ItemVenda.CriarItemVenda(vendas.First(), produtos.First(), command.Quantidade, produtos.First().Preco);
                var venda = vendas.First();
                venda.AdicionarItemAVenda(item);
                try
                {
                    row = await _repository.AdicionarProdutoEmVenda(item, token);
                }
                catch (Exception)
                {
                    _unitOfWork.CloseConnection();
                    throw;

                }
            }
            return row > 0;
        }

        public async Task<bool> Handle(RemoverItemVendaCommand command, CancellationToken token)
        {
            int row = 0;
            _unitOfWork.Begin();
            var vendas = await _repository.BuscarVendaPorId(command.VendaId, token);
            if (vendas.Any())
            {
                var venda = vendas.First();

                var itens = venda.RemoverItemVenda(command.ProdutoId);
                if (itens.Count() > 0)
                {

                    try
                    {
                        var item = itens.First();
                        row = await _repository.RemoverProdutoEmVenda(item, token);
                    }
                    catch (Exception)
                    {
                        _unitOfWork.CloseConnection();
                        throw;

                    }
                }
            }
            return row > 0;
        }

        public async Task<bool> Handle(AtualizarItemVendaCommand command, CancellationToken token)
        {
            int row = 0;
            _unitOfWork.Begin(true);
            var vendas = await _repository.BuscarVendaPorId(command.VendaId, token);
            if (vendas.Any())
            {
                var venda = vendas.First();

                var itens = venda.Items.Where(i => i.Produto.Id == command.ProdutoId).ToList();
                if (itens.Any())
                {
                    itens.First().AtualizarQuantidadeItemVenda(command.Quantidade);
                    try
                    {
                        row = await _repository.AtualizarProdutoEmVenda(itens.First(), token);
                    }
                    catch (Exception)
                    {
                        _unitOfWork.CloseConnection();
                        throw;

                    }
                }
            }
            return row > 0;
        }

        public async Task<bool> Handle(ProcessarVendaCommand command, CancellationToken token)
        {
            int row = 0;
            _unitOfWork.Begin();
            var vendas = await _repository.BuscarVendaPorId(command.Id, token);
            if (vendas.Any())
            {
                try
                {
                    var venda = vendas.First();
                    venda.ProcessarVenda();
                    row = await _repository.AtualizarVenda(venda, token);

                    if (row > 0)
                    {
                        var produtos = new List<ProdutoVendaEventItem>();
                        foreach (var item in venda.Items)
                        {
                            produtos.Add(new ProdutoVendaEventItem(item.Produto.Id, item.Quantidade));
                        }
                        //Enviar Quantidades negativas
                        var mensagem = new VendaConfirmadaEvent(venda.Id, produtos);
                        await _publisher.Enqueue(_settings.FilaVendaConfirmada, mensagem.Serialize());
                        //Evento Para Diminuir Estoque no Dominio Produto
                    }

                }
                catch (Exception)
                {
                    _unitOfWork.CloseConnection();
                    throw;

                }

            }

            return row > 0;
        }
    }
}
