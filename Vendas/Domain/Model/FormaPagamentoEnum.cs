using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Domain.Model
{
    public class FormaPagamentoEnum
    {
        public enum FormaPagamento
        {
            A_VISTA = 0,
            CARTAO_CREDITO = 1
        }

        public static FormaPagamento SelecionarFormaDePagamento(int value)
        {
            switch(value)
            {
                case 0:
                    return FormaPagamento.A_VISTA;
                default:
                    return FormaPagamento.CARTAO_CREDITO;
            }
        }
    }
}
