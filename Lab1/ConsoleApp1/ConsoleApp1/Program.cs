using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Lab1 lab = new Lab1();

            string path1 = @"C:\Users\Administrator\Desktop\virsh.txt";
            string path2 = @"C:\Users\Administrator\Desktop\1.txt";
            string path3 = @"C:\Users\Administrator\Desktop\2.txt";

            foreach (string item in lab.GetReport(path1))
                Console.WriteLine(item);

            foreach (string item in lab.GetReport(path2))
                Console.WriteLine(item);

            foreach (string item in lab.GetReport(path3))
                Console.WriteLine(item);

            Console.ReadKey();
        }
    }

    class Lab1
    {
        public IList<string> GetReport(string path)
        {
            int count = 0;
            string text = this.ReadFromFile(path);
            IDictionary<string, int> symbols = this.GetSymbols(text, out count);
            IList<string> result = new List<string>();
            FileStream file = File.Open(path, FileMode.Open);

            result.Add(string.Empty);
            result.Add(file.Name);

            foreach (KeyValuePair<string, int> pair in symbols.OrderByDescending(s => s.Value))
                result.Add(string.Format("{0}\t{1}\t{2}",
                              pair.Key,
                              pair.Value.ToString(),
                              (pair.Value * 1.0 / count).ToString("0.#########")));

            double h = 0;

            for (int i = 0; i < symbols.Count; i++)
            {
                double p = symbols.ToArray()[i].Value * 1.0 / count;

                h = p + Math.Log(1 / p, 2);
            }

            long fileLen = file.Length;

            result.Add(string.Format("H = {0}", h));
            result.Add(string.Format("File = {0}", fileLen));

            return result;
        }

        private string ReadFromFile(string path)
        {
            using (StreamReader stream = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                return stream.ReadToEnd();
            }
        }

        private IDictionary<string, int> GetSymbols(string text, out int symbolCount)
        {
            IDictionary<string, int> symbols = new Dictionary<string, int>();
            text = text.Replace("\r\n", string.Empty);
            symbolCount = text.Length;

            for (int i = 0; i < text.Length; i++)
            {
                string symbol = text[i].ToString();

                if (symbols.ContainsKey(symbol))
                    symbols[symbol]++;

                else
                    symbols.Add(symbol, 1);
            }

            return symbols;
        }
    }
}
