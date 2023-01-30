using Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands
{
    public class CriarVendaCommand : ICommandRequest
    {
        public string Id { get; internal set; } = string.Empty;
        public string ClienteId { get; set; } = null!;
    }
}
