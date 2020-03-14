using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Utils;

namespace Snake
{
    class Program
    {
        
        

        private const int TamañoMapa = 6;
        private static Random Aleatorio = new Random();

        static void Main(string[] args)
        {

            mostrarMenu();
        }

        private static void mostrarMenu() {

            string entradaUsuario;
            int opcionElegida = -1;

            imprimirMenu();

            while (opcionElegida < 1 || opcionElegida > 3) {
                
                Console.Write(@"Elije una opción: ");

                entradaUsuario = Console.ReadLine();

                if (!int.TryParse(entradaUsuario, out opcionElegida) || opcionElegida < 1 || opcionElegida > 3) {
                    Console.WriteLine("Entrada incorrecta.");
                    continue;
                }
            }

            switch (opcionElegida) {
                case 1: // Jugar
                    jugarPartida();
                    break;
                case 2: // Instrucciones
                    mostrarInstrucciones();
                    break;
                case 3: // Salir
                    return;
            }

            mostrarMenu();
        }

        private static void mostrarInstrucciones() {

        }

        #region Lógica de juego

        private enum ResultadoMovimiento
        {
            Muerte,
            Nada,
            Manzana
        }

        private static void jugarPartida()
        {
            List<Point> serpiente;
            Point manzana;
            bool? victoria = null;

            inicializarPartida(out serpiente, out manzana);

            pintarPartida(serpiente, manzana);
            while (victoria == null)
            {
                ConsoleKeyInfo entrada = Console.ReadKey();
                
                // Je je je
                if (entrada.KeyChar == 'x')
                    break;

                ResultadoMovimiento nuevoPunto = moverSerpiente(serpiente, entrada.Key, manzana);
                
                if(nuevoPunto == ResultadoMovimiento.Manzana)
                    manzana = new Point(Aleatorio.Next(0, TamañoMapa - 1), Aleatorio.Next(0, TamañoMapa - 1));

                // Console.Clear();
                Console.WriteLine("Usa las flechas para moverte!");
                pintarPartida(serpiente, manzana);


                if (nuevoPunto == ResultadoMovimiento.Muerte)
                    victoria = false;

                if(serpiente.Count > TamañoMapa * TamañoMapa) {
                    victoria = true;
                }
            }

            if (victoria.Value)
                Console.Write("¡Enhorabuena, has ganado!");
            else
                Console.Write("¡Has perdido!");

            Console.WriteLine(" - Pulsa enter para volver al menú principal.");
            Console.ReadLine();
        }

        private static void inicializarPartida(out List<Point> serpiente, out Point manzana) {

            manzana = new Point(Aleatorio.Next(0, TamañoMapa - 1), Aleatorio.Next(0, TamañoMapa - 1));

            serpiente = new List<Point>();

            int centro = TamañoMapa / 2;

            serpiente.Add(new Point(centro, centro));
            serpiente.Add(new Point(centro, centro + 1));
            serpiente.Add(new Point(centro, centro + 2));
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

                    if (serpiente[0].X >= TamañoMapa - 1)
                        return ResultadoMovimiento.Muerte;

                    nuevoPunto = new Point(serpiente[0].X + 1, serpiente[0].Y);
                    break;
                case ConsoleKey.DownArrow:

                    if (serpiente[0].Y <= 0)
                        return ResultadoMovimiento.Muerte;

                    nuevoPunto = new Point(serpiente[0].X, serpiente[0].Y - 1);
                    break;
                case ConsoleKey.UpArrow:

                    if (serpiente[0].Y >= TamañoMapa - 1)
                        return ResultadoMovimiento.Muerte;

                    nuevoPunto = new Point(serpiente[0].X, serpiente[0].Y + 1);
                    break;
            }

            if (nuevoPunto != null)
            {
                if (serpiente.GetRange(0, serpiente.Count - 1).Contains(nuevoPunto.Value))
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

        #endregion

        #region Impresión por pantalla


        private static void pintarPartida(List<Point> serpiente, Point manzana)
        {
            String buffer = "";

            ConsolaUtils.limpiarPantalla();

            buffer += "+";
            buffer += StringUtils.RepetirCaracterNVeces('-', TamañoMapa * 2);
            buffer += "+" + Environment.NewLine;

            for(int i = TamañoMapa - 1; i >= 0; i--)
            {
                buffer += "|";
                for (int j = 0; j < TamañoMapa; j++)
                {
                    buffer += (serpiente.Any(p => p.X == j && p.Y == i)) ? "O " : manzana.X == j && manzana.Y == i ? "X " : "  ";
                }
                buffer += "|" + Environment.NewLine;
            }

            buffer += "+";
            buffer += StringUtils.RepetirCaracterNVeces('-', TamañoMapa * 2);
            buffer += "+" + Environment.NewLine;

            buffer += "Tamaño: " + serpiente.Distinct().Count() + Environment.NewLine;

            Console.Write(buffer);
        }

        private static void imprimirMenu() {

            ConsolaUtils.limpiarPantalla();
            Console.WriteLine();

            Console.WriteLine(@"+------------------------------------------------+");
            Console.WriteLine(@"|                                                |");
            Console.WriteLine(@"|                                                |");
            Console.WriteLine(@"|       _________              __                |");
            Console.WriteLine(@"|      / _____ /  ____ _____  |  | __ ____       |");
            Console.WriteLine(@"|      \_____  \ /    \\__  \ |  |/ // __ \      |");
            Console.WriteLine(@"|      /        \   |  \/ __ \|    <\  ___ /     |");
            Console.WriteLine(@"|     / _______ /___|  (____ / __|_ \\___ >      |");
            Console.WriteLine(@"|             \/     \/     \/     \/    \/      |");
            Console.WriteLine(@"|                                                |");
            Console.WriteLine(@"|                                                |");
            Console.WriteLine(@"+------------------------------------------------+");
            Console.WriteLine(@"|                 1.Jugar                        |");
            Console.WriteLine(@"|             2.Instrucciones                    |");
            Console.WriteLine(@"|                 3.Salir                        |");
            Console.WriteLine(@"+------------------------------------------------+");
        }


        #endregion
    }
}
