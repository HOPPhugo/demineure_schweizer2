///ETML  
///Auteur : Hugo Schweizer
///Date   : 09.12.2025
///Description : Application de démineur simplifié en console C#, une grill est générée et le joueure doit mettre un drapeau sur les mines et explorer les cases sûres.

using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Lifetime;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace demineure_schweizer2
{
    internal class Program
    {
        public static bool win = false;//Stocke le statut de victoire du joueur
        static void Main(string[] args)
        {
            //Commence le jeu et gère la boucle principale du jeu
            GamePlay();

        }//Fin Main

        static void GamePlay()
        {
            do
            {
                List <char> lifeT = new List<char>();
                int rowsNb =0; //Nombre de lignes
                int columnsNb=0; //Nombre de colonnes
                int difficulty=0; //Niveau de difficulté
                int landMines=0; //Nombre de mines
                string direction = ""; //Direction du déplacement selon les touches fléchées
                int currentX=0; //Position X actuelle du curseur
                int currentY=0; //Position Y actuelle du curseur
                int heartX = 0; //Position X du dernier cœur
                int heartY = 0; //Position Y du dernier cœur
                int nbrDrapeau = 0;
                const int START = 10;

                Console.Clear();
                Console.ResetColor();

                //Affiche la bannière de titre
                Title();

                //Affiche les instructions des limites du plateau de jeu
                Instructions();

                //Demande le nombre de lignes et stocke la valeur
                Rows(ref rowsNb);

                //Demande le nombre de colonnes et stocke la valeur
                Columns(ref columnsNb);

                //Initialise la grille de jeu avec les dimensions choisies
                int[,] grid = new int[rowsNb, columnsNb];

                //Demande le niveau de difficulté et stocke la valeur
                Difficulty(ref difficulty, ref lifeT, grid, landMines);

                Console.Clear();

                //Affiche la bannière de titre
                Title();

                //Affiche les instructions du niveau de difficulté choisi
                instructions2(difficulty, landMines);

                //Genère et affiche la grille de jeu
                Grid(columnsNb,rowsNb,ref landMines,difficulty,ref grid,ref heartX,ref heartY, lifeT, START);

                //Affiche les consignes de jeu et les actions assignées aux touches
                Instructions3(columnsNb, START);

                //Place les mines sur la grille de jeu
                LandMines(landMines,columnsNb,rowsNb,ref grid, START, lifeT);

                //Gère les déplacements et les actions du joueur sur la grille
                Movement(columnsNb,rowsNb,ref direction,ref currentX,ref currentY,ref grid,ref heartX,ref heartY, ref lifeT, ref landMines, ref nbrDrapeau, START);
            } while (win == false);//Tant que le joueur n'a pas gagné

        }

        /// <summary>
        /// Affiche la bannière de titre du jeu.
        /// </summary>
        static void Title()
        {
            Console.WriteLine("\t****************************************************************************");
            Console.WriteLine("\t*                (: Démineur simplifié (Hugo Schweizer) :)                 *");
            Console.WriteLine("\t****************************************************************************");

        }//Title

        /// <summary>
        /// Affiche les instructions pour définir les dimensions du plateau de jeu.
        /// </summary>
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
        /// Demande à l'utilisateur d'entrer le nombre de lignes et valide l'entrée.
        /// </summary>
        /// <param name="rowsNb">Nombre de lignes pour la grille de jeu, compris entre 6 et 30</param>
        static void Rows(ref int rowsNb)
        { 
            bool rowsTrue = false;//vérifie si l'entrée est validée
            do {
                Console.ResetColor();
                Console.Write("Nombre de lignes : ");
                bool getOut = int.TryParse(Console.ReadLine(), out rowsNb);
                Console.ForegroundColor = ConsoleColor.Red;
                if (getOut)//vérifie si l'entrée est bien entre 6 et 30
                {
                    if (rowsNb < 6 || rowsNb > 30)
                    {
                        Console.WriteLine("Valeur hors limite ! Merci de réessayer !\n");
                    }
                    else
                    {
                        rowsTrue = true;//Entrée validée
                    }
                }
                else
                {
                    Console.WriteLine("Valeur hors limite ! Merci de réessayer !\n");
                }
            
            }while(rowsTrue==false);//Tant que l'entrée n'est pas validée

        }//Fin Rows

        /// <summary>
        /// Demande à l'utilisateur d'entrer le nombre de colonnes et la stocke la valeure.
        /// </summary>
        /// <param name="columnsNb">Nombre de collonnes pour la grille de jeu, compris entre 6 et 30</param>
        static void Columns(ref int columnsNb)
        {

            bool columnsTrue = false;//Vérrifie si l'entrée est validée
            do
            {
                Console.ResetColor();
                Console.Write("Nombre de colonnes: ");

                bool getOut = int.TryParse(Console.ReadLine(), out columnsNb);
                Console.ForegroundColor = ConsoleColor.Red;
                if (getOut)//Vérifie si l'entrée est bien entre 6 et 30
                {
                    if (columnsNb < 6 || columnsNb > 30)
                    {
                        Console.WriteLine("Valeur hors limite ! Merci de réessayer !\n");
                    }
                    else
                    {
                        Console.SetWindowSize(240, 63);
                        columnsTrue = true;//Entrée validée

                    }
                }
                else
                {
                    Console.WriteLine("Valeur hors limite ! Merci de réessayer !\n");

                }
                
            }
            while (columnsTrue==false);//Tant que l'entrée n'est pas validée
        }// Fin Columns

        /// <summary>
        /// Demande à l'utilisateur de choisir un niveau de difficulté et stocke la valeure.
        /// </summary>
        /// <param name="difficulty">Difficulté du jeu comrpise entre 1 et 3 </param>
        static void Difficulty(ref int difficulty, ref List <char> lifeT, int[,] grid, int landMines)
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
                if (getOut) //Vérifie si l'entrée est bien entre 1 et 3, sinon affiche un message d'erreur et redemande une entrée
                {
                    switch (difficulty) //Gère les différents cas de difficulté entrés
                    {
                        case 1: Console.WriteLine("→ Vous avez choisi le niveau FACILE !"); diffcultyTrue = true;
                            
                            break;
                        case 2: Console.WriteLine("→ Vous avez choisi le niveau MOYEN !"); diffcultyTrue = true; break;
                        case 3: Console.WriteLine("→ Vous avez choisi le niveau DIFFICILE !"); diffcultyTrue = true; break;
                        default: Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("\n-Valeur hors limite ! Merci de choisir 1, 2 ou 3.\n");break;
                    }
                }
                else //Si l'entrée n'est pas un entier
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Saisie invalide !\n");

                }
            }while (diffcultyTrue==false);//Tant que l'entrée n'est pas validée
        }//Fin Difficulty

        /// <summary>
        /// Affiche les instructions du niveau de difficulté choisi.
        /// </summary>
        /// <param name="difficulty">Niveau de difficulté du choisi par l'utilisateur</param>
        static void instructions2(int difficulty, int landMines)
        {
            switch (difficulty)//Gère l'affichage des instructions selon le niveau de difficulté choisi
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

        }//Fin Instructions2
        static void Grid(int columnsNb, int rowsNb, ref int landMines, int difficulty, ref int[,] grid, ref int heartX, ref int heartY, List<char> lifeT, int START)
        {
            int i = 0; //Compteur du nombre de tours de boucle
            Console.CursorLeft = 5;
            Console.CursorTop = START;
            Console.Write("╔═══╦");

            for (; i < columnsNb - 2; i++) //Dessine la partie supérieure de la grille selon le nombre de colonnes
            {
                Console.Write("═══╦");
            }
            Console.Write("═══╗");
            i = 0;

            Console.WriteLine("");
            Console.CursorLeft = 5;
            Console.Write("║  ");

            for (; i < columnsNb; i++)//Dessine les lignes intermédiaires de la grille selon le nombre de colonnes
            {

                Console.Write(" ║  ");
            }

            for (int j = 0; j < rowsNb - 1; j++)//Dessine les lignes intermédiaires verticales de la grille selon le nombre de lignes
            {
                Console.WriteLine("");
                Console.CursorLeft = 5;
                i = 0;
                Console.Write("╠═══╬");

                for (; i < columnsNb - 2; i++)//Dessine les lignes intermédiaires de la grille selon le nombre de colonnes
                {
                    Console.Write("═══╬");
                }
                Console.Write("═══╣");

                i = 0;
                Console.WriteLine("");
                Console.CursorLeft = 5;
                Console.Write("║  ");

                for (; i < columnsNb; i++)//Dessine les lignes intermédiaires de la grille selon le nombre de colonnes
                {

                    Console.Write(" ║  ");
                }
            }
            i = 0;
            Console.WriteLine("");
            Console.CursorLeft = 5;
            Console.Write("╚═══╩");

            for (; i < columnsNb - 2; i++)//Dessine la partie inférieure de la grille selon le nombre de colonnes
            {
                Console.Write("═══╩");
            }

            Console.Write("═══╝");

            landMines = (rowsNb / 2 + 1) * (columnsNb / 2 + 1); //Calcule la surface totale de la grille avce le calcule du cahier des charges

            switch (difficulty) //Gère le nombre de mines selon le niveau de difficulté choisi
            {
                case 1: landMines = landMines / 10; break;
                case 2: landMines = landMines / 4; break;
                case 3: landMines = (landMines * 40) / 100; break;
            }

            grid = new int[rowsNb, columnsNb];//Initialise la grille de jeu avec les dimensions choisies
            Console.ForegroundColor = ConsoleColor.DarkRed;
            foreach (char x in lifeT)
            {
                Console.Write(x);
            }
            Console.ResetColor();
            heartX = Console.CursorLeft-3; //Position X du début de la barre de vie
            heartY = Console.CursorTop; //Position Y du début du tableau
        }// Fin Grid

        /// <summary>
        /// //Gère les déplacements et les actions du joueur sur la grille.
        /// </summary>
        /// <param name="columnsNb">Nombre de collones choisi par le joueur</param>
        /// <param name="rowsNb">Nombre de lignes choisi par le joueur</param>
        /// <param name="direction">Référence à la direction actuelle du joueur dans la grille. Mise à jour en fonction des entrées de l'utilisateur.</param>
        /// <param name="currentX">Référence à la position X actuelle du joueur dans la grille. Mise à jour au fur et à mesure des déplacements du joueur.</param>
        /// <param name="currentY">Référence à la position Y actuelle du joueur dans la grille. Mise à jour au fur et à mesure des déplacements du joueur.</param>
        /// <param name="grid">Référence à la grille de jeu représentée sous forme de tableau à deux dimensions.</param>
        /// <param name="heartX">Position X du dernier cœur dans la grille.</param>
        /// <param name="heartY">Position Y du dernier cœur dans la grille.</param>
        static void Movement( int columnsNb, int rowsNb, ref string direction, ref int currentX, ref int currentY, ref int[,] grid, ref int heartX, ref int heartY, ref List <char> lifeT, ref int landMines, ref int nbrDrapeau, int START)
        {
            int lastSquare = (7 + columnsNb * 4) - 4; //Position X de la dernière case de la grille, calculée en fonction du nombre de colonnes et de la largeur des cases
            int lastSquareY = (START + rowsNb * 2) - 2; //Position Y de la dernière case de la grille, calculée en fonction du nombre de lignes et de la hauteur des cases
            int firstSquare = Console.CursorLeft; //Position X de la première case de la grille
            int life = 3; //Nombre de vies initiales du joueur
            for (int i = 0; win == false; i++) //Boucle principale du jeu qui continue tant que le joueur n'a pas gagné
            {
                string action = ""; // Action actuelle du joueur (explorer une case, placer un drapeau, etc...)
                direction = ""; // Réinitialise la direction à chaque itération
                int positionX = Console.CursorLeft; int positionY = Console.CursorTop; //Position actuelle du curseur
                ConsoleKeyInfo keyPressed = Console.ReadKey(); //Lit l'entrée de l'utilisateur (touche pressée) et la stocke
                switch (keyPressed.Key) //Gère les différentes actions de l'utilisateur
                {
                    case ConsoleKey.UpArrow: direction = "haut"; break;
                    case ConsoleKey.DownArrow: direction = "bas"; break;
                    case ConsoleKey.LeftArrow: direction = "gauche"; break;
                    case ConsoleKey.RightArrow: direction = "droite"; break;
                    case ConsoleKey.Escape: Environment.Exit(0); break;
                    case ConsoleKey.Enter: action = "Enter"; Console.SetCursorPosition(positionX, positionY); CheckMines(positionX, positionY, ref life, action,ref grid,currentX,currentY, heartX, heartY, ref lifeT, ref landMines, ref nbrDrapeau); break;
                    case ConsoleKey.Spacebar: action = "Spacebar"; Console.SetCursorPosition(positionX, positionY); CheckMines(positionX, positionY, ref life, action, ref grid, currentX, currentY, heartX, heartY,ref lifeT, ref landMines, ref nbrDrapeau); break;
                    default: action = "Nothing"; Console.SetCursorPosition(positionX, positionY); CheckMines(positionX, positionY, ref life, action,ref grid, currentX, currentY, heartX, heartY,ref  lifeT, ref landMines, ref nbrDrapeau); break;


                }

                switch (direction) //Gère les déplacements du joueur dans la grille en fonction de la direction choisie
                {
                    case "droite":
                        if (Console.CursorLeft == lastSquare)
                        {
                            Console.CursorLeft = 7;
                            currentX = 1;
                        }
                        else
                        {
                            Console.CursorLeft += 4;
                            currentX++;
                        }
                        break;
                    case "gauche":
                        if (Console.CursorLeft == 7)
                        {
                            Console.CursorLeft = lastSquare;
                            currentX = columnsNb;
                        }
                        else
                        {
                            Console.CursorLeft -= 4;
                            currentX--;
                        }
                        break;
                    case "haut":
                        if (Console.CursorTop <= START)
                        {
                            Console.CursorTop = lastSquareY;
                            currentY = rowsNb;
                        }
                        else
                        {
                            Console.CursorTop -= 2;
                            currentY--;
                        }
                        break;
                    case "bas":
                        if (Console.CursorTop >= lastSquareY)
                        {
                            Console.CursorTop = START;
                            currentY = 1;
                            currentY = 1;
                        }
                        else
                        {
                            Console.CursorTop += 2;
                            currentY++;
                        }
                        break;
                    default: break;
                }

            }

        }// Fin Movement

        /// <summary>
        /// Génère et place des mines aléatoirement sur la grille de jeu.
        /// </summary>
        /// <param name="landMines">Nombre de mines placées sur le terrains</param>
        /// <param name="columnsNb">Nombre de colonnes dans la grille de jeu.</param>
        /// <param name="rowsNb">Nombre de lignes dans la grille de jeu.</param>
        /// <param name="grid">Référence à la grille de jeu représentée sous forme de tableau à deux dimensions.</param>
        /// <returns> La grille de jeu mise à jour avec les mines placées.</returns>
        static int[,] LandMines(int landMines, int columnsNb, int rowsNb,ref int[,] grid, int START, List <char> lifeT)
        {
            Random random = new Random();
            Console.SetCursorPosition(7, START-6);
            for (int i = 0; i < landMines; i++)
            {
                lifeT.Add('♥');
            }
            Console.Write("Il y a ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(landMines);
            Console.ResetColor();
            Console.WriteLine(" mines cachées sur la grille !");
            for (int i = 0; i < landMines; i++) //Boucle pour placer le nombre de mines selon le niveau de difficulté choisi, refait un tour si une mine est déjà présente à l'emplacement choisi
            {
                int X = random.Next(0, columnsNb); //Génère un nombre aléatoire pour la position X de la mine, entre 0 et le nombre de colonnes
                int Y = random.Next(0, rowsNb); //Génère un nombre aléatoire pour la position Y de la mine, entre 0 et le nombre de lignes

                if (grid[Y, X] == 1)//Vérifie si une mine est déjà présente à l'emplacement choisi
                {
                    i--;
                }
                else //Place la mine à l'emplacement choisi (le "°" représente une mine dans la grille, seulement actif pour quand je developpe, ne sera présent dans la version jouable finale) 
                {
                    grid[Y, X] = 1;
                    Console.SetCursorPosition(7 + (Y * 4), START+1 + (X * 2)); //Calcule la position du curseur en fonction de la position de la mine dans la grille pour y écrire le symbole de la mine
                    Console.Write("°");
                    Console.CursorLeft--;
                }
            }
            Console.SetCursorPosition(7, START+1);
            return grid; //Retourne la grille de jeu mise à jour avec les mines placées

        }//Fin LandMines

        /// <summary>
        /// Mets à jour l'état de la grille de jeu en fonction de l'action du joueur et gère les conséquences de cette action.
        /// </summary>
        /// <param name="positionX">Coordonnée X du positionnement du joueur sur la console.</param>
        /// <param name="positionY">Coordonnée Y du positionnement du joueur sur la console.</param>
        /// <param name="life">Les vies restantes du joueur. Diminue de 1 si le joueur explore une case contenant une mine.</param>
        /// <param name="action">L'action effectuée par le joueur (explorer une case, placer un drapeau, etc...).</param>
        /// <param name="grid">La grille de jeu représentée sous forme de tableau à deux dimensions.</param>
        /// <param name="currentX">Coordonnée X actuelle du joueur sur la grille 2D.</param>
        /// <param name="currentY">Coordonnée Y actuelle du joueur sur la grille 2D.</param>
        /// <param name="heartX">Coordonnée X de l'icone du cœur représentant la vie du joueur sur la console.</param>
        /// <param name="heartY">Coordonnée Y de l'icone du cœur représentant la vie du joueur sur la console.</param>
        static void CheckMines(int positionX, int positionY, ref int life, string action, ref int[,] grid, int currentX, int currentY, int heartX, int heartY, ref List<char> lifeT, ref int landMines, ref int nbrDrapeau)
        {
            if (grid[currentX, currentY] == 0) //Case sûre
            {
                switch (action) //Gère les différentes actions possibles sur une case sûre
                {

                    case "Enter": Console.Write("▒"); grid[currentX, currentY] = 2; break;
                    case "Spacebar": Console.Write("◄"); break;
                    case "Nothing": Console.Write(" "); Console.CursorLeft++; break;


                }
            }
            else if (grid[currentX, currentY] == 1) //Case avec une mine
            {
                switch (action) //Gère les différentes actions possibles sur une case avec une mine
                {
                    case "Enter":
                        Console.Write("X"); life -= 1; Console.SetCursorPosition(heartX, heartY); Console.Write(" "); Console.Write(" "); Console.Write(" "); Console.SetCursorPosition(heartX, heartY);
                        lifeT[life] = ' ';
                        landMines--;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        foreach (char x in lifeT) {
                            Console.Write(x);
                        }

                        Console.ResetColor();

                            grid[currentX, currentY] = 3;

                        Console.Beep();
                        if (life == 0) //Si le joueur n'a plus de vie, affiche l'écran de fin de jeu
                        {
                            Looser();
                            break;
                        }
                        Console.SetCursorPosition(positionX + 1, positionY);

                        break;
                    case "Spacebar": Console.Write("◄");nbrDrapeau++; break;
                    case "Nothing": Console.Write(" "); break;
                    default: break;
                }


            }
            else if (grid[currentX, currentY] == 2)//Case déjà explorée et sûre
            {
                Console.Write("▒"); grid[currentX, currentY] = 2;

            }
            else if (grid[currentX, currentY] == 3) //Case déjà explorée et avec une mine
            {
                Console.Write("X");

            }
            if (nbrDrapeau == landMines)
            {
                Winner(life);
                Console.CursorLeft++;
            }
                Console.CursorLeft--;
        }//Fin CheckMines

        /// <summary>
        /// Affiche les consignes de jeu et les actions assignées aux touches.
        /// </summary>
        /// <param name="columnsNb">Nombre de colonnes dans la grille de jeu.</param>
        static void Instructions3(int columnsNb, int START)
        {
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Consignes");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START+1);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("----------");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START+2);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\t- Pour se déplacer dans le jeu utiliser les touches flèchées");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START+3);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\t- Pour explorer une case la touche Enter");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START+4);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\t- Pour définir une case en tant que mine (flag) la touche Espace");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START+5);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\t- La touche Enter sur un flag enlève le flag");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START+6);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\t- Pour quitter la touche Esc");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START+7);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("La partie est gagnée :");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START+8);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\t- une fois que toutes les cases ont été explorées");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START+9);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\t- que toutes les vies ont été épuisées");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START+10);
            Console.ResetColor();
        }//Fin Instructions3

        /// <summary>
        /// Affiche l'écran de fin de jeu lorsque le joueur perd toutes ses vies et demande s'il souhaite rejouer.
        /// </summary>
        static void Looser()
        {
            string answer; //Réponse du joueur pour rejouer ou quitter
            Console.Clear();
            Console.Beep();
            Console.Beep();
            Console.Beep();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("C'est la fin !\r\n\r\n!! PERDU !! Désolé toutes vos vies sont tombé eà zéro");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Veut tu rejouer (O/N) ?");
            Console.CursorLeft++; 
            
            do{
                answer = Console.ReadLine().ToUpper();            
                switch (answer) // Gère la réponse du joueur pour rejouer ou quitter
                {
                    case "O":
                        GamePlay(); break;
                    case "N":
                        Console.WriteLine("Bonne journée !"); 
                        Console.Read(); Environment.Exit(0); break;
                }
            } while (answer != "N" ||answer != "O"); //vérifie que la réponse est valide, sois oui ou non.
        }//Fin Looser

        /// <summary>
        /// Affiche l'écran de fin de jeu lorsque le joueur gagne et indique le nombre de vies restantes.
        /// </summary>
        /// <param name="life">Le nombre de vies restantes du joueur à la fin du jeu.</param>
        static void Winner(int life)
        {
            Console.Clear();
            Console.WriteLine("Félicitations ! Vous avez gagné ! \n Vous avez gangé avec " + life + " restante(s).");
        }//Fin Winner

    }//Fin Program

}//Fin NameSpaceA