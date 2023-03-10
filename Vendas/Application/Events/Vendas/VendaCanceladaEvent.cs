using Core.Commands;
using Core.EventMessages;

namespace Vendas.Application.Events.Vendas
{
    public class VendaCanceladaEvent : EventRequest
    {
        public VendaCanceladaEvent(string id, IList<ProdutoVendaEventItem> produtos)
        {
            Id = id;
            Produtos = produtos;
        }

        public string Id { get; private set; } = null!;
        public IList<ProdutoVendaEventItem> Produtos { get; private set; }
    }
}
