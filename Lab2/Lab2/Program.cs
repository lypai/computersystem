using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class Program
    {
        public static void Main(string[] args)
        {
            DivisionResult result = RemShiftDivider.Devide(13, 2);

            Console.WriteLine(result.Quotient);
            Console.WriteLine(result.Remainder);
            Console.WriteLine(result.Def);
            Console.Read();
        }
    }
    public static class RemShiftDivider
    {
        public static DivisionResult Devide(int dividend, int divisor)
        {
            int lenght;
            long r = (long)Math.Abs(dividend);
            string def = string.Format("divident: {0} \n divisor: {1} ", Convert.ToString(dividend, 2), Convert.ToString(divisor, 2));
            long b = ((long)Math.Abs(divisor)) << 32;
            int q = 0;
            def += string.Format("remainder = {0}\n", Convert.ToString(r, 2));
            for (lenght = 0; dividend != 0; dividend >>= 1, lenght++) ;
            def += string.Format("Shift the remainder register to {0} bits left to optimize algorithm\n\n", 32 - lenght - 1);
            r <<= (32 - lenght - 1);
            for (int i = 0; i < lenght + 1; i++)
            {

                r <<= 1;
                def += string.Format("shift left remainder to 1 bit {0}\n", Convert.ToString(r, 2));
                def += "subtract  divisor from remainder:\n ";
                def += string.Format("remainder: {0} divisor: {1}\n", Convert.ToString(r, 2), Convert.ToString(divisor, 2));
                r += -b;
                def += string.Format("operation result: (remainder) {0}\n", Convert.ToString(r, 2));
                q <<= 1;
                if (r < 0)
                {
                    r += b;
                    def += string.Format("Restoring the remainder original value:\n{0}\nShift quotient register left to 1 bit: {1}\n\n  ", Convert.ToString(r, 2), Convert.ToString(q, 2));

                }
                else
                {
                    q++;
                    def += string.Format("Shift quotient register left to 1 bit and plus 1:  {0}\n\n", Convert.ToString(q, 2));
                }

            }
            r >>= 32;
            if (dividend < 0)
            {
                r = -r;
                q = -q;
            }
            return new DivisionResult { Def = def, Remainder = r, Quotient = q };
        }
        //static (string def, long remainder, int quotient) Devide (int dividend, int divisor, int lenght)
        //{
        //    string def = $"{dividend} :  {divisor}\n";
        //    long rem = dividend;
        //    int quotient = 0;

        //    for (int i = 0; i < lenght; i++)
        //    {

        //        rem <<= 1;
        //        def += $"shift left remainder to 1 bit {Convert.ToString(rem,2)}\n";
        //        long temp = rem & Convert.ToInt32(new string('1',lenght),2);//error
        //        def += "subtract  divisor from remainder:\n ";
        //        def += $"before: rem: {Convert.ToString(rem, 2)} divisor: {Convert.ToString(divisor, 2)}\n";
        //        def += $"{Convert.ToString(rem >> lenght - divisor,2)}";
        //        rem = (((rem >> lenght) - divisor) << lenght) + temp;
        //        def += $"after: rem: {Convert.ToString(rem,2)} divisor: {Convert.ToString(divisor,2)}\n";
        //        if (rem >> lenght < 0)
        //        {
        //            quotient <<= 1;
        //            rem = (((rem >> lenght) + divisor) << lenght) + temp;
        //            def += $"restore the value rem: {Convert.ToString(rem,2)}\nshift left quotient by 1 {Convert.ToString(quotient,2)}\n";
        //        }
        //        else
        //        {
        //            quotient <<=  1 + 1;
        //            def += $"shift left quatient and plus 1 {Convert.ToString(quotient,2)}\n\n";
        //        }
        //    }

        //    return (def, rem, quotient);

        //}
        // static long Logic1(int length)
        //{
        //    long val = 0;
        //    for (int i = 0; i < length; i++)
        //    {
        //        val *= 2 + 1;
        //    }
        //    return val;
        //}

    }

    public class DivisionResult
    {
        public string Def { get; set; }
        public long Remainder { get; set; }
        public int Quotient { get; set; }
    }
}
