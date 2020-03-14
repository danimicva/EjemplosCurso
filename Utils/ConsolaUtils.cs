using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class ConsolaUtils
    {

        public static void limpiarPantalla() {
            string s = "";
            for (int i = 0; i < 30; i++) {
                s += Environment.NewLine;
            }

            Console.Write(s);
        }
    }
}
