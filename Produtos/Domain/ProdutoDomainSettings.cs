using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Produtos.Domain
{
    public class ProdutoDomainSettings
    {
        public string FilaProdutoCadastrado { get; set; } = null!;
        public string FilaProdutoAtualizado { get; set; } = null!;
        public string FilaProdutoStatusAlterado { get; set; } = null!;
        public string FilaProdutoEstoqueAlterado { get; set; } = null!;
    }
}
