using System.Text.Json.Serialization;
using Estoque.Entities.Enums;

namespace Estoque.Entities;

public class Movimento
{
    [JsonPropertyName("codigo")]
    public long Codigo { get; set; }

    [JsonPropertyName("codigoProduto")]
    public int CodigoProduto { get; set; }

    [JsonPropertyName("tipoMovimento")]
    public TipoMovimento Tipo { get; set; }

    [JsonPropertyName("quantidade")]
    public int Quantidade { get; set; }
}