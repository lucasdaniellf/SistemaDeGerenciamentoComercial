using Core.Commands;
using System.ComponentModel.DataAnnotations;

namespace Produtos.Application.Commands
{
    public class AtualizarEstoqueProdutoCommand : ICommandRequest
    {
        public string ProdutoId { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int EstoqueMinimo { get; set; }

        [Range(0, int.MaxValue)]
        public int QuantidadeAtual { get; set; }
    }
}
