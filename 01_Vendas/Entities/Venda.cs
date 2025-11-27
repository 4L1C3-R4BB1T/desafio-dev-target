namespace Vendas.Entities;

public class Venda
{
    public string Vendedor { get; set; } = string.Empty;
    public double Valor { get; set; }
    public double Comissao { get; set; }
}
