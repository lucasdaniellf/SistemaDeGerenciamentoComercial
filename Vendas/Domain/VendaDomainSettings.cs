namespace Vendas.Domain
{
    public class VendaDomainSettings
    {
        public string FilaClienteCadastrado { get; set; } = null!;
        public string FilaVendaConfirmada { get; set; } = null!;
        public string FilaVendaCancelada { get; set; } = null!;
        public string FilaVendaAprovada { get; set; } = null!;
        public string FilaVendaReprovada { get; set; } = null!;
    }
}
