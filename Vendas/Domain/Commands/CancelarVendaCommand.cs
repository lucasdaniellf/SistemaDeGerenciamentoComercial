using Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Domain.Commands
{
    public class CancelarVendaCommand : ICommandRequest
    {
        public string Id { get; set; } = null!;
    }
}
