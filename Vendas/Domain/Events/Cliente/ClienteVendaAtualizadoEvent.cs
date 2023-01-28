using Core.EventMessages;

namespace Vendas.Domain.Events.Cliente
{
    public class ClienteVendaAtualizadoEvent : EventRequest
    {
        public string Id { get; set; } = null!;
        public int EstaAtivo { get; set; }
    }
}
