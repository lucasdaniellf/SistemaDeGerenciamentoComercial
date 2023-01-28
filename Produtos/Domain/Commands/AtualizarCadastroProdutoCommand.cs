using Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Produtos.Domain.Commands
{
    public class AtualizarCadastroProdutoCommand : ICommandRequest
    {
        public string Id { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public decimal Preco { get; set; }
        public int EstoqueMinimo { get; set; }
    }
}
