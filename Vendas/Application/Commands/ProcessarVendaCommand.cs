using Core.Commands;

namespace Vendas.Application.Commands
{
    public class ProcessarVendaCommand : ICommandRequest
    {
        public string Id { get; set; } = null!;
    }
}
