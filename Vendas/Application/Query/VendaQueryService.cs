﻿using Core.Infrastructure;
using Vendas.Domain.Model;
using Vendas.Domain.Repository;

namespace Vendas.Application.Query
{
    public class VendaQueryService
    {
        private readonly IUnitOfWork<Venda> _unitOfWork;
        private readonly IVendaRepository _repository;

        public VendaQueryService(IUnitOfWork<Venda> unitOfWork, IVendaRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<IEnumerable<VendaDto>> BuscarVendas(CancellationToken token)
        {
            try
            {
                _unitOfWork.Begin();
                var vendas = from venda in await _repository.BuscarVendas(token) select MapVenda(venda);
                _unitOfWork.CloseConnection();

                return vendas;
            }
            catch (Exception)
            {
                _unitOfWork.CloseConnection();
                throw;
            }
        }
        public async Task<IEnumerable<VendaDto>> BuscarVendasPorId(string Id, CancellationToken token)
        {
            try
            {
                _unitOfWork.Begin();
                var vendas = from venda in await _repository.BuscarVendaPorId(Id, token) select MapVenda(venda);
                _unitOfWork.CloseConnection();
                return vendas;
            }
            catch (Exception)
            {
                _unitOfWork.CloseConnection();
                throw;
            }
        }

        public async Task<IEnumerable<VendaDto>> BuscarVendasPorCliente(string clienteId, CancellationToken token)
        {

            try
            {
                _unitOfWork.Begin();
                var vendas = from venda in await _repository.BuscarVendasPorCliente(clienteId, token) select MapVenda(venda);
                _unitOfWork.CloseConnection();
                return vendas;
            }
            catch (Exception)
            {
                _unitOfWork.CloseConnection();
                throw;
            }

        }

        public async Task<IEnumerable<VendaDto>> BuscarVendasPorPeriodo(DateTime inicio, DateTime fim, CancellationToken token)
        {

            try
            {
                _unitOfWork.Begin();
                var vendas = from venda in await _repository.BuscarVendasPorData(inicio, fim, token) select MapVenda(venda);
                _unitOfWork.CloseConnection();
                return vendas;
            }
            catch (Exception)
            {
                _unitOfWork.CloseConnection();
                throw;
            }
        }

        public async Task<IEnumerable<ClienteDto>> BuscarClientes(CancellationToken token)
        {

            try
            {
                _unitOfWork.Begin();
                var clientes = from cliente in await _repository.BuscarClientes(token) select MapCliente(cliente);
                _unitOfWork.CloseConnection();
                return clientes;
            }
            catch (Exception)
            {
                _unitOfWork.CloseConnection();
                throw;
            }
        }

        public async Task<IEnumerable<ProdutoDto>> BuscarProdutos(CancellationToken token)
        {

            try
            {
                _unitOfWork.Begin();
                var produtos = from produto in await _repository.BuscarProdutos(token) select MapProduto(produto);
                _unitOfWork.CloseConnection();
                return produtos;
            }
            catch (Exception)
            {
                _unitOfWork.CloseConnection();
                throw;
            }
        }

        private VendaDto MapVenda(Venda venda)
        {
            List<VendaItemDto> items = new List<VendaItemDto>();
            foreach (var item in venda.Items)
            {
                items.Add(MapItemVenda(item));
            }
            return new VendaDto(venda.Id, venda.Cliente.Id, venda.DataVenda, venda.Desconto, venda.Status, venda.FormaDePagamento, items);
        }

        private VendaItemDto MapItemVenda(ItemVenda item)
        {
            return new VendaItemDto(item.Venda.Id, item.Produto.Id, item.ValorPago, item.Quantidade);
        }

        private ProdutoDto MapProduto(ProdutoVenda produto)
        {
            return new ProdutoDto(produto.Id, produto.Preco, produto.QuantidadeEstoque, (int)produto.EstaAtivo);
        }

        private ClienteDto MapCliente(ClienteVenda cliente)
        {
            return new ClienteDto(cliente.Id, (int)cliente.Status);
        }
    }
}
