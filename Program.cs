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
        public static bool win = false;
        static void Main(string[] args)
        {
            //Commence le jeu et gère la boucle principale du jeu
            GamePlay();

        }//Main

        static void GamePlay()
        {


            do
            {
                int rowsNb =0; //Nombre de lignes
                int columnsNb=0; //Nombre de colonnes
                int difficulty=0; //Niveau de difficulté
                int landMines=0; //Nombre de mines
                string direction = ""; //Direction du déplacement selon les touches fléchées
                int currentX=0; //Position X actuelle du curseur
                int currentY=0; //Position Y actuelle du curseur
                int heartX = 0; //Position X du dernier cœur
                int heartY = 0; //Position Y du dernier cœur

                Console.Clear();
                Console.ResetColor();

                //Show the title banner
                Title();

                //Show game instructions
                Instructions();

                //Prompt for number of rows and stock the value
                Rows(ref rowsNb);

                //Prompt for number of columns and stock the value
                Columns(ref columnsNb);

                int[,] grid = new int[rowsNb, columnsNb];

                //Prompt for difficulty level and stock the value
                Difficulty(ref difficulty);

                Console.Clear();

                //Show the title banner again after clearing the console
                Title();

                //Display game mode instructions
                instructions2(difficulty);
                //Generation of the game grid and display it
                Grid(columnsNb,rowsNb,ref landMines,difficulty,ref grid,ref heartX,ref heartY);

                //Display game rules and controls
                Instructions3(columnsNb);

                //Place landmines on the grid
                LandMines(landMines,columnsNb,rowsNb,ref grid);

                //Manage player movement and interactions
                Movement(columnsNb,rowsNb,ref direction,ref currentX,ref currentY,ref grid,ref heartX,ref heartY);
            } while (win == false);

        }

        /// <summary>
        /// Displays the title banner for the simplified Minesweeper game.
        /// </summary>
        /// <remarks>This method outputs a decorative title banner to the console, providing a visual
        /// introduction to the game.</remarks>
        static void Title()
        {
            Console.WriteLine("\t****************************************************************************");
            Console.WriteLine("\t*                (: Démineur simplifié (Hugo Schweizer) :)                 *");
            Console.WriteLine("\t****************************************************************************");

        }//Title

        /// <summary>
        /// Displays instructions for setting up the game board dimensions.
        /// </summary>
        /// <remarks>The instructions specify the minimum and maximum allowable dimensions for the game
        /// board. The minimum size is 6 rows by 6 columns, and the maximum size is 30 rows by 30 columns.</remarks>
        static void Instructions()
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

        /// <summary>
        /// Prompts the user to input the number of rows, ensuring the value is within a valid range.
        /// </summary>
        /// <remarks>The method repeatedly prompts the user until a valid integer between 6 and 30
        /// (inclusive) is entered. If the input is invalid, an error message is displayed, and the user is asked to try
        /// again.</remarks>
        static void Rows(ref int rowsNb)
        { 
            bool rowsTrue = false;//check if the input is validated
            do {
                Console.ResetColor();
                Console.Write("Nombre de lignes : ");
                bool getOut = int.TryParse(Console.ReadLine(), out rowsNb);
                Console.ForegroundColor = ConsoleColor.Red;
                if (getOut)//check if the input is an integer
                {
                    if (rowsNb < 6 || rowsNb > 30)
                    {
                        Console.WriteLine("Valeur hors limite ! Merci de réessayer !\n");
                    }//if
                    else
                    {
                        rowsTrue = true;//the input is validated
                    }//else
                }//if
                else
                {
                    Console.WriteLine("Valeur hors limite ! Merci de réessayer !\n");
                }//else
            
            }while(rowsTrue==false);//while the input is not validated

        }//Rows

        /// <summary>
        /// Prompts the user to input the number of columns and validates the input.
        /// </summary>
        /// <remarks>The method ensures that the user enters a valid integer within the range of 6 to 30.
        /// If the input is invalid, the user is prompted to try again until a valid value is provided. Once a valid
        /// value is entered, the console window size is adjusted accordingly.</remarks>
        static void Columns(ref int columnsNb)
        {

            bool columnsTrue = false;//check if the input is validated
            do
            {
                Console.ResetColor();
                Console.Write("Nombre de colonnes: ");

                bool getOut = int.TryParse(Console.ReadLine(), out columnsNb);
                Console.ForegroundColor = ConsoleColor.Red;
                if (getOut)//check if the input is an integer
                {
                    if (columnsNb < 6 || columnsNb > 30)
                    {
                        Console.WriteLine("Valeur hors limite ! Merci de réessayer !\n");
                    }//if
                    else
                    {
                        Console.SetWindowSize(240, 63);
                        columnsTrue = true;//the input is validated

                    }//else
                }//if
                else
                {
                    Console.WriteLine("Valeur hors limite ! Merci de réessayer !\n");

                }//else
                
            }
            while (columnsTrue==false);//while the input is not validated
        }//Columns

        /// <summary>
        /// Prompts the user to select a difficulty level for the game and processes the input.
        /// </summary>
        /// <remarks>The user is presented with three difficulty options: easy, medium, and hard. The
        /// method validates the input and provides feedback based on the selected difficulty. If the input is invalid,
        /// the user is prompted again.</remarks>
        static void Difficulty(ref int difficulty)
        {
            bool diffcultyTrue = false;
            do{ 
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
                        case 1: Console.WriteLine("→ Vous avez choisi le niveau FACILE !"); diffcultyTrue = true; break;
                        case 2: Console.WriteLine("→ Vous avez choisi le niveau MOYEN !"); diffcultyTrue = true; break;
                        case 3: Console.WriteLine("→ Vous avez choisi le niveau DIFFICILE !"); diffcultyTrue = true; break;
                        default: Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("\n-Valeur hors limite ! Merci de choisir 1, 2 ou 3.\n");break;
                    }//switch
                }//if
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Saisie invalide !\n");

                }//else
            }while (diffcultyTrue==false) ;//while the input is not validated
        }//Difficulty

        /// <summary>
        /// Displays a message indicating the current game mode based on the selected difficulty level.
        /// </summary>
        /// <remarks>The method outputs a message to the console, including the difficulty level, with
        /// specific background and foreground colors for each difficulty.</remarks>
        static void instructions2(int difficulty)
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
        static void Grid(int columnsNb, int rowsNb, ref int landMines, int difficulty, ref int[,] grid, ref int heartX, ref int heartY)
        {
            int i = 0;
            Console.CursorLeft = 5;
            Console.Write("╔═══╦");

            for (; i < columnsNb - 2; i++)
            {
                Console.Write("═══╦");
            }//for
            Console.Write("═══╗");
            i = 0;

            Console.WriteLine("");
            Console.CursorLeft = 5;
            Console.Write("║  ");

            for (; i < columnsNb; i++)
            {

                Console.Write(" ║  ");
            }//for

            for (int j = 0; j < rowsNb - 1; j++)
            {
                Console.WriteLine("");
                Console.CursorLeft = 5;
                i = 0;
                Console.Write("╠═══╬");

                for (; i < columnsNb - 2; i++)
                {
                    Console.Write("═══╬");
                }//for
                Console.Write("═══╣");

                i = 0;
                Console.WriteLine("");
                Console.CursorLeft = 5;
                Console.Write("║  ");

                for (; i < columnsNb; i++)
                {

                    Console.Write(" ║  ");
                }//for
            }//for
            i = 0;
            Console.WriteLine("");
            Console.CursorLeft = 5;
            Console.Write("╚═══╩");

            for (; i < columnsNb - 2; i++)
            {
                Console.Write("═══╩");
            }//for

            Console.Write("═══╝");

            landMines = (rowsNb * 2 + 1) * (columnsNb * 2 + 1);

            switch (difficulty)
            {
                case 1: landMines = landMines / 10; break;
                case 2: landMines = landMines / 4; break;
                case 3: landMines = (landMines * 40) / 100; break;
            }//switch

            grid = new int[rowsNb, columnsNb];
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("\t♥ ♥ ♥");
            Console.ResetColor();
            heartX = Console.CursorLeft;
            heartY = Console.CursorTop;
        }//Grid
        static void Movement( int columnsNb, int rowsNb, ref string direction, ref int currentX, ref int currentY, ref int[,] grid, ref int heartX, ref int heartY)
        {
            int lastSquare = (7 + columnsNb * 4) - 4;
            int lastSquareY = (6 + rowsNb * 2) - 2;
            int firstSquare = Console.CursorLeft;
            int life = 3;
            for (int i = 0; win == false; i++)
            {
                string action = "";
                direction = "";
                int positionX = Console.CursorLeft; int positionY = Console.CursorTop;
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                switch (keyPressed.Key)
                {
                    case ConsoleKey.UpArrow: direction = "haut"; break;
                    case ConsoleKey.DownArrow: direction = "bas"; break;
                    case ConsoleKey.LeftArrow: direction = "gauche"; break;
                    case ConsoleKey.RightArrow: direction = "droite"; break;
                    case ConsoleKey.Escape: Environment.Exit(0); break;
                    case ConsoleKey.Enter: action = "Enter"; Console.SetCursorPosition(positionX, positionY); CheckMines(positionX, positionY, ref life, action,ref grid,currentX,currentY, heartX, heartY); break;
                    case ConsoleKey.Spacebar: action = "Spacebar"; Console.SetCursorPosition(positionX, positionY); CheckMines(positionX, positionY, ref life, action, ref grid, currentX, currentY, heartX, heartY); break;
                    default: action = "Nothing"; Console.SetCursorPosition(positionX, positionY); CheckMines(positionX, positionY, ref life, action,ref grid, currentX, currentY, heartX, heartY); break;


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
                            currentX = columnsNb;
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
                            currentY = rowsNb;
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

        /// <summary>
        /// Generate randomly some mines in the grid
        /// positions.
        /// </summary>
        /// <remarks>This method generates a specified number of landmines and places them randomly on the
        /// grid.  If a randomly chosen position already contains a landmine, the method retries until the required
        /// number of landmines is placed. The grid is updated in-place, and the positions of the landmines are
        /// displayed on the console.</remarks>
        /// <returns>A two-dimensional array representing the grid, where cells with a value of 1 indicate the presence of a
        /// landmine.</returns>
        static int[,] LandMines(int landMines, int columnsNb, int rowsNb,ref int[,] grid)
        {
            Random random = new Random();
            for (int i = 0; i < landMines; i++)
            {
                int X = random.Next(0, columnsNb);
                int Y = random.Next(0, rowsNb);
                if (grid[X, Y] == 1)
                {
                    i--;
                }//if
                else
                {
                    grid[Y, X] = 1;
                    Console.SetCursorPosition(7 + (Y * 4), 6 + (X * 2));
                    Console.Write("°");
                    Console.CursorLeft--;
                }//else
            }//for
            Console.SetCursorPosition(7, 6);
            return grid;

        }//LandMines
        static void CheckMines(int positionX, int positionY, ref int life, string action, ref int[,] grid, int currentX, int currentY, int heartX, int heartY)
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
                        Console.Write("X"); life -= 1; Console.SetCursorPosition(heartX -= 2, heartY); Console.Write(" "); Console.Write(" "); grid[currentX, currentY] = 3;
                        Console.Beep();
                        if (life == 0)
                        {
                            Looser(); break;
                        }//if
                        Console.SetCursorPosition(positionX + 1, positionY);
                        break;
                    case "Spacebar": Console.Write("◄"); break;
                    case "Nothing": Console.Write(" "); break;
                    default: break;
                }//switch


            }//else if 
            else if (grid[currentX, currentY] == 2)
            {
                switch (action)
                {
                    default: Console.Write("▒"); grid[currentX, currentY] = 2; break;
                }//switch

            }//else if 
            else if (grid[currentX, currentY] == 3)
            {
                switch (action)
                {
                    default: Console.Write("X"); break;
                }//switch

            }//else if
                Console.CursorLeft--;
        }//CheckMines
        static void Instructions3(int columnsNb)
        {
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, 6);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Consignes");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, 7);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("----------");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, 8);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\t- Pour se déplacer dans le jeu utiliser les touches flèchées");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, 9);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\t- Pour explorer une case la touche Enter");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, 10);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\t- Pour définir une case en tant que mine (flag) la touche Espace");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, 11);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\t- La touche Enter sur un flag enlève le flag");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, 12);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\t- Pour quitter la touche Esc");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, 14);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("La partie est gagnée :");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, 15);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\t- une fois que toutes les cases ont été explorées");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, 16);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\t- que toutes les vies ont été épuisées");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, 17);
            Console.ResetColor();
        }//Consignes
        static void Looser()
        {
            Console.Clear();
            Console.Beep();
            Console.Beep();
            Console.Beep();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("C'est la fin !\r\n\r\n!! PERDU !! Désolé toutes les mines ont explosés !");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Veut tu rejouer (O/N) ?");
            string answer = Console.ReadLine();
            switch (answer)
            {
                case "O":
                case "o":
                    GamePlay(); break;
                case "N":
                case "n":
                    Console.WriteLine("Bonne journée !"); 
                    Console.Read(); Environment.Exit(0); break;
            }//switch
        }//Looser


    }//Program
}//NameSpaceA