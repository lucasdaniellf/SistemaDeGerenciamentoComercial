using Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Produtos.Domain.Commands.ProdutoEstoque
{
    public class ReporEstoqueProdutoCommand : ICommandRequest
    {
        public IEnumerable<EstoqueProduto> Produtos { get; set; } = Enumerable.Empty<EstoqueProduto>();
    }
}
