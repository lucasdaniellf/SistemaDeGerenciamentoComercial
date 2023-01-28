using Core.Commands;
namespace Vendas.Domain.Commands
{
    public class ProcessarVendaCommand : ICommandRequest
    {
        public string Id { get; set; } = null!;
    }
}
