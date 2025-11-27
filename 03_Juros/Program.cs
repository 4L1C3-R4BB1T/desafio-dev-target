using System;
using System.Globalization;

namespace Juros;

/*
    3. Faça um programa que a partir de um valor e de uma data de vencimento, calcule o valor dos juros 
    na data de hoje considerando que a multa seja de 2,5% ao dia.
*/

class Program
{
    static void Main(string[] args)
    {
        const decimal taxa = 0.025m;

        decimal valor;
        while (true)
        {
            Console.Write("Valor: ");
            if (decimal.TryParse(Console.ReadLine(), NumberStyles.Number,
                CultureInfo.CurrentCulture, out valor) && valor > 0m) break;
            Console.WriteLine("Valor inválido. Digite novamente.");
        }

        DateTime dataVencimento;
        while (true)
        {
            Console.Write("Data de Vencimento [dd/MM/yyyy]: ");
            if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out dataVencimento)) break;
            Console.WriteLine("Data inválida.");
        }

        DateTime dataHoje = DateTime.Today;

        if (dataHoje <= dataVencimento)
        {
            Console.WriteLine($"\nSem juros. Vencimento: {dataVencimento:dd/MM/yyyy}. Valor: R$ {valor:F2}");
            return;
        }

        int diasAtraso = (dataHoje - dataVencimento).Days;

        decimal jurosSimples = decimal.Round(valor + valor * taxa * diasAtraso);
        decimal jurosComposto = decimal.Round(valor * (decimal)Math.Pow((double)(1 + taxa), diasAtraso));

        Console.WriteLine($"\nDias de atraso: {diasAtraso}");
        Console.WriteLine($"Valor original: R$ {valor:F2}");
        Console.WriteLine($"Valor com juros simples: R$ {jurosSimples:F2}");
        Console.WriteLine($"Valor com juros compostos: R$ {jurosComposto:F2}");
    }
}

