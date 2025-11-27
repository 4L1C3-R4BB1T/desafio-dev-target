using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Estoque.Entities;
using Estoque.Entities.Enums;

/*
    2. Faça um programa onde eu possa lançar movimentações de estoque dos produtos que estão no json 
    abaixo, dando entrada ou saída da mercadoria no meu depósito, onde cada movimentação deve ter:
        • Um número identificador único.
        • Uma descrição para identificar o tipo da movimentação realizada
    E que ao final da movimentação me retorne a qtde final do estoque do produto movimentado.
*/

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

        if (!File.Exists(PATH))
        {
            Console.WriteLine($"Arquivo não encontrado: {PATH}");
            return;
        }

        string json = File.ReadAllText(PATH);
        var dados = JsonNode.Parse(json)!["estoque"]!.Deserialize<Produto[]>(_options)!;

        var movimentos = new List<Movimento>();

        while (true)
        {
            Menu(dados);

            var opc = Console.ReadLine()?.Trim();

            if (opc == "0") break;

            if (opc != "1")
            {
                Console.WriteLine("Opção inválida.");
                continue;
            }

            int codigo;
            Console.Write("Código do produto: ");
            while (!int.TryParse(Console.ReadLine(), out codigo))
            {
                Console.WriteLine("Código inválido. Digite novamente:");
            }

            var produto = dados.FirstOrDefault(p => p.CodigoProduto == codigo);
            if (produto == null)
            {
                Console.WriteLine("Produto não encontrado.");
                continue;
            }

            string tipoStr;
            do
            {
                Console.Write("Tipo [E = Entrada / S = Saída]: ");
                tipoStr = (Console.ReadLine() ?? "").Trim().ToUpper();
            } while (tipoStr != "E" && tipoStr != "S");

            TipoMovimento tipo = tipoStr == "E" ? TipoMovimento.Entrada : TipoMovimento.Saida;

            int quantidade;
            Console.Write("Quantidade: ");
            while (!int.TryParse(Console.ReadLine(), out quantidade) || quantidade <= 0)
            {
                Console.WriteLine("Quantidade inválida. Digite um inteiro maior que zero:");
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

            if (tipo == TipoMovimento.Entrada)
            {
                produto.Estoque += quantidade;
            }
            else
            {
                produto.Estoque -= quantidade;
            }

            movimentos.Add(movimento);

            Console.WriteLine($"\nMovimento Registrado [Cod.: {movimento.Codigo}].");
            Console.WriteLine($"Produto: {produto.DescricaoProduto} [Cod. {produto.CodigoProduto}]");
            Console.WriteLine($"Quantidade Movimentada: {movimento.Quantidade} [{movimento.Tipo}]");
            Console.WriteLine($"Estoque Produto: {produto.Estoque}");
        }

        File.WriteAllText(PATH, JsonSerializer.Serialize(new { estoque = dados }, _options));
        File.WriteAllText("Resources/movimentos.json", JsonSerializer.Serialize(movimentos, _options));
    }

    static void Menu(Produto[] produtos)
    {
        Console.WriteLine("\n==================== ESTOQUE ====================");
        Console.WriteLine("Código |          Descrição             | Estoque");
        foreach (var p in produtos)
        {
            Console.WriteLine($"{p.CodigoProduto,6} | {p.DescricaoProduto,-30} | {p.Estoque,6}");
        }
        Console.WriteLine("=================================================");
        Console.WriteLine("\nEscolha uma opção:\n1 - Lançar movimentação\n0 - Sair\nOpção: ");
    }
}
