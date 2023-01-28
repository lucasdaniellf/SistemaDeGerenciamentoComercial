using Core.EventMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Domain.Events.Vendas
{
    public class VendaAprovadaEvent : EventRequest
    {
        public string Id { get; set; } = null!;
    }
}
