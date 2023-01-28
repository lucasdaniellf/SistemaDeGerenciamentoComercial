using Clientes.Domain.Model;
using Core.Commands;
using System.ComponentModel.DataAnnotations;

namespace Clientes.Domain.Commands
{
    public class CadastrarClienteCommand : ICommandRequest
    {
        public string Nome { get; set; } = null!;

        [RegularExpression(@"^\d{11}$")]
        public string Cpf { get; set; } = null!;
        public string Id { get; internal set; } = string.Empty; 
    }
}
