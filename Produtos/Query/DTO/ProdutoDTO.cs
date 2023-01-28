using static Produtos.Domain.Model.Status;

namespace Produtos.Query.DTO
{
    public record ProdutoQueryDto(string Id, string Descricao, decimal Preco, EstoqueDto Estoque, ProdutoStatus EstaAtivo);
    public record EstoqueDto(int Quantidade, int EstoqueMinimo);
}
