using Estoque.Entities.Enums;

namespace Estoque.Entities;

public class Movimento
{
    public long Codigo { get; set; }
    public int CodigoProduto { get; set; }
    public TipoMovimento Tipo { get; set; }
    public int Quantidade { get; set; }
}