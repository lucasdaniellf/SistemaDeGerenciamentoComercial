using Core.Infrastructure;
using Produtos.Domain.Model;
using Produtos.Domain.Repository;
using Produtos.Infrastructure;
using Produtos.Query.DTO;

namespace Produtos.Query
{
    public class ProdutoQueryService
    {

        private readonly IUnitOfWork<Produto> _unitOfWork;
        private readonly IProdutoRepository _repository;

        public ProdutoQueryService(IUnitOfWork<Produto> unitOfWork, IProdutoRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<IEnumerable<ProdutoQueryDto>> BuscarProdutos(CancellationToken token)
        {
            _unitOfWork.Begin();
            return from produto in await _repository.BuscarProdutos(token) select MapQueryDto(produto);
        }
        public async Task<IEnumerable<ProdutoQueryDto>> BuscarProdutoPorDescricao(string descricao, CancellationToken token)
        {
            _unitOfWork.Begin();
            return from produto in await _repository.BuscarProdutoPorDescricao(descricao, token) select MapQueryDto(produto);
        }

        //==================================================================================================================//
        private async Task<IEnumerable<Produto>> BuscarProdutoPorIdNoRepositorio(string id, CancellationToken token)
        {
            _unitOfWork.Begin();
            return await _repository.BuscarProdutoPorId(id, token);
        }

        public async Task<IEnumerable<ProdutoQueryDto>> BuscarProdutoPorId(string id, CancellationToken token)
        {
            _unitOfWork.Begin();
            return from produto in await BuscarProdutoPorIdNoRepositorio(id, token) select MapQueryDto(produto);
        }
        //==================================================================================================================//

        private ProdutoQueryDto MapQueryDto(Produto produto)
        {
            return new ProdutoQueryDto(produto.Id, produto.Descricao, produto.Preco, MapEstoqueDto(produto.Estoque), produto.EstaAtivo);
        }

        private EstoqueDto MapEstoqueDto(Estoque estoque)
        {
            return new EstoqueDto(estoque.Quantidade, estoque.EstoqueMinimo);
        }
    }
}
