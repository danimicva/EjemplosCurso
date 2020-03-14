using System;

namespace Utils
{
    public class StringUtils
    {
        public static string RepetirCaracterNVeces(char caracter, int nVeces) {
            string ret = "";
            for (int i = 0; i < nVeces; i++)
                ret += caracter;
            return ret;
        }
    }
}
