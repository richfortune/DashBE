using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class BackUp
    {
        public static IEnumerable<(int Number, int Sum)> GetNumberSums(int[] number)
        {
            if (number == null || number.Length == 0)
                throw new ArgumentException("sdffafdafasdf");

            return number.GroupBy(g => g).Select(g => new { num = g.Key, Sum = g.Sum() }).OrderByDescending(x => x.Sum).Select(x => (x.num, x.Sum));

        }

        public static int TrovaMassimo(int[] iNumeri)
        {
            if (iNumeri == null || iNumeri.Length == 0)
            {
                throw new ArgumentException("kljlasjfjklaf");
            }

            int iNumMax = iNumeri[0];

            foreach (var item in iNumeri)
            {
                if (item > iNumMax)
                {
                    iNumMax = item;
                }
            }

            return iNumMax;

        }
        public static bool IsBalanced(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            int Balanced = 0;

            foreach (var item in input)
            {
                if (item == '(')
                {
                    Balanced++;
                }
                else if (item == ')')
                {
                    Balanced--;
                    if (Balanced == 0) { return true; }
                }
            }

            return Balanced == 0;
        }

        public static bool AreAnagrams(string first, string second)
        {
            if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(second))
                return false;

            first = new string(first.ToLower().Where(char.IsLetterOrDigit).ToArray());
            second = new string(second.ToLower().Where(char.IsLetterOrDigit).ToArray());

            if (first.Length != second.Length)
                return false;

            var firstdict = first.GroupBy(g => g).ToDictionary(dc => dc.Key, dc => dc.Count());
            var seconddict = second.GroupBy(g => g).ToDictionary(dc => dc.Key, dc => dc.Count());

            return firstdict.OrderBy(kvp => kvp.Key).SequenceEqual(seconddict.OrderBy(kvp => kvp.Key));

        }

        public static (List<int> EvenNumbers, List<int> OddNumbers) SeparateNumber(List<int> numbers)
        {
            List<int> evenNumbers = new List<int>();
            List<int> oddNumbers = new List<int>();

            foreach (var item in numbers)
            {
                if (item % 2 == 0)
                    evenNumbers.Add(item);
                else
                    oddNumbers.Add(item);
            }

            return (evenNumbers, oddNumbers);


        }
    }

}
