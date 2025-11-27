using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Estoque.Entities;
using Estoque.Entities.Enums;

namespace Estoque;

class Program
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    static void Main(string[] args)
    {
        const string PATH = "Resources/estoque.json";

        string json = File.ReadAllText(PATH);
        var estoque = JsonNode.Parse(json)!["estoque"]!.Deserialize<Produto[]>(_options)!;

        var movimentos = new List<Movimento>();

        while (true)
        {
            Menu(estoque);

            var opc = Console.ReadLine()?.Trim();

            if (opc == "0") break;

            if (opc != "1")
            {
                Console.WriteLine("Opção inválida.");
                continue;
            }

            Console.Write("Código do produto: ");
            if (!int.TryParse(Console.ReadLine(), out int codigo))
            {
                Console.WriteLine("Código inválido.");
                continue;
            }

            var produto = estoque.FirstOrDefault(p => p.CodigoProduto == codigo);
            if (produto == null)
            {
                Console.WriteLine("Produto não encontrado.");
                continue;
            }

            Console.Write("Tipo [E = Entrada / S = Saída]: ");
            var tipoStr = Console.ReadLine()?.Trim().ToUpper();
            TipoMovimento tipo;

            if (tipoStr == "E") tipo = TipoMovimento.Entrada;
            else if (tipoStr == "S") tipo = TipoMovimento.Saida;
            else
            {
                Console.WriteLine("Tipo inválido.");
                continue;
            }

            Console.Write("Quantidade: ");
            if (!int.TryParse(Console.ReadLine(), out int quantidade) || quantidade <= 0)
            {
                Console.WriteLine("Quantidade inválida [deve ser inteiro > 0].");
                continue;
            }

            if (tipo == TipoMovimento.Saida && quantidade > produto.Estoque)
            {
                Console.WriteLine($"Estoque insuficiente. Estoque atual: {produto.Estoque}");
                continue;
            }

            var movimento = new Movimento
            {
                Codigo = DateTime.UtcNow.Ticks,
                CodigoProduto = produto.CodigoProduto,
                Tipo = tipo,
                Quantidade = quantidade
            };

            if (tipo == TipoMovimento.Entrada) produto.Estoque += quantidade;
            else produto.Estoque -= quantidade;

            movimentos.Add(movimento);

            Console.WriteLine($"\nMovimento Registrado [Código: {movimento.Codigo}].");
            Console.WriteLine($"Produto: {produto.DescricaoProduto} [Cod. {produto.CodigoProduto}]");
            Console.WriteLine($"Quantidade Movimentada: {movimento.Quantidade} [{movimento.Tipo}]");
            Console.WriteLine($"Estoque Produto: {produto.Estoque}");
        }

        File.WriteAllText(PATH, JsonSerializer.Serialize(new { estoque }, _options));
    }

    static void Menu(Produto[] estoque)
    {
        Console.WriteLine("\n==================== ESTOQUE ====================");
        Console.WriteLine("Código |          Descrição             | Estoque");
        foreach (var p in estoque)
        {
            Console.WriteLine($"{p.CodigoProduto,6} | {p.DescricaoProduto,-30} | {p.Estoque,6}");
        }
        Console.WriteLine("=================================================");
        Console.WriteLine("\nEscolha uma opção:\n1 - Lançar movimentação\n0 - Sair\nOpção: ");
    }
}
