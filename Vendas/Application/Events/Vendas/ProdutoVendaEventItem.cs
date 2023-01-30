using Core.EventMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Events.Vendas
{
    public class ProdutoVendaEventItem
    {
        public ProdutoVendaEventItem(string produtoId, int quantidade)
        {
            ProdutoId = produtoId;
            Quantidade = quantidade;
        }

        public string ProdutoId { get; private set; } = null!;
        public int Quantidade { get; private set; }
    }
}
