using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
        public static int landMines;
        public static bool gagner = true;
        public static int j;
        public static bool test = true;
        public static string direction = "";
        static void Main(string[] args)
        {
            Title();
            instructions();
            Lines();
            Console.ReadLine();
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
            Console.ForegroundColor = ConsoleColor.Red;
            if (getOut)
            {
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

            bool getOut = int.TryParse(Console.ReadLine(), out collones);
            Console.ForegroundColor = ConsoleColor.Red;
            if (getOut)
            {
                if (collones < 6 || collones > 30)
                {
                    Console.WriteLine("Valeur hors limite ! Merci de réessayer !\n");
                }
                else
                {
                    Difficulty();
                }
            }
            else {
                Console.WriteLine("Valeur hors limite ! Merci de réessayer !\n");
                
            }
            Collones();
        }
        static void Difficulty()
        {
            Console.ResetColor();
            Console.WriteLine("\r\n    Merci d'entrer la difficulté pour votre jeu ");
            Console.WriteLine("    en sachant que : ");
            Console.WriteLine("         1--> niveau facile");
            Console.WriteLine("         2--> niveau moyen");
            Console.WriteLine("         3--> niveau difficile");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.Write("\nEntrez le niveau de difficulté: ");
            bool getOut = int.TryParse(Console.ReadLine(), out difficulty);
            if (getOut)
            {
                switch (difficulty)
                {
                    case 1: Console.WriteLine("→ Vous avez choisi le niveau FACILE !"); break;
                    case 2: Console.WriteLine("→ Vous avez choisi le niveau MOYEN !"); break;
                    case 3: Console.WriteLine("→ Vous avez choisi le niveau DIFFICILE !"); break;
                    default: Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("\n-Valeur hors limite ! Merci de choisir 1, 2 ou 3.\n"); Difficulty(); break;
                }
                Grid();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Saisie invalide !\n");

            }
            Difficulty();
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
            Console.WriteLine("");
            Console.ResetColor();



        }
        static void Grid()
        {
            Console.Clear();
            Title();
            instructions2();
            int i = 0;
            
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
            landMines = lines * collones ;
            switch (difficulty)
            {
                case 1: landMines = landMines / 10; break;
                case 2: landMines = landMines / 4; break;
                case 3: landMines = (landMines * 40 ) /100; break;
            }
            
            Console.WriteLine(landMines);
            movement();
            Console.ReadLine();
        }
        static void movement()
        {
            int lastSquare = (7 + collones * 4) -4;
            int lastSquareY = (6 + lines * 2) -2;
            Console.SetCursorPosition(7, 6);
            int firstSquare = Console.CursorLeft;
            for (int i =0; gagner == true; i++)
            {
                int positionX = Console.CursorLeft; int positionY = Console.CursorTop;
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                j++;
                switch (keyPressed.Key)
                {
                    case ConsoleKey.UpArrow: direction = "haut"; break;
                    case ConsoleKey.DownArrow: direction = "bas"; break;
                    case ConsoleKey.LeftArrow: direction = "gauche"; break;
                    case ConsoleKey.RightArrow: direction="droite"; break;
                    case ConsoleKey.Escape: Environment.Exit(0); break;
                    case ConsoleKey.Enter: Console.SetCursorPosition(positionX, positionY); Console.Write("X");Console.CursorLeft--;  break;
                    default: direction = ""; break;


                } 

                switch (direction)

                {
                    case "droite":
                        if (Console.CursorLeft >= lastSquare)
                        {
                            Console.CursorLeft = 7;
                        }
                        else
                        {
                            Console.CursorLeft += 4;
                        }
                        break;
                    case "gauche":
                        if (Console.CursorLeft <= 7)
                        {
                            Console.CursorLeft = lastSquare;
                        }
                        else
                        {
                            Console.CursorLeft -= 4;
                        }
                        break;
                    case "haut":
                        if (Console.CursorTop <= 6)
                        {
                            Console.CursorTop = lastSquareY;
                        }
                        else
                        {
                            Console.CursorTop -= 2;
                        }
                        break;
                    case "bas":
                        if (Console.CursorTop >= lastSquareY)
                        {
                            Console.CursorTop = 6;
                        }
                        else
                        {
                            Console.CursorTop += 2;
                        }
                        break;
                    default: break;
                }
            }
            
        }

    }
}
