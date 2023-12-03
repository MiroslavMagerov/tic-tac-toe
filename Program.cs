/*
 * 
*/

using System.Reflection.Metadata.Ecma335;

namespace TicTacToe
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Constantes
            const string MsgWelcome = "Bienvenido al juego del 3 en raya. ¡Espero que disfrutes jugando!";
            const string MsgRandomPlayer = "Jugarás contra la máquina. Se decicirá al azar si comienzas tú o la máquina.";
            const string MsgAskToken = "¿Quieres ser la 'X' o la 'O'? (No es un cero).";
            const string MsgGetToken = "Escribe la ficha con la que quieres jugar escribiéndola: ";
            const string MsgUserTokenIsX = "Tu serás la 'X' y el ordenador será la 'O'.";
            const string MsgUserTokenIsO = "Tu serás la 'O' y el ordenador será la 'X'.";
            const string MsgPressAnyKey = "Presiona cualquier tecla para continuar... ";
            const string MsgFirstComputerPlayer = "Te ha tocado ser el segundo jugador.";
            const string MsgFirstUserPlayer = "Te ha tocado ser el primer jugador.";
            const string MsgExplanationBoard = "Los asteriscos ('*') que ves en el tablero son las posiciones vacías donde se colocarán las fichas de cada jugador.";
            const string MsgUserElection = "Elige una posición del tablero (el tablero de la derecha indica la posición para cada número): ";
            const string MsgWrongLetter = "Te has equivocado al escribir la ficha. Vuelve a intentarlo: ";
            const string MsgWrongPosition = "Te has equivocado de posición. Vuelve a intentarlo: ";
            const int boardRows = 3, boardColumns = 3;

            // Variables
            string userToken, computerToken;

            Random rnd = new Random();
            
            // Decidir el primer jugador al azar
            bool userFirstPlayer = rnd.Next(0,2) == 0 ? true : false, secondExecution = false;

            string[,] gameBoard = new string[boardRows, boardColumns] 
            {
                {"*", "*", "*"},
                {"*", "*", "*"},
                {"*", "*", "*"},
            };

            int[,] helpBoard =
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9},
            };
            int userPosition, remainingSpaces = 9;

            // Bienvenida al juego
            Console.WriteLine(MsgWelcome);
            Console.WriteLine(MsgRandomPlayer);

            // Elegir que ficha quiere ser el jugador
            do
            {   
                if (secondExecution)
                {
                    Console.WriteLine(MsgWrongLetter);
                }

                Console.WriteLine(MsgAskToken);
                Console.Write(MsgGetToken);
                userToken = Console.ReadLine().ToUpper();

                secondExecution = true;

            } while (!UserTokenVerification(userToken));

            secondExecution = false;

            if (userToken == "X")
            {
                computerToken = "O";
                Console.WriteLine(MsgUserTokenIsX);
            }
            else
            {
                computerToken = "X";
                Console.WriteLine(MsgUserTokenIsO);
            }

            // Indicar quien es el primer jugador
            if (userFirstPlayer)
            {
                Console.WriteLine(MsgFirstUserPlayer);
            }
            else
            {
                Console.WriteLine(MsgFirstComputerPlayer);
            }

            Console.Write(MsgPressAnyKey);
            Console.ReadKey();
            Console.Clear();

            // Explicar como funciona el juego
            Console.WriteLine(MsgExplanationBoard);
            Console.WriteLine();

            // Bucle principal del juego
            do
            {
                ShowBoards(gameBoard, helpBoard);
                Console.WriteLine();
                secondExecution = false;

                do
                {
                    if (secondExecution)
                    {
                        Console.WriteLine(MsgWrongPosition);
                    }

                    Console.Write(MsgUserElection);
                    userPosition = Convert.ToInt16(Console.ReadLine());

                    secondExecution = true;

                } while (CheckForUpdateBoard(ref gameBoard, userPosition, userToken, computerToken));

                UpdateBoard(ref gameBoard, userPosition, userToken, computerToken);

                remainingSpaces--;

            } while ((CheckVictoryUser(gameBoard, userToken) || CheckVictoryComputer(gameBoard, computerToken)) && remainingSpaces > 0);
        }

        public static bool UserTokenVerification(string token)
        {
            if (token == "X" || token == "O")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ShowBoards(string[,] boardGame, int[,] infoBoard)
        {
            for (int i = 0; i < boardGame.GetLength(0); i++)
            {
                // Mostrar tablero de juego
                for (int j = 0; j < boardGame.GetLength(1); j++)
                {
                    Console.Write(boardGame[i, j]);

                    if (j < boardGame.GetLength(1) - 1)
                    {
                        Console.Write(" | ");
                    }
                }

                Console.Write("\t\t");

                // Mostrar tablero de ayuda
                for (int j = 0; j < infoBoard.GetLength(1); j++)
                {
                    Console.Write(infoBoard[i, j]);

                    if (j < infoBoard.GetLength(1) - 1)
                    {
                        Console.Write(" | ");
                    }
                }

                if (i < boardGame.GetLength(0) - 1)
                {
                    Console.WriteLine("\n- + - + - \t\t- + - + -");
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }

        public static bool CheckForUpdateBoard(ref string[,] gameBoard, int userElection, string userToken, string computerToken)
        {
            int row = (userElection - 1) / 3;
            int col = (userElection - 1) % 3;

            if (gameBoard[row, col] != userToken || gameBoard[row, col] != computerToken)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void UpdateBoard(ref string[,] gameBoard, int userElection, string userToken, string computerToken)
        {
            int row = (userElection - 1) / 3;
            int col = (userElection - 1) % 3;

            gameBoard[row, col] = userToken;
        }

        public static bool CheckVictoryUser(string[,] board, string userToken)
        {
            for (int i = 0; i < 3; i++)
            {
                // Verificar filas
                if (board[i, 0] == userToken && board[i, 1] == userToken && board[i, 2] == userToken)
                    return false;

                // Verificar columnas
                if (board[0, i] == userToken && board[1, i] == userToken && board[2, i] == userToken)
                    return false;
            }

            // Verificar diagonales
            if ((board[0, 0] == userToken && board[1, 1] == userToken && board[2, 2] == userToken) ||
                (board[0, 2] == userToken && board[1, 1] == userToken && board[2, 0] == userToken))
            {
                return false;
            }

            // No hay victoria
            return true;
        }

        public static bool CheckVictoryComputer(string[,] board, string computerToken)
        {
            for (int i = 0; i < 3; i++)
            {
                // Verificar filas
                if (board[i, 0] == computerToken && board[i, 1] == computerToken && board[i, 0] == computerToken)
                    return false;

                // Verificar columnas
                if (board[0, i] == computerToken && board[1, i] == computerToken && board[0, i] == computerToken)
                    return false;
            }

            // Verificar diagonales
            if ((board[0, 0] == computerToken && board[1, 1] == computerToken && board[0, 0] == computerToken) ||
                (board[0, 2] == computerToken && board[1, 1] == computerToken && board[0, 2] == computerToken))
            {
                return false;
            }

            // No hay victoria
            return true;
        }
    }
}