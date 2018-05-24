using System;
using System.IO;
using System.Text;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Lab2 lab = new Lab2();
            string plainText = "Hello world";
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            //microsoft base64
            Console.WriteLine(Convert.ToBase64String(plainTextBytes));
            //my base64
            Console.WriteLine(lab.GetEncoded(plainTextBytes));

            Console.WriteLine(lab.GetReport(@"C:\Users\Administrator\Desktop\virsh.txt"));
            Console.WriteLine(lab.GetReport(@"C:\Users\Administrator\Desktop\1.txt"));
            Console.WriteLine(lab.GetReport(@"C:\Users\Administrator\Desktop\2.txt"));

            Console.ReadKey();
        }
    }

    class Lab2
    {
        private char[] charTable = new char[64]
          {  'A','B','C','D','E','F','G','H','I','J','K','L','M',
            'N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
            'a','b','c','d','e','f','g','h','i','j','k','l','m',
            'n','o','p','q','r','s','t','u','v','w','x','y','z',
            '0','1','2','3','4','5','6','7','8','9','+','/'};

        public string GetReport(string path)
        {
            string text = this.ReadFile(path);

            long textLenght = this.GetFileLenght(path);
            long encodeLenght = this.GetEncoded(Encoding.UTF8.GetBytes(text)).Length;

            return string.Format("textLenght = {0};\tencodeLenght = {1}", textLenght.ToString(), encodeLenght.ToString());
        }

        public char[] GetEncoded(byte[] input)
        {
            int paddingCount = GetPaddingCount(input.Length);
            int blockCount = GetBlockCount(input.Length, paddingCount);
            byte[] source = new byte[input.Length + paddingCount];
            byte[] buffer = new byte[blockCount * 4];
            char[] result = new char[blockCount * 4];

            for (int i = 0; i < source.Length; i++)
            {
                if (i < input.Length)
                    source[i] = input[i];

                else
                    source[i] = 0;
            }

            for (int i = 0; i < blockCount; i++)
            {
                buffer[i * 4] = (byte)((source[i * 3] & 252) >> 2);
                buffer[i * 4 + 1] = (byte)(((source[i * 3 + 1] & 240) >> 4) + (byte)((source[i * 3] & 3) << 4));
                buffer[i * 4 + 2] = (byte)(((source[i * 3 + 2] & 192) >> 6) + ((source[i * 3 + 1] & 15) << 2));
                buffer[i * 4 + 3] = (byte)(source[i * 3 + 2] & 63);
            }

            for (int x = 0; x < blockCount * 4; x++)
                result[x] = this.ConvertToChar(buffer[x]);

            if (paddingCount == 1)
                result[blockCount * 4 - 1] = '=';

            else if (paddingCount == 2)
            {
                result[blockCount * 4 - 1] = '=';
                result[blockCount * 4 - 2] = '=';
            }

            return result;
        }

        public string ReadFile(string path)
        {
            using (StreamReader stream = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                return stream.ReadToEnd();
            }
        }

        public long GetFileLenght(string path)
        {
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                return fs.Length;
            }
        }

        private int GetPaddingCount(int length)
        {
            return length % 3 == 0 ? 0 : 3 - (length % 3);
        }

        private int GetBlockCount(int length, int paddingCount)
        {
            return length % 3 == 0 ? length / 3 : (length + paddingCount) / 3;
        }

        private char ConvertToChar(byte b)
        {
            return charTable[(int)b];
        }
    }
}
