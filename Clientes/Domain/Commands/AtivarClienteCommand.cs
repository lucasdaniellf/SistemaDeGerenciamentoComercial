using Core.Commands;

namespace Clientes.Domain.Commands
{
    public class AtivarClienteCommand : ICommandRequest
    {
        public string Id { get; private set; }
        public AtivarClienteCommand(string id)
        {
            Id = id;
        }
    }
}
