///ETML  
///Auteur : Hugo Schweizer
///Date   : 21.12.2025
///Description : Application de démineur simplifié en console C#. 
///Une grille est générée et le joueur doit placer des drapeaux sur les mines et explorer les cases sûres.

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
using static System.Collections.Specialized.BitVector32;

namespace demineure_schweizer2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool win = false;
            do
            {
                List<char> lifeT = new List<char>();
                int rowsNb = 0; //Nombre de lignes
                int columnsNb = 0; //Nombre de colonnes
                int difficulty = 0; //Niveau de difficulté
                int landMines = 0; //Nombre de mines
                string direction = ""; //Direction du déplacement selon les touches fléchées
                int currentX = 0; //Position X actuelle du curseur
                int currentY = 0; //Position Y actuelle du curseur
                int heartX = 0; //Position X du dernier cœur
                int heartY = 0; //Position Y du dernier cœur
                int nbrDrapeau = 0;
                int start = 10;
                win = false;
                bool matchOver = false;
                int life = 0;
                int lastSquare = 0;
                int lastSquareY = 0;
                int firstSquare = 0;
                string action = "";
                int positionX = 0;
                int positionY = 0;
                int actualTextY = 0;
                Console.Clear();
                Console.ResetColor();
                Console.SetWindowSize(120, 30);

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
                Difficulty(ref difficulty);

                Console.Clear();

                //Affiche la bannière de titre
                Title();

                //Affiche les instructions du niveau de difficulté choisi
                instructions2(difficulty, landMines);

                //Calcule le nombre de mines selon le niveau de difficulté et la taille du terrain
                CalculeLandmines(rowsNb, columnsNb, ref landMines, difficulty);

                //Génère et affiche la grille de jeu
                Grid(columnsNb, rowsNb, ref landMines, difficulty, ref grid, ref heartX, ref heartY, lifeT, ref start, ref actualTextY);


                //Affiche les consignes de jeu et les actions assignées aux touches
                Instructions3(columnsNb, start);

                //Place les mines sur la grille de jeu
                PhysicalAndLogicalLandMines(landMines, columnsNb, rowsNb, ref grid, start, lifeT, heartX, heartY, difficulty, ref life);
                firstSquare = 7; //Position X de la première case de la grille

                //Affiche la barre de vie du joueur
                LifeBar(heartX, heartY, ref lifeT, start, difficulty, landMines, ref life);

                do
                {
                    bool haveToCheck = false;
                    //Gère les déplacements et les actions du joueur sur la grille
                    Movement(difficulty, ref lastSquare, ref lastSquareY, ref firstSquare, ref action, ref positionX, ref positionY, win, columnsNb, rowsNb, ref direction, ref currentX, ref currentY, ref grid, ref heartX, ref heartY, ref lifeT, ref landMines, ref nbrDrapeau, start, ref life, ref haveToCheck);
                    if (haveToCheck == true)
                    {
                        //Met à jour l'état de la grille de jeu en fonction de l'action du joueur et gère les conséquences de cette action
                        CheckMines(positionX, positionY, ref life, action, ref grid, currentX, currentY, heartX, heartY, ref lifeT, ref landMines, ref nbrDrapeau);
                    }
                    //Affiche le nombre de mines restantes sur le terrain
                    ShowActualPlacedMines(landMines, positionX, positionY, 4, start + 12, rowsNb, actualTextY);
                    if (life == 0)
                    {
                        //Affiche l'écran de fin de jeu lorsque le joueur perd toutes ses vies
                        Looser(ref win);
                        matchOver = true;
                    }
                    if (nbrDrapeau == landMines)
                    {
                        Console.CursorLeft++;

                        //Affiche l'écran de fin de jeu lorsque le joueur gagne
                        Winner(life, ref win);
                        matchOver = true;
                    }
                } while (matchOver == false); //Tant que la partie n'est pas terminée
            } while (win == false);

        }//Fin Main

        /// <summary>
        /// Affiche la bannière de titre du jeu.
        /// </summary>
        static void Title()
        {
            Console.SetWindowSize(240, 63);
            Console.WriteLine("\t****************************************************************************");
            Console.WriteLine("\t*                (: Démineur simplifié (Hugo Schweizer) :)                 *");
            Console.WriteLine("\t****************************************************************************");

        }//Fin Title

        /// <summary>
        /// Affiche les instructions pour définir les dimensions du plateau de jeu.
        /// </summary>
        static void Instructions()
        {
            Console.SetCursorPosition(3, 4);
            Console.WriteLine("Merci d'entrer le nombre de lignes et de colonnes de votre plateau de jeu");
            Console.SetCursorPosition(3, 5);
            Console.WriteLine("en sachant qu'au minimum le plateau fait 6 lignes x 6 colonnes !");
            Console.SetCursorPosition(3, 6);
            Console.WriteLine("et au maximum 30 lignes x 30 colonnes !");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.SetCursorPosition(0, 9);

        }//Fin Instructions

        /// <summary>
        /// Demande à l'utilisateur d'entrer le nombre de lignes et valide l'entrée.
        /// </summary>
        /// <param name="rowsNb">Nombre de lignes pour la grille de jeu, compris entre 6 et 30</param>
        static void Rows(ref int rowsNb)
        {
            bool rowsTrue = false;//vérifie si l'entrée est validée
            do
            {
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
                    Console.WriteLine("Veuillez entrer un chiffre !\n");
                }

            } while (rowsTrue == false);//Tant que l'entrée n'est pas validée

        }//Fin Rows

        /// <summary>
        /// Demande à l'utilisateur d'entrer le nombre de colonnes et la stocke la valeur.
        /// </summary>
        /// <param name="columnsNb">Nombre de colonnes pour la grille de jeu, compris entre 6 et 30</param>
        static void Columns(ref int columnsNb)
        {

            bool columnsTrue = false;//Vérifie si l'entrée est validée
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
                        columnsTrue = true;//Entrée validée

                    }
                }
                else
                {
                    Console.WriteLine("Veuillez entrer un chiffre !\n");

                }

            }
            while (columnsTrue == false);//Tant que l'entrée n'est pas validée
        }// Fin Columns

        /// <summary>
        /// Demande à l'utilisateur de choisir un niveau de difficulté et stocke la valeur.
        /// </summary>
        /// <param name="difficulty">Difficulté du jeu comprise entre 1 et 3 </param>
        static void Difficulty(ref int difficulty)
        {
            bool diffcultyTrue = false;
            do
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
                if (getOut) //Vérifie si l'entrée est bien entre 1 et 3, sinon affiche un message d'erreur et redemande une entrée
                {

                    switch (difficulty) //Gère les différents cas de difficulté entrés
                    {
                        case 1:
                            Console.WriteLine("→ Vous avez choisi le niveau FACILE !"); diffcultyTrue = true;

                            break;
                        case 2: Console.WriteLine("→ Vous avez choisi le niveau MOYEN !"); diffcultyTrue = true; break;
                        case 3: Console.WriteLine("→ Vous avez choisi le niveau DIFFICILE !"); diffcultyTrue = true; break;
                        default: Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("\n-Valeur hors limite ! Merci de choisir 1, 2 ou 3.\n"); break;
                    }
                }
                else //Si l'entrée n'est pas un entier
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Saisie invalide !\n");

                }
            } while (diffcultyTrue == false);//Tant que l'entrée n'est pas validée
            Console.ReadKey();
        }//Fin Difficulty

        /// <summary>
        /// Affiche les instructions du niveau de difficulté choisi.
        /// </summary>
        /// <param name="difficulty">Niveau de difficulté du choisi par l'utilisateur</param>
        /// <param name="landMines">Nombre de mines calculé selon le niveau de difficulté et la taille du terrain</param>
        static void instructions2(int difficulty, int landMines)
        {
            Console.Write("À vous de jouer ! Mode : ");
            Console.BackgroundColor = ConsoleColor.Yellow;
            switch (difficulty)//Gère l'affichage des instructions selon le niveau de difficulté choisi
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Facile");
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("Moyen");
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Difficile");
                    break;
            }
            Console.ResetColor();
            Console.WriteLine("");
            Console.ResetColor();

        }//Fin Instructions2

        /// <summary>
        /// Calcule le nombre de mines en fonction de la taille du terrain et du niveau de difficulté choisi.
        /// </summary>
        /// <param name="rowsNb"> Nombre de lignes choisies  par l'utilisateur</param>
        /// <param name="columnsNb"> Nombre de colonnes choisi par l'utilisateur</param>
        /// <param name="landMines">Nombre de mines calculé selon le niveau de difficulté et la taille du terrain</param>
        /// <param name="difficulty"> Niveau de difficulté choisi par l'utilisateur</param>
        static void CalculeLandmines(int rowsNb, int columnsNb, ref int landMines, int difficulty)
        {
            landMines = (rowsNb / 2 + 1) * (columnsNb / 2 + 1); //Calcule la surface totale de la grille avec le calcul conformément au cahier des charges (va devenir le nombre de mines)

            switch (difficulty) //Gère le nombre de mines selon le niveau de difficulté choisi
            {
                case 1: landMines = landMines / 10; break;
                case 2: landMines = landMines / 4; break;
                case 3: landMines = (landMines * 40) / 100; break;
            }
        }// Fin CalculeLandmines

        /// <summary>
        /// Dessine la grille de jeu dans la console et initialise les paramètres associés.
        /// </summary>
        /// <param name="columnsNb">Le nombre de colonnes dans le tableau</param>
        /// <param name="rowsNb">Le nombre de lignes dans le tableau.</param>
        /// <param name="landMines">Retourne le nombre de mines placées sur le terrain</param>
        /// <param name="difficulty">La difficulté choisie par le joueur.</param>
        /// <param name="grid">Retourne le tableau à deux dimensions</param>
        /// <param name="heartX">Retourne la coordonnée X où la barre de vie commence.</param>
        /// <param name="heartY">Retourne la coordonnée Y où la barre de vie commence.</param>
        /// <param name="lifeT">Liste représentant la barre de vie.</param>
        /// <param name="START">Retourne la position Y de début de la grille.</param>
        static void Grid(int columnsNb, int rowsNb, ref int landMines, int difficulty, ref int[,] grid, ref int heartX, ref int heartY, List<char> lifeT, ref int START, ref int actualTextY)
        {
            int i = 0; //Compteur du nombre de tours de boucle
            Console.CursorLeft = 5;
            Console.CursorTop = START++;
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
            grid = new int[rowsNb, columnsNb];//Initialise la grille de jeu avec les dimensions choisies
            heartX = Console.CursorLeft + 1; //Position X du début de la barre de vie
            heartY = Console.CursorTop; //Position Y du début du tableau
            actualTextY = Console.CursorTop + 1; //Position Y du texte des mines restantes
        }// Fin Grid


        /// <summary>
        /// Affiche les consignes de jeu et les actions assignées aux touches.
        /// </summary>
        /// <param name="columnsNb">Le nombre de colonnes choisi par le joueur</param>
        /// <param name="START">Le point de départ Y de la grille</param>
        static void Instructions3(int columnsNb, int START)
        {
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Consignes");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START + 1);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("----------");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START + 2);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\t- Pour se déplacer dans le jeu, utilisez les touches fléchées");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START + 3);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\t- Pour explorer une case : la touche Entrée");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START + 4);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\t- Pour marquer une case comme mine (drapeau) : la touche Espace");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START + 5);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\t- La touche Entrée sur un drapeau retire le drapeau");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START + 6);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\t- Pour quitter : la touche Échap");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START + 7);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("La partie est gagnée :");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START + 8);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\t- une fois que toutes les cases ont été explorées");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START + 9);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\t- que toutes les vies ont été épuisées");
            Console.SetCursorPosition((7 + columnsNb * 4) + 4, START + 10);
            Console.ResetColor();
        }//Fin Instructions3


        /// <summary>
        /// Place des mines physiquement et logiquement sur la grille de jeu et dans le tableau 2D.
        /// </summary>
        /// <param name="landMines">Le total de mines à placer sur le terrain.</param>
        /// <param name="columnsNb">Le nombre de colonnes du tableau.</param>
        /// <param name="rowsNb">Le nombre de lignes du tableau.</param>
        /// <param name="grid">Le tableau à deux dimensions représentant la grille de jeu.</param>
        /// <param name="START">Le point de départ Y de la grille.</param>
        /// <param name="lifeT">Liste représentant la barre de vie.</param>
        /// <param name="heartX">La coordonnée X où la barre de vie commence.</param>
        /// <param name="heartY">La coordonnée Y où la barre de vie commence.</param>
        /// <param name="difficulty">La difficulté choisie par le joueur.</param>
        /// <param name="life">Les vies restantes du joueur.</param>
        /// <returns>Retourne la grille de jeu mise à jour avec les mines placées.</returns>
        static int[,] PhysicalAndLogicalLandMines(int landMines, int columnsNb, int rowsNb, ref int[,] grid, int START, List<char> lifeT, int heartX, int heartY, int difficulty, ref int life)
        {
            Random random = new Random();


            for (int i = 0; i < landMines; i++) //Boucle pour placer le nombre de mines selon le niveau de difficulté choisi, refait un tour si une mine est déjà présente à l'emplacement choisi
            {
                int X = random.Next(0, columnsNb); //Génère un nombre aléatoire pour la position X de la mine, entre 0 et le nombre de colonnes
                int Y = random.Next(0, rowsNb); //Génère un nombre aléatoire pour la position Y de la mine, entre 0 et le nombre de lignes

                if (grid[Y, X] == 1)//Vérifie si une mine est déjà présente à l'emplacement choisi
                {
                    i--;
                }
                else //Place la mine à l'emplacement choisi (le "°" représente une mine dans la grille, uniquement pour le développement, ne sera pas présent dans la version jouable finale) 
                {
                    grid[Y, X] = 1;
                    Console.SetCursorPosition(7 + (Y * 4), START + (X * 2)); //Calcule la position du curseur en fonction de la position de la mine dans la grille pour y écrire le symbole de la mine
                    Console.Write("");
                    Console.CursorLeft--;
                }
            }
            Console.SetCursorPosition(7, START - 6);

            Console.Write("Il y a ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(landMines);
            Console.ResetColor();
            Console.WriteLine(" mines cachées sur la grille !");
            return grid; //Retourne la grille de jeu mise à jour avec les mines placées

        }//Fin PhysicalAndLogicalLandMines


        /// <summary>
        /// Affiche la barre de vie du joueur en fonction de la difficulté choisie et du nombre de mines.
        /// </summary>
        /// <param name="heartX">La coordonnée X dans la console où la barre de vie est affichée.</param>
        /// <param name="heartY">La coordonnée Y dans la console où la barre de vie est affichée.</param>
        /// <param name="lifeT">Liste représentant la barre de vie.</param>
        /// <param name="START">Le point de départ Y de la grille.</param>
        /// <param name="difficulty">La difficulté choisie par le joueur.</param>
        /// <param name="landMines">Le nombre de mines placées sur le terrain.</param>
        /// <param name="life">La vie du joueur.</param>
        static void LifeBar(int heartX, int heartY, ref List<char> lifeT, int START, int difficulty, int landMines, ref int life)
        {
            switch (difficulty)
            {
                case 1:
                    for (int i = 0; i < landMines; i++)
                    {
                        lifeT.Add('♥');
                        life++;
                    }
                    break;

                case 2:
                    for (int i = 0; i < landMines / 2; i++)
                    {
                        lifeT.Add('♥');
                        life++;
                    }
                    break;

                case 3:
                    for (int i = 0; i < 3; i++)
                    {
                        lifeT.Add('♥');
                        life++;
                    }
                    break;
            }

            Console.SetCursorPosition(heartX, heartY);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.OutputEncoding = Encoding.UTF8;
            foreach (char c in lifeT)
            {
                Console.Write(c + " ");
            }
            Console.ResetColor();
            Console.SetCursorPosition(7, START);
        }// Fin LifeBar


        /// <summary>
        /// //Gère les déplacements et les actions du joueur sur la grille.
        /// </summary>
        /// <param name="columnsNb">Nombre de colonnes choisi par le joueur</param>
        /// <param name="rowsNb">Nombre de lignes choisi par le joueur</param>
        /// <param name="direction">Référence à la direction actuelle du joueur dans la grille. Mise à jour en fonction des entrées de l'utilisateur.</param>
        /// <param name="currentX">Référence à la position X actuelle du joueur dans la grille. Mise à jour au fur et à mesure des déplacements du joueur.</param>
        /// <param name="currentY">Référence à la position Y actuelle du joueur dans la grille. Mise à jour au fur et à mesure des déplacements du joueur.</param>
        /// <param name="grid">Référence à la grille de jeu représentée sous forme de tableau à deux dimensions.</param>
        /// <param name="heartX">Position X du dernier cœur dans la grille.</param>
        /// <param name="heartY">Position Y du dernier cœur dans la grille.</param>
        static void Movement(int difficulty, ref int lastSquare, ref int lastSquareY, ref int firstSquare, ref string action, ref int positionX, ref int positionY, bool win, int columnsNb, int rowsNb, ref string direction, ref int currentX, ref int currentY, ref int[,] grid, ref int heartX, ref int heartY, ref List<char> lifeT, ref int landMines, ref int nbrDrapeau, int START, ref int life, ref bool haveToCheck)
        {

            for (int i = 0; i < 1; i++)
            {
                lastSquare = (7 + columnsNb * 4) - 4; //Position X de la dernière case de la grille, calculée en fonction du nombre de colonnes et de la largeur des cases
                lastSquareY = (START + rowsNb * 2) - 2; //Position Y de la dernière case de la grille, calculée en fonction du nombre de lignes et de la hauteur des cases

            }
            action = ""; // Action actuelle du joueur (explorer une case, placer un drapeau, etc...)
            direction = ""; // Réinitialise la direction à chaque itération
            positionX = Console.CursorLeft; positionY = Console.CursorTop; //Position actuelle du curseur
            ConsoleKeyInfo keyPressed = Console.ReadKey(); //Lit l'entrée de l'utilisateur (touche pressée) et la stocke
            switch (keyPressed.Key) //Gère les différentes actions de l'utilisateur
            {
                case ConsoleKey.UpArrow: direction = "haut"; break;
                case ConsoleKey.DownArrow: direction = "bas"; break;
                case ConsoleKey.LeftArrow: direction = "gauche"; break;
                case ConsoleKey.RightArrow: direction = "droite"; break;
                case ConsoleKey.Escape: Environment.Exit(0); break;
                case ConsoleKey.Enter: action = "Enter"; Console.SetCursorPosition(positionX, positionY); haveToCheck = true; break;
                case ConsoleKey.Spacebar: action = "Spacebar"; Console.SetCursorPosition(positionX, positionY); haveToCheck = true; break;
                default: action = "Nothing"; Console.SetCursorPosition(positionX, positionY); haveToCheck = true; break;


            }

            switch (direction) //Gère les déplacements du joueur dans la grille en fonction de la direction choisie
            {
                case "droite":
                    if (Console.CursorLeft == lastSquare)
                    {
                        Console.CursorLeft = firstSquare;
                        currentX = 0;
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
                        currentX = columnsNb - 1;
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
                        currentY = rowsNb - 1;
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
                        currentY = 0;
                    }
                    else
                    {
                        Console.CursorTop += 2;
                        currentY++;
                    }
                    break;
                default: break;
            }



        }// Fin Movement


        /// <summary>
        /// Met à jour l'état de la grille de jeu en fonction de l'action du joueur et gère les conséquences de cette action.
        /// </summary>
        /// <param name="positionX">Coordonnée X du positionnement du joueur sur la console.</param>
        /// <param name="positionY">Coordonnée Y du positionnement du joueur sur la console.</param>
        /// <param name="life">Les vies restantes du joueur. Diminue de 1 si le joueur explore une case contenant une mine.</param>
        /// <param name="action">L'action effectuée par le joueur (explorer une case, placer un drapeau, etc...).</param>
        /// <param name="grid">La grille de jeu représentée sous forme de tableau à deux dimensions.</param>
        /// <param name="currentX">Coordonnée X actuelle du joueur sur la grille 2D.</param>
        /// <param name="currentY">Coordonnée Y actuelle du joueur sur la grille 2D.</param>
        /// <param name="heartX">Coordonnée X de l'icône du cœur représentant la vie du joueur sur la console.</param>
        /// <param name="heartY">Coordonnée Y de l'icône du cœur représentant la vie du joueur sur la console.</param>
        static void CheckMines(int positionX, int positionY, ref int life, string action, ref int[,] grid, int currentX, int currentY, int heartX, int heartY, ref List<char> lifeT, ref int landMines, ref int nbrDrapeau)
        {
            if (grid[currentX, currentY] == 0) //Case sûre
            {
                switch (action) //Gère les différentes actions possibles sur une case sûre
                {

                    case "Enter": Console.Write("▒"); grid[currentX, currentY] = 2; break;
                    case "Spacebar": Console.Write("◄"); grid[currentX, currentY] = 5; break;
                    case "Nothing": Console.Write(" ");break;


                }
            }
            else if (grid[currentX, currentY] == 1) //Case avec une mine
            {
                switch (action) //Gère les différentes actions possibles sur une case avec une mine
                {
                    case "Enter":Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("X"); life -= 1; Console.SetCursorPosition(heartX, heartY);
                        lifeT[life] = ' ';
                        landMines--;
                        foreach (char x in lifeT)
                        {
                            Console.Write(x + " ");
                        }

                        Console.ResetColor();

                        grid[currentX, currentY] = 3;

                        Console.Beep();
                        if (life == 0) //Si le joueur n'a plus de vie, affiche l'écran de fin de jeu
                        {
                            break;
                        }
                        Console.SetCursorPosition(positionX + 1, positionY);

                        break;
                    case "Spacebar": Console.Write("◄"); nbrDrapeau++; grid[currentX, currentY] = 4; break;
                    case "Nothing": Console.Write(" "); break;
                    default: break;
                }


            }
            else if (grid[currentX, currentY] == 2)//Case déjà explorée et sûre
            {
                Console.Write("▒");

            }
            else if (grid[currentX, currentY] == 3) //Case déjà explorée et avec une mine
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("X");
                Console.ResetColor();
            }
            else if (grid[currentX, currentY] == 4) //Case avec une mine et un drapeau
            {
                switch (action) //Gère les différentes actions possibles sur une case avec une mine et un drapeau
                {
                    case "Enter":
                        Console.Write(" "); nbrDrapeau--; grid[currentX, currentY] = 1;

                        break;
                    case "Spacebar": Console.Write("◄"); break;
                    case "Nothing": Console.Write(" "); nbrDrapeau--;grid[currentX, currentY] = 1; break;
                }


            }
            else if (grid[currentX, currentY] == 5)//Case sûre avec un drapeau
            {
                switch (action) //Gère les différentes actions possibles sur une case avec une mine et un drapeau
                {
                    case "Enter":
                        Console.Write(" "); nbrDrapeau--; grid[currentX, currentY] = 0; break;
                    case "Spacebar": Console.Write("◄"); break;
                    case "Nothing": Console.Write(" "); nbrDrapeau--; grid[currentX, currentY] = 0; break;
                }

            }
                Console.CursorLeft--;
        }//Fin CheckMines

        /// <summary> 
        /// Affiche le nombre de mines restantes sur le terrain. 
        /// </summary>
        /// <param name="landMines">Le nombre de mines non explorées</param>
        /// <param name="positionX">La position X actuelle du curseur.</param>
        /// <param name="positionY">La position Y actuelle du curseur.</param>
        /// <param name="actualMinesTextX">La position X où le texte doit être affiché.</param>
        static void ShowActualPlacedMines(int landMines, int positionX, int positionY, int actualMinesTextX, int actualMinesTextY, int rowsNb, int actualTextY)
        {
            positionX = Console.CursorLeft;
            positionY = Console.CursorTop;
            Console.SetCursorPosition(actualMinesTextX, actualTextY);
            Console.WriteLine("Il y a encore " + landMines + " mines cachées sur le terrain !");
            Console.SetCursorPosition(positionX, positionY);
        }// Fin ShowActualPlacedMines

        /// <summary>
        /// Affiche l'écran de fin de jeu lorsque le joueur perd toutes ses vies et demande s'il souhaite rejouer.
        /// </summary>
        /// <param name="matchOver">Indique si la partie est terminée.</param>
        static void Looser(ref bool win)
        {
            string answer; //Réponse du joueur pour rejouer ou quitter
            Console.Clear();
            Console.Beep();
            Console.Beep();
            Console.Beep();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("C'est la fin !\r\n\r\n!! PERDU !! Désolé, toutes vos vies sont tombées à zéro");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Veux-tu rejouer (O/N) ?");
            Console.CursorLeft++;

            do
            {
                answer = Console.ReadLine().ToUpper();
                switch (answer) // Gère la réponse du joueur pour rejouer ou quitter
                {
                    case "O":
                        win = false; break;
                    case "N":
                        Console.WriteLine("Bonne journée !");
                        win = true; break;
                }
            } while (answer != "N" && answer != "O"); //vérifie que la réponse est valide, soit oui soit non.
        }//Fin Looser

        /// <summary>
        /// Affiche l'écran de fin de jeu lorsque le joueur gagne et indique le nombre de vies restantes.
        /// </summary>
        /// <param name="life">Le nombre de vies restantes du joueur à la fin du jeu.</param>
        /// <param name="matchOver">Indique si la partie est terminée.</param> 
        static void Winner(int life,ref bool win)
        {
            string answer; //Réponse du joueur pour rejouer ou quitter
            Console.Clear();
            Console.WriteLine("Félicitations ! Vous avez gagné avec " + life + " vie(s) restante(s).");
            Console.WriteLine("Veux-tu rejouer (O/N) ?");
            do
            {
                answer = Console.ReadLine().ToUpper();
                switch (answer) // Gère la réponse du joueur pour rejouer ou quitter
                {
                    case "O":
                        win = false; break;
                    case "N":
                        Console.WriteLine("Bonne journée !");
                        win = true; break;
                }
            } while (answer != "N" && answer != "O"); //vérifie que la réponse est valide, soit oui soit non.
        }//Fin Winner

    }//Fin Program

}//Fin Namespace