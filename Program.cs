using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

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
        public static int currentX;
        public static int currentY;
        public static int[,] grid;
        public static int HeartX;
        public static int HeartY;
        static void Main(string[] args)
        {
            int nbRow = 0;

            //affichage un titre
            Title();

            //affichage des instructions
            instructions();

            // récupération du nombre de ligne

            Lines();

            // r
            Console.ReadLine();
        }//Main
        static void Title()
        {
            Console.WriteLine("\t****************************************************************************");
            Console.WriteLine("\t*                (: Démineur simplifié (Hugo Schweizer) :)                 *");
            Console.WriteLine("\t****************************************************************************");

        }//Title
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

        }//Instructions
        static void Lines()
        {
            Console.ResetColor();
            Console.Write("Nombre de lignes : ");
            bool getOut = int.TryParse(Console.ReadLine(), out lines);
            Console.ForegroundColor = ConsoleColor.Red;
            if (getOut)
            {
                if (lines < 6 || lines > 30)
                {
                    Console.WriteLine("Valeur hors limite ! Merci de réessayer !\n");
                }//if
                else
                {
                    Collones();
                }//else
            }//if
            else
            {
                Console.WriteLine("Valeur hors limite ! Merci de réessayer !\n");
            }//else
            Lines();

        }//Rows
        
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
                }//if
                else
                {
                    Console.SetWindowSize(240, 63);
                        Difficulty();
                }//else
            }//if
            else
            {
                Console.WriteLine("Valeur hors limite ! Merci de réessayer !\n");

            }//else
            Collones();
        }//Columns
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
                }//switch
                Grid();
                Movement();
            }//if
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Saisie invalide !\n");

            }//else
            Difficulty();
        }//Difficulty
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
            }//switch
            Console.WriteLine("");
            Console.ResetColor();



        }//Instructions2
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
            }//for
            Console.Write("═══╗");
            i = 0;

            Console.WriteLine("");
            Console.CursorLeft = 5;
            Console.Write("║  ");
            for (; i < collones; i++)
            {

                Console.Write(" ║  ");
            }//for
            for (int j = 0; j < lines - 1; j++)
            {
                Console.WriteLine("");
                Console.CursorLeft = 5;
                i = 0;
                Console.Write("╠═══╬");
                for (; i < collones - 2; i++)
                {
                    Console.Write("═══╬");
                }//for
                Console.Write("═══╣");

                i = 0;
                Console.WriteLine("");
                Console.CursorLeft = 5;
                Console.Write("║  ");
                for (; i < collones; i++)
                {

                    Console.Write(" ║  ");
                }//for
            }//for
            i = 0;
            Console.WriteLine("");
            Console.CursorLeft = 5;
            Console.Write("╚═══╩");
            for (; i < collones - 2; i++)
            {
                Console.Write("═══╩");
            }//for
            Console.Write("═══╝");
            landMines = (lines*2+1)*(collones*2+1);
            switch (difficulty)
            {
                case 1: landMines = landMines / 10;break;
                case 2: landMines = landMines / 4; break;
                case 3: landMines = (landMines * 40) / 100; break;
            }//switch
            grid = new int[lines, collones];
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("\t♥ ♥ ♥");
            Console.ResetColor();
            HeartX = Console.CursorLeft;
            HeartY = Console.CursorTop;
            consignes();
            LandMines();
        }//Grid
        static void Movement()
        {
            int lastSquare = (7 + collones * 4) - 4;
            int lastSquareY = (6 + lines * 2) - 2;
            int firstSquare = Console.CursorLeft;
            int life = 3;
            for (int i = 0; gagner == true; i++)
            {
                string action = "";
                direction = "";
                int positionX = Console.CursorLeft; int positionY = Console.CursorTop;
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                j++;
                switch (keyPressed.Key)
                {
                    case ConsoleKey.UpArrow: direction = "haut"; break;
                    case ConsoleKey.DownArrow: direction = "bas"; break;
                    case ConsoleKey.LeftArrow: direction = "gauche"; break;
                    case ConsoleKey.RightArrow: direction = "droite"; break;
                    case ConsoleKey.Escape: Environment.Exit(0); break;
                    case ConsoleKey.Enter: action = "Enter"; Console.SetCursorPosition(positionX, positionY); CheckMines(positionX, positionY, ref life, action); break;
                    case ConsoleKey.Spacebar: action = "Spacebar"; Console.SetCursorPosition(positionX, positionY); CheckMines(positionX, positionY, ref life, action); break;
                    default: action = "Nothing"; Console.SetCursorPosition(positionX, positionY); CheckMines(positionX, positionY, ref life, action); break;

                        
                }//switch

                switch (direction)
                {
                    case "droite":
                        if (Console.CursorLeft == lastSquare)
                        {
                            Console.CursorLeft = 7;
                            currentX = 1;
                        }//if
                        else
                        {
                            Console.CursorLeft += 4;
                            currentX++;
                        }//else
                        break;
                    case "gauche":
                        if (Console.CursorLeft == 7)
                        {
                            Console.CursorLeft = lastSquare;
                            currentX = collones;
                        }//if
                        else
                        {
                            Console.CursorLeft -= 4;
                            currentX--;
                        }//else
                        break;
                    case "haut":
                        if (Console.CursorTop <= 6)
                        {
                            Console.CursorTop = lastSquareY;
                            currentY = lines;
                        }//if
                        else
                        {
                            Console.CursorTop -= 2;
                            currentY--;
                        }//else
                        break;
                    case "bas":
                        if (Console.CursorTop >= lastSquareY)
                        {
                            Console.CursorTop = 6;
                            currentY = 1;
                        }//if
                        else
                        {
                            Console.CursorTop += 2;
                            currentY++;
                        }//else
                        break;
                    default: break;
                }//switch
                
            }//for

        }//Movement
        static int[,] LandMines()
        {
            Random random = new Random();
            for (int i = 0; i < landMines; i++)
            {
                int X = random.Next(0, collones);
                int Y = random.Next(0, lines);
                if (grid[X,Y] ==1)
                {
                    i--;
                }//if
                else{
                    grid[Y, X] = 1;
                    Console.SetCursorPosition(7 + (X * 4), 6 + (Y * 2));
                    Console.Write("°");
                    Console.CursorLeft--;
                }//else
            }//for
            Console.SetCursorPosition(7, 6);
            return grid;

        }//LandMines
        static void CheckMines(int positionX,int positionY, ref int life, string action)
        {
            if (grid[currentX, currentY] == 0)
            {
                switch (action)
                {

                    case "Enter": Console.Write("▒"); grid[currentX, currentY] = 2; break;
                    case "Spacebar": Console.Write("◄"); break;
                    case "Nothing": Console.Write(" "); Console.CursorLeft++; break;


                }//switch
            }//if
            else if (grid[currentX, currentY] == 1)
            {
                switch (action)
                {
                    case "Enter":
                        Console.Write("X"); life -= 1; Console.SetCursorPosition(HeartX -= 2, HeartY); Console.Write(" "); Console.Write(" "); grid[currentX, currentY] = 3;
                        if (life == 0)
                        {
                            Looser();
                        }//if
                        Console.SetCursorPosition(positionX+1, positionY);
                        break;
                    case "Spacebar": Console.Write("◄"); break;
                    case "Nothing": Console.Write(" "); break;
                    default: break;
                }//switch


            }//else if 
            else if (grid[currentX, currentY]==2)
            {
                switch (action)
                {
                    default: Console.Write("▒");grid[currentX, currentY] = 2; break;
                }//switch

            }//else if 
            else if (grid[currentX, currentY] == 3)
            {
                switch (action)
                {
                    default: Console.Write("X");break;
                }//switch

            }//else if
            Console.CursorLeft--;
        }//CheckMines
        static void consignes()
        {
            Console.SetCursorPosition((7 + collones * 4) + 4, 6);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Consignes");
            Console.SetCursorPosition((7 + collones * 4) + 4, 7);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("----------");
            Console.SetCursorPosition((7 + collones * 4) + 4, 8);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\t- Pour se déplacer dans le jeu utiliser les touches flèchées");
            Console.SetCursorPosition((7 + collones * 4) + 4, 9);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\t- Pour explorer une case la touche Enter");
            Console.SetCursorPosition((7 + collones * 4) + 4, 10);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\t- Pour définir une case en tant que mine (flag) la touche Espace");
            Console.SetCursorPosition((7 + collones * 4) + 4, 11);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\t- La touche Enter sur un flag enlève le flag");
            Console.SetCursorPosition((7 + collones * 4) + 4, 12);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\t- Pour quitter la touche Esc");
            Console.SetCursorPosition((7 + collones * 4) + 4, 14);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("La partie est gagnée :");
            Console.SetCursorPosition((7 + collones * 4) + 4, 15);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\t- une fois que toutes les cases ont été explorées");
            Console.SetCursorPosition((7 + collones * 4) + 4, 16);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\t- que toutes les vies ont été épuisées");
            Console.SetCursorPosition((7 + collones * 4) + 4, 17);§
            Console.ResetColor();
        }//Consignes
        static void Looser()
        {
            Console.Clear(); 
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("C'est la fin !\r\n\r\n!! PERDU !! Désolé toutes les mines ont explosés !");
            Console.ForegroundColor=ConsoleColor.Green;
            Console.WriteLine("Veut tu rejouer (O/N) ?");
            string answer = Console.ReadLine();
            switch(answer)
            {
                case "O":
                case "o":
                    Console.Clear();
                    Console.ResetColor();
                    Title();
                    instructions();
                    Lines(); break;
                case "N":
                case "n":Environment.Exit(0); break;
            }//switch
        }//Looser
        

    }//Program
}//NameSpace
