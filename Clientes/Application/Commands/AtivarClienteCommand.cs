using Core.Commands;

namespace Clientes.Application.Commands
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
