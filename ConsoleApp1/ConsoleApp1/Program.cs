using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        const string Divider = "\n--------------------------------------------------------\n";


        //*******************************************************
        //*****ESERCIZIO 1 **************************************
        int[] iNumeri = { 3, 7, 1, 9, 5 };
        try
        {
            int massimo = TrovaMassimo(iNumeri);
            Console.WriteLine($"Il numero massimo nell'array è: {massimo}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Errore: {ex.Message}");
        }

        Console.WriteLine(Divider);

        //*******************************************************
        //*****ESERCIZIO 2 **************************************
        string[] testCases = { "()", "(())", "(()", ")(", "((())())", "))))())"};

        foreach (var test in testCases)
        {
            Console.WriteLine($"Input: \"{test}\" -> IsBalanced: {IsBalanced(test)}");
        }

        Console.WriteLine(Divider);

        //*******************************************************
        //*****ESERCIZIO 3 **************************************
        string[] firstInputs = { "listen", "triangle", "hello", "aabbcc" };
        string[] secondInputs = { "silent", "integral", "world", "abcabc" };

        for (int i = 0; i < firstInputs.Length; i++)
        {
            Console.WriteLine($"\"{firstInputs[i]}\" and \"{secondInputs[i]}\" are anagrams: {AreAnagrams(firstInputs[i], secondInputs[i])}");
        }

        Console.WriteLine(Divider);

        //*******************************************************
        //*****ESERCIZIO 4 **************************************
        var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        var result = SeparateNumber(numbers);

        Console.WriteLine("Numeri pari: " + string.Join(", ", result.EvenNumbers));
        Console.WriteLine("Numeri dispari: " + string.Join(", ", result.OddNumbers));

        Console.WriteLine(Divider);

        //*******************************************************
        //*****ESERCIZIO 5 **************************************

        int[] newnumbers = { 3, 5, 3, 3, 5, 8, 8, 8, 1 };

        var resultNum = GetNumberSums(newnumbers);

        Console.WriteLine($"Contenuto array da calcolare: " + string.Join('-', newnumbers));
        

        foreach (var item in resultNum)
        {
            Console.WriteLine($"Number: {item.Number}, Sum: {item.Sum}");
        }

        Console.WriteLine(Divider);

        //*******************************************************
        //*****ESERCIZIO 6 **************************************

        double latoQuadrato = 5;
        double areaQuadrato = CalcolaAreaQuadrato(latoQuadrato);
        Console.WriteLine($"L'area del quadrato con lato {latoQuadrato} è: {areaQuadrato}");

        double baseTriangolo = 4;
        double altezzaTriangolo = 6;
        double areaTriangolo = CalcolaAreaTriangolo(baseTriangolo, altezzaTriangolo);
        Console.WriteLine($"L'area del triangolo con base {baseTriangolo} e altezza {altezzaTriangolo} è: {areaTriangolo}");

    }

    public static IEnumerable<(int Number, int Sum)> GetNumberSums(int[] number)
    {
        if (number == null || number.Length == 0)
            throw new ArgumentException("sdffafdafasdf");

        return number.GroupBy(g => g).Select(g => new {num = g.Key, sum = g.Sum() }).OrderByDescending(x=> x.sum).Select(x => (x.num, x.sum));
        

    }

    public static int TrovaMassimo(int[] iNumeri)
    {
        if (iNumeri == null || iNumeri.Length == 0) throw new ArgumentException("sdfadfasdfasdfafas");

        int iNumMax = iNumeri[0];

        foreach (var item in iNumeri)
        {
            if(item > iNumMax)
            {
                iNumMax = item;
            }
        }

        return iNumMax;

    }
    public static bool IsBalanced(string input)
    {
       if(input == null || input.Length == 0)
            return false;

        int Balanced = 0;

       foreach (var item in input)
        {
            if(item == '(')
            {
                Balanced++;
            }
            else if (item == ')') 
            {
                Balanced--;
                if (Balanced == 0) return true;
            }
        }

       return Balanced == 0;
    }

    public static bool AreAnagrams(string first, string second)
    {
        if(string.IsNullOrEmpty(first) ||  string.IsNullOrEmpty(second)) return false;

        first = new string(first.ToLower().Where(char.IsLetterOrDigit).ToArray());
        second = new string(second.ToLower().Where(char.IsLetterOrDigit).ToArray());

        if(first.Length != second.Length) return false;

        var firstchar = first.GroupBy(g => g).ToDictionary(x => x.Key, x => x.Count());
        var secondchar = second.GroupBy(g => g).ToDictionary(x => x.Key, x => x.Count());

        return firstchar.OrderBy(kvp => kvp.Key).SequenceEqual(secondchar.OrderBy(kvp => kvp.Key));

    }

    public static (List<int> EvenNumbers, List<int> OddNumbers) SeparateNumber(List<int> numbers)
    {
       List<int> evenNumbers = new List<int>();
       List<int> oddNumbers = new List<int>();

       foreach (int number in numbers) 
       {
            if(number % 2 == 0)
                evenNumbers.Add(number);
            else
            {
                oddNumbers.Add(number);
            }
        }

       return (evenNumbers, oddNumbers);

    }

    public static double CalcolaAreaQuadrato(double LatoQuadrato)
    {
        return LatoQuadrato * LatoQuadrato;
    }

    public static double CalcolaAreaTriangolo(double Base, double LatoTriangolo) 
    {
        return Base * LatoTriangolo/ 2;
    }



    
}




