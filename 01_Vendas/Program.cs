using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Vendas.Entities;

namespace Vendas;

class Program
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    static void Main(string[] args)
    {
        const string PATH = "Resources/vendas.json";

        string json = File.ReadAllText(PATH);
        var vendas = JsonNode.Parse(json)!["vendas"]!.Deserialize<Venda[]>(_options)!;

        foreach (var v in vendas)
        {
            v.Comissao = CalcularComissao(v.Valor);
        }

        var comissoes = vendas
           .GroupBy(v => v.Vendedor)
           .Select(g => new
           {
               Vendedor = g.Key,
               TotalComissao = g.Sum(v => v.Comissao),
               TotalVendas = g.Sum(v => v.Valor),
               Quantidade = g.Count()
           })
           .ToList();

        foreach (var c in comissoes)
        {
            Console.WriteLine($"\nVendedor: {c.Vendedor}");
            Console.WriteLine($"Quantidade Vendas: {c.Quantidade}");
            Console.WriteLine($"Total Vendas: R$ {c.TotalVendas:F2}");
            Console.WriteLine($"Total Comissões: R$ {c.TotalComissao:F2}");
        }
    }

    static double CalcularComissao(double valor)
    {
        if (valor >= 500.00) return valor * 0.05;
        else if (valor < 100) return 0;
        return valor * 0.01;
    }
}
