namespace Vendas.Domain.Model
{
    public class StatusVenda
    {
        public enum Status
        {
            PENDENTE = 0,
            PROCESSANDO = 1,
            APROVADO = 2,
            REPROVADO = 3,
            CANCELADO = 99
        }

        public static Status AplicarStatus(int value)
        {
            switch (value)
            {
                case 0:
                    return Status.PENDENTE;
                case 1:
                    return Status.PROCESSANDO;
                case 2:
                    return Status.APROVADO;
                case 3:
                    return Status.REPROVADO;
                default:
                    return Status.CANCELADO;
            }
        }
    }
}