using Core.Commands;

namespace Clientes.Application.Commands
{
    public class InativarClienteCommand : ICommandRequest
    {
        public string Id { get; private set; }
        public InativarClienteCommand(string id)
        {
            Id = id;
        }
    }
}