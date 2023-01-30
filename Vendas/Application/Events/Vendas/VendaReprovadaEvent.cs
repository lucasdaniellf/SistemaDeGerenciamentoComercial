using Core.EventMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Events.Vendas
{
    public class VendaReprovadaEvent : EventRequest
    {
        public string Id { get; set; } = null!;
    }
}
