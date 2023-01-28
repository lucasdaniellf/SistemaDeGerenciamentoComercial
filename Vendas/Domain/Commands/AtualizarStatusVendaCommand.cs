using Core.Commands;
using static Vendas.Domain.Model.StatusVenda;

namespace Vendas.Domain.Commands
{
    public class AtualizarStatusVendaCommand : ICommandRequest
    {
        public string Id { get; set; } = null!;
        public Status Status { get; set; }
    }
}
