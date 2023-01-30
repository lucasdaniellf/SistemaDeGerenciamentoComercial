using Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Produtos.Application.Commands
{
    public class AdicionarProdutoEmCatalogoCommand : ICommandRequest
    {
        public string Id { get; set; } = null!;
    }
}
