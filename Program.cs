using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demineure_schweizer2
{
    internal class Program
    {
        public static int lines;
        public static int collones;
        public static int difficulty;
        static void Main(string[] args)
        {
            Title();
            instructions();
            Lines();
            Title();
            instructions2();
            Grid();
            Console.ReadKey();
        }
        static void Title()
        {
            Console.WriteLine("\t****************************************************************************");
            Console.WriteLine("\t*                :) Démineur simplifié (Hugo Schweizer) :)                 *");
            Console.WriteLine("\t****************************************************************************");

        }
        static void instructions()
        {
            Console.SetCursorPosition(3, 4);
            Console.WriteLine("Merci d'entrer le nombre de ligne et de colonne de votre plateau de jeux");
            Console.SetCursorPosition(3, 5);
            Console.WriteLine("en sachant qu'au minimum on a un plateau de 6 lignes x 6 colonnes !");
            Console.SetCursorPosition(3, 6);
            Console.WriteLine("et au maximum un plateau de 30 lignes et 30 colonnes !");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.SetCursorPosition(0, 9);

        }
        static void Lines()
        {
            Console.ResetColor();
            Console.Write("Nombre de lignes : ");
            bool getOut = int.TryParse(Console.ReadLine(),out lines);
            if (getOut)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                if (lines < 6 || lines > 30)
                {
                    Console.WriteLine("Valeur hors limite ! Merci de réessayer !\n");
                }
                else
                {
                    Collones();
                }
            }
            else
            {
                Console.WriteLine("Valeur hors limite ! Merci de réessayer !\n");
            }
            Lines();

        }
        static void Collones()
        {
            Console.ResetColor();
            Console.Write("Nombre de colonnes: ");

            collones = int.Parse(Console.ReadLine());
            if (collones < 6 || collones > 30)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Valeur hors limite ! Merci de réessayer !\n");
                Console.ResetColor();
                Collones();
            }
            else
            {
                Difficulty();
            }
        }
        static void Difficulty()
        {
            Console.WriteLine("\r\n    Merci d'entrer la difficulté pour votre jeu ");
            Console.WriteLine("    en sachant que : ");
            Console.WriteLine("         1--> niveau facile");
            Console.WriteLine("         2--> niveau moyen");
            Console.WriteLine("         3--> niveau difficile");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.Write("\nEntrez le niveau de difficulté: ");
            if (!int.TryParse(Console.ReadLine(), out difficulty))
            {
                Console.WriteLine("Saisie invalide !\n");
                Difficulty();
                return;
            }
            if (difficulty == 1)
            {
                Console.WriteLine("→ Vous avez choisi le niveau FACILE !");
            }
            else if (difficulty == 2)
            {
                Console.WriteLine("→ Vous avez choisi le niveau MOYEN !");
            }
            else if (difficulty == 3)
            {
                Console.WriteLine("→ Vous avez choisi le niveau DIFFICILE !");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Valeur hors limite ! Merci de choisir 1, 2 ou 3.\n");
                Console.ResetColor();
                Difficulty();
            }
        }
        static void instructions2()
        {
            switch (difficulty)
            {
                case 1:
                    Console.Write("A vous de jouer !! Mode : ");
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Facile");
                    Console.ResetColor(); break;
                case 2:
                    Console.Write("A vous de jouer !! Mode : ");
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("Moyen");
                    Console.ResetColor(); break;
                case 3:
                    Console.Write("A vous de jouer !! Mode : ");
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Difficile");
                    Console.ResetColor(); break;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.ResetColor();



        }
        static void Grid()
        {
            int i = 0;
            Console.Clear();
            
            Console.CursorLeft = 5;
            Console.Write("╔═══╦");
            for (; i < collones - 2; i++)
            {
                Console.Write("═══╦");
            }
            Console.Write("═══╗");
            i = 0;

            Console.WriteLine("");
            Console.CursorLeft = 5;
            Console.Write("║  ");
            for (; i < collones; i++)
            {

                Console.Write(" ║  ");
            }
            for (int j = 0; j < lines - 1; j++)
            {
                Console.WriteLine("");
                Console.CursorLeft = 5;
                i = 0;
                Console.Write("╠═══╬");
                for (; i < collones - 2; i++)
                {
                    Console.Write("═══╬");
                }
                Console.Write("═══╣");

                i = 0;
                Console.WriteLine("");
                Console.CursorLeft = 5;
                Console.Write("║  ");
                for (; i < collones; i++)
                {

                    Console.Write(" ║  ");
                }
            }
            i = 0;
            Console.WriteLine("");
            Console.CursorLeft = 5;
            Console.Write("╚═══╩");
            for (; i < collones - 2; i++)
            {
                Console.Write("═══╩");
            }
            Console.Write("═══╝");
        }
    }
}
