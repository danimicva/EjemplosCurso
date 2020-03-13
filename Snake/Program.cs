using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Snake
{
    class Program
    {

        private const int TAMANO_MAPA = 10;
        static void Main(string[] args)
        {
            jugarPartida();
        }

        private enum ResultadoMovimiento
        {
            Muerte,
            Nada,
            Manzana
        }

        private static void jugarPartida()
        {
            List<Point> serpiente = new List<Point>();
            Random rand = new Random();
            Point manzana = new Point(rand.Next(0, TAMANO_MAPA - 1), rand.Next(0, TAMANO_MAPA - 1));

            serpiente.Add(new Point(5, 5));
            serpiente.Add(new Point(5, 6));
            serpiente.Add(new Point(5, 7));

            pintarPantalla(serpiente, manzana);
            while (true)
            {
                ConsoleKeyInfo entrada = Console.ReadKey();
                

                if (entrada.KeyChar == 'x')
                    break;

                ResultadoMovimiento nuevoPunto = moverSerpiente(serpiente, entrada.Key, manzana);
                
                if(nuevoPunto == ResultadoMovimiento.Manzana)
                    manzana = new Point(rand.Next(0, TAMANO_MAPA - 1), rand.Next(0, TAMANO_MAPA - 1));

                // Console.Clear();
                Console.WriteLine("Usa las flechas para moverte!")
                pintarPantalla(serpiente, manzana);
                Console.WriteLine("Tamaño: " + serpiente.Count);


                if (nuevoPunto == ResultadoMovimiento.Muerte)
                    break;
            }

            Console.WriteLine("Partida terminada.");
        }

        private static ResultadoMovimiento moverSerpiente(List<Point> serpiente, ConsoleKey pulsacion, Point manzana)
        {
            Point? nuevoPunto = null;
            ResultadoMovimiento ret = ResultadoMovimiento.Nada;

            switch (pulsacion)
            {
                case ConsoleKey.LeftArrow:

                    if (serpiente[0].X <= 0)
                        return ResultadoMovimiento.Muerte;

                    nuevoPunto = new Point(serpiente[0].X - 1, serpiente[0].Y);
                    break;
                case ConsoleKey.RightArrow:

                    if (serpiente[0].X >= TAMANO_MAPA - 1)
                        return ResultadoMovimiento.Muerte;

                    nuevoPunto = new Point(serpiente[0].X + 1, serpiente[0].Y);
                    break;
                case ConsoleKey.DownArrow:

                    if (serpiente[0].Y <= 0)
                        return ResultadoMovimiento.Muerte;

                    nuevoPunto = new Point(serpiente[0].X, serpiente[0].Y - 1);
                    break;
                case ConsoleKey.UpArrow:

                    if (serpiente[0].Y >= TAMANO_MAPA - 1)
                        return ResultadoMovimiento.Muerte;

                    nuevoPunto = new Point(serpiente[0].X, serpiente[0].Y + 1);
                    break;
            }

            if (nuevoPunto != null)
            {
                if (serpiente.Contains(nuevoPunto.Value))
                    return ResultadoMovimiento.Muerte;

                serpiente.Insert(0, nuevoPunto.Value);
                serpiente.RemoveAt(serpiente.Count - 1);
            }

            if (serpiente.Any(p => p.Equals(manzana)))
            {
                serpiente.Add(new Point(serpiente[serpiente.Count - 1].X, serpiente[serpiente.Count - 1].Y));
                ret = ResultadoMovimiento.Manzana;
            }

            return ret;
        }

        private static string imprimeCaracterRepetido(char caracter, int nVeces)
        {
            string ret = "";
            for(int i = 0; i < nVeces; i++)
                ret += caracter;
            return ret;
        }

        private static void pintarPantalla(List<Point> serpiente, Point manzana)
        {

            String buffer = "";

            buffer += "+";
            buffer += imprimeCaracterRepetido('-', TAMANO_MAPA * 2);
            buffer += "+" + Environment.NewLine;

            for(int i = TAMANO_MAPA - 1; i >= 0; i--)
            {
                buffer += "|";
                for (int j = 0; j < TAMANO_MAPA; j++)
                {
                    buffer += (serpiente.Any(p => p.X == j && p.Y == i)) ? "O " : manzana.X == j && manzana.Y == i ? "X " : "  ";
                }
                buffer += "|" + Environment.NewLine;
            }

            buffer += "+";
            buffer += imprimeCaracterRepetido('-', TAMANO_MAPA * 2);
            buffer += "+" + Environment.NewLine;

            Console.Write(buffer);
        }
    }
}
