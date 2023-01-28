using Core.EventMessages;
using Core.Infrastructure;
using Vendas.Domain.Events.Cliente;
using Vendas.Domain.Events.Produto;
using Vendas.Domain.Events.Vendas;
using Vendas.Domain.Model;
using Vendas.Domain.Repository;
using static Vendas.Domain.Model.StatusVenda;

namespace Vendas.Domain.Events
{
    public class VendaEventHandler : IEventHandler<ClienteVendaAtualizadoEvent>,
                                     IEventHandler<ProdutoVendaAtualizadoEvent>,
                                     IEventHandler<VendaAprovadaEvent>,
                                     IEventHandler<VendaReprovadaEvent>
    {
        private readonly IUnitOfWork<Venda> _unitOfWork;
        private readonly IVendaRepository _repository;

        public VendaEventHandler(IUnitOfWork<Venda> unitOfWork, IVendaRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task Handle(ClienteVendaAtualizadoEvent request, CancellationToken token)
        {
            try
            {
                _unitOfWork.Begin();
                var cliente = new ClienteVenda(request.Id, request.EstaAtivo);

                var clientes = await _repository.BuscarClientePorId(request.Id, token);
                if (clientes.Any())
                {
                    await _repository.AtualizarCliente(cliente, token);
                } else
                {
                    await _repository.CadastrarCliente(cliente, token);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Concat(ex.Message, " - ", ex.StackTrace));
                _unitOfWork.CloseConnection();
            }
        }

        //---------------------------------------------------------------------------------------------------------------------//
        public async Task Handle(ProdutoVendaAtualizadoEvent request, CancellationToken token)
        {
            var produto = new ProdutoVenda(request.Id, request.Preco, request.QuantidadeEstoque, request.EstaAtivo);
            try
            {
                _unitOfWork.Begin();
                _unitOfWork.BeginTransaction();
                var produtos = await _repository.BuscarProdutoPorId(request.Id, token);
                if(produtos.Any())
                {
                    await _repository.AtualizarCadastroProduto(produto, token);
                    
                    if (produtos.First().Preco != produto.Preco) 
                    {
                        await AtualizarPrecoProdutosEmVendas(produto, token);
                    }
                } 
                else
                {
                    await _repository.CadastrarProduto(produto, token);
                }
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Concat(ex.Message, " - ", ex.StackTrace));
                _unitOfWork.Rollback();
            }
        }

        public async Task Handle(VendaAprovadaEvent request, CancellationToken token)
        {
            try
            {
                _unitOfWork.Begin();
                var venda = (await _repository.BuscarVendaPorId(request.Id, token)).First();
                venda.FinalizarVenda();
                await _repository.AtualizarVenda(venda, token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Concat(ex.Message, " - ", ex.StackTrace));
                _unitOfWork.CloseConnection();
            }
        }

        public async Task Handle(VendaReprovadaEvent request, CancellationToken token)
        {
            try
            {
                _unitOfWork.Begin();
                var venda = (await _repository.BuscarVendaPorId(request.Id, token)).First();
                venda.ReprovarVenda();
                await _repository.AtualizarVenda(venda, token);
            }
            catch(Exception ex)
            {
                Console.WriteLine(string.Concat(ex.Message, " - ", ex.StackTrace));
                _unitOfWork.CloseConnection();
            }
        }

        private async Task AtualizarPrecoProdutosEmVendas(ProdutoVenda produto, CancellationToken token)
        {
            var vendas = (await _repository.BuscarVendaPorStatusVenda(Status.PENDENTE, token)).Where(v => v.Items.Any(i => i.Produto.Id == produto.Id));
            foreach (var venda in vendas)
            {
                var selectedItem = from item in venda.Items where item.Produto.Id == produto.Id select item;
                if (selectedItem.Any())
                {
                    var item = selectedItem.First();
                    item.AtualizarValorPago(produto.Preco);
                    await _repository.AtualizarProdutoEmVenda(selectedItem.First(), token);
                }
            }
        }
    }
}
