
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
            const string MsgUserWon = "¡Has ganado!";
            const string MsgComputerWon = "Has perdido.";
            const string MsgNobodyWon = "Has empatado contra el ordenador";
            const string EmptyBoardSpace = " ";
            const int boardRows = 3, boardColumns = 3;

            // Variables
            string userToken, computerToken;

            Random rnd = new();

            // Decidir el primer jugador al azar
            bool userFirstPlayer = rnd.Next(0, 2) == 0 ? true : false, secondExecution = false;

            string[,] gameBoard = new string[boardRows, boardColumns]
            {
                {EmptyBoardSpace, EmptyBoardSpace, EmptyBoardSpace},
                {EmptyBoardSpace, EmptyBoardSpace, EmptyBoardSpace},
                {EmptyBoardSpace, EmptyBoardSpace, EmptyBoardSpace},
            };

            int[,] helpBoard =
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9},
            };

            int[] winnerPositions = new int[3];

            int userPosition, remainingSpaces = boardRows * boardColumns;

            // Bienvenida al juego
            Console.WriteLine(MsgWelcome);
            Console.WriteLine(MsgRandomPlayer);
            Console.WriteLine();

            // Elegir que ficha quiere ser el jugador
            do
            {
                if (secondExecution)
                {
                    Console.WriteLine(MsgWrongLetter);
                }

                Console.WriteLine(MsgAskToken);
                Console.WriteLine();
                Console.Write(MsgGetToken);
                userToken = Console.ReadLine() ?? " ";
                userToken = userToken.ToUpper();

                secondExecution = true;

            } while (!UserTokenVerification(userToken));

            Console.WriteLine();

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

            // En caso de que el jugador sea segundo, el ordenador juega el primer turno.
            if (!userFirstPlayer)
            {
                ComputerTurn(ref gameBoard, computerToken, rnd, EmptyBoardSpace);
                ShowBoards(gameBoard, helpBoard);
            }

            // Bucle principal del juego
            do
            {
                if (userFirstPlayer)
                {
                    ShowBoards(gameBoard, helpBoard);
                    Console.WriteLine();
                }
                userFirstPlayer = true;
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

                } while (CheckForUpdateBoard(ref gameBoard, userPosition, userToken, computerToken, EmptyBoardSpace));

                UpdateBoard(ref gameBoard, userPosition, userToken, computerToken);

                remainingSpaces--;

                if (remainingSpaces > 0 && (CheckVictoryUser(gameBoard, userToken, ref winnerPositions)))
                {
                    ComputerTurn(ref gameBoard, computerToken, rnd, EmptyBoardSpace);
                }

            } while ((CheckVictoryUser(gameBoard, userToken, ref winnerPositions) &&
            CheckVictoryComputer(gameBoard, computerToken, ref winnerPositions)) && remainingSpaces > 0);

            // Ha ganado el jugador o el ordenador, o bien ha habido empate y no hay más espacio para colocar fichas
            Console.Clear();

            Console.WriteLine(winnerPositions[0]);
            Console.WriteLine(winnerPositions[1]);
            Console.WriteLine(winnerPositions[2]);

            ShowBoardColorizedWinner(gameBoard, !CheckVictoryUser(gameBoard, userToken, ref winnerPositions), winnerPositions);

            if (!CheckVictoryUser(gameBoard, userToken, ref winnerPositions))
            {
                Console.WriteLine(MsgUserWon);
            }
            else if (!CheckVictoryComputer(gameBoard, computerToken, ref winnerPositions))
            {
                Console.WriteLine(MsgComputerWon);
            }
            else
            {
                Console.WriteLine(MsgNobodyWon);
            }
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

        public static void ShowBoard(string[,] boardGame)
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

                if (i < boardGame.GetLength(0) - 1)
                {
                    Console.WriteLine("\n- + - + -");
                }
                else
                {
                    Console.WriteLine();
                }
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

        public static bool CheckForUpdateBoard(ref string[,] gameBoard, int userElection, string userToken, string computerToken, string emptySpace)
        {
            int row = (userElection - 1) / gameBoard.GetLength(1);
            int col = (userElection - 1) % gameBoard.GetLength(1);

            if (gameBoard[row, col] == emptySpace)
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
            int row = (userElection - 1) / gameBoard.GetLength(1);
            int col = (userElection - 1) % gameBoard.GetLength(1);

            gameBoard[row, col] = userToken;
        }

        public static int[] UpdateAvailablePositions(string[,] gameBoard, string emptySpace)
        {
            int freePositions = 0;

            for (int i = 0; i < gameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < gameBoard.GetLength(1); j++)
                {
                    if (gameBoard[i, j] == emptySpace)
                    {
                        freePositions++;
                    }
                }
            }

            int[] availablePositions = new int[freePositions];
            int currentIndex = 0;

            for (int i = 0; i < gameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < gameBoard.GetLength(1); j++)
                {
                    if (gameBoard[i, j] == emptySpace)
                    {
                        int position = i * gameBoard.GetLength(1) + j;
                        availablePositions[currentIndex] = position;
                        currentIndex++;
                    }
                }
            }

            return availablePositions;
        }

        public static void ComputerTurn(ref string[,] boardGame, string computerToken, Random rnd, string emptySpace)
        {
            int[] avPositions = UpdateAvailablePositions(boardGame, emptySpace);

            if (avPositions.Length > 0)
            {
                int computerPositionIndex = rnd.Next(0, avPositions.Length);
                int computerPosition = avPositions[computerPositionIndex];

                int row = avPositions[computerPositionIndex] / boardGame.GetLength(1);
                int col = avPositions[computerPositionIndex] % boardGame.GetLength(1);

                boardGame[row, col] = computerToken;
            }
        }

        public static bool CheckVictoryUser(string[,] board, string userToken, ref int[] victoryPositions)
        {
            // Verificar diagonales
            if (board[0, 0] == userToken && board[1, 1] == userToken && board[2, 2] == userToken)
            {
                victoryPositions[0] = 1;
                victoryPositions[1] = 5;
                victoryPositions[2] = 9;
                return false;
            }

            else if (board[0, 2] == userToken && board[1, 1] == userToken && board[2, 0] == userToken)
            {
                victoryPositions[0] = 3;
                victoryPositions[1] = 5;
                victoryPositions[2] = 7;
                return false;
            }

            for (int i = 0; i < board.GetLength(0); i++)
            {
                // Verificar filas
                if (board[i, 0] == userToken && board[i, 1] == userToken && board[i, 2] == userToken)
                {
                    victoryPositions[0] = i * 3 + 1;
                    victoryPositions[1] = i * 3 + 2;
                    victoryPositions[2] = i * 3 + 3;
                    return false;
                }

                // Verificar columnas
                else if (board[0, i] == userToken && board[1, i] == userToken && board[2, i] == userToken)
                {
                    victoryPositions[0] = i + 1;
                    victoryPositions[1] = i + 4;
                    victoryPositions[2] = i + 7;
                    return false;
                }
            }

            // No hay victoria
            return true;
        }

        public static bool CheckVictoryComputer(string[,] board, string computerToken, ref int[] victoryPositions)
        {
            // Verificar diagonales
            if (board[0, 0] == computerToken && board[1, 1] == computerToken && board[2, 2] == computerToken)
            {
                victoryPositions[0] = 1;
                victoryPositions[1] = 5;
                victoryPositions[2] = 9;
                return false;
            }

            else if (board[0, 2] == computerToken && board[1, 1] == computerToken && board[2, 0] == computerToken)
            {
                victoryPositions[0] = 3;
                victoryPositions[1] = 5;
                victoryPositions[2] = 7;
                return false;
            }

            for (int i = 0; i < board.GetLength(0); i++)
            {
                // Verificar filas
                if (board[i, 0] == computerToken && board[i, 1] == computerToken && board[i, 2] == computerToken)
                {
                    victoryPositions[0] = i * 3 + 1;
                    victoryPositions[1] = i * 3 + 2;
                    victoryPositions[2] = i * 3 + 3;
                    return false;
                }

                // Verificar columnas
                else if (board[0, i] == computerToken && board[1, i] == computerToken && board[2, i] == computerToken)
                {
                    victoryPositions[0] = i * 3 + 1;
                    victoryPositions[1] = i * 3 + 4;
                    victoryPositions[2] = i * 3 + 7;
                    return false;
                }
            }

            // No hay victoria
            return true;
        }

        public static void ShowBoardColorizedWinner(string[,] boardGame, bool userVictory, int[] victoryPositions)
        {
            for (int i = 0; i < boardGame.GetLength(0); i++)
            {
                // Mostrar tablero de juego
                for (int j = 0; j < boardGame.GetLength(1); j++)
                {
                    int position = victoryPositions[i] - 1;
                    int row = position / 3;
                    int col = position % 3;

                    if (col == i && row == j)
                    {
                        if (userVictory)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                    }
                    Console.Write(boardGame[i, j]);

                    Console.ResetColor();

                    if (j < boardGame.GetLength(1) - 1)
                    {
                        Console.Write(" | ");
                    }
                }

                if (i < boardGame.GetLength(0) - 1)
                {
                    Console.WriteLine("\n- + - + -");
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }
    }
}