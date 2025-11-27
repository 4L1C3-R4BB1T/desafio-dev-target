using System.Globalization;

namespace Juros;

class Program
{
    static void Main(string[] args)
    {

        /*
            Faça um programa que a partir de um valor e de uma data de vencimento, calcule o valor dos juros na data de hoje considerando que a multa seja de 2,5% ao dia.
        */

        double valor;
        while (true)
        {
            Console.Write("Valor: ");
            if (double.TryParse(Console.ReadLine(), out valor) && valor > 0) break;
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

        if (dataHoje > dataVencimento)
        {
            TimeSpan diff = dataHoje - dataVencimento;
            double dias = diff.TotalDays;

            for (int i = 1; i <= dias; i++)
            {
                valor += valor * 0.025;
            }
        }

        Console.WriteLine($"\nJuros hoje [{dataHoje.ToString("dd/MM/yyyy")}]: R$ {valor:F2}");
    }
}

