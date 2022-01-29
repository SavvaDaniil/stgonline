using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Component
{
    public class RandomComponent
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            //return new string(Enumerable.Repeat(chars, length)
            //.Select(s => s[random.Next(s.Length)]).ToArray());

            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[random.Next(0, allowedChars.Length)];
            }
            return new string(chars).ToLower();
        }

        public static string RandomIntAsString(int length)
        {
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            //return new string(Enumerable.Repeat(chars, length)
            //.Select(s => s[random.Next(s.Length)]).ToArray());

            const string allowedChars = "0123456789";
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[random.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }
    }
}
