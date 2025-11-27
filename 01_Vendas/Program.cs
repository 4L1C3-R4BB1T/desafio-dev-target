using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Vendas.Entities;

/*
    1. Considerando que o json abaixo tem registros de vendas de um time comercial, faça um programa 
    que leia os dados e calcule a comissão de cada vendedor, seguindo a seguinte regra para cada venda:
        • Vendas abaixo de R$100,00 não gera comissão
        • Vendas abaixo de R$500,00 gera 1% de comissão
        • A partir de R$500,00 gera 5% de comissão
*/

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

        if (!File.Exists(PATH))
        {
            Console.WriteLine($"Arquivo não encontrado: {PATH}");
            return;
        }

        string json = File.ReadAllText(PATH);
        var dados = JsonNode.Parse(json)!["vendas"]!.Deserialize<Venda[]>(_options)!;

        foreach (var venda in dados)
        {
            venda.Comissao = CalcularComissao(venda.Valor);
        }

        var comissoes = dados
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
