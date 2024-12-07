using System;
using System.IO;

public class Board
{
    private char[,] board;
    private const int Size = 3;

    public Board()
    {
        board = new char[Size, Size];
        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                board[i, j] = ' ';
    }

    public void Draw()
    {
        Console.Clear();
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                Console.Write(board[i, j]);
                if (j < Size - 1) Console.Write("|");
            }
            Console.WriteLine();
            if (i < Size - 1) Console.WriteLine("-----");
        }
    }

    public bool PlaceMove(int x, int y, char playerSymbol)
    {
        if (board[x, y] == ' ')
        {
            board[x, y] = playerSymbol;
            return true;
        }
        return false;
    }

    public bool CheckWin(char playerSymbol)
    {
        for (int i = 0; i < Size; i++)
            if ((board[i, 0] == playerSymbol && board[i, 1] == playerSymbol && board[i, 2] == playerSymbol) ||
                (board[0, i] == playerSymbol && board[1, i] == playerSymbol && board[2, i] == playerSymbol))
                return true;

        return (board[0, 0] == playerSymbol && board[1, 1] == playerSymbol && board[2, 2] == playerSymbol) ||
               (board[0, 2] == playerSymbol && board[1, 1] == playerSymbol && board[2, 0] == playerSymbol);
    }

    public bool IsFull()
    {
        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                if (board[i, j] == ' ') return false;
        return true;
    }
}

public abstract class Player
{
    public char Symbol { get; set; }

    protected Player(char symbol)
    {
        Symbol = symbol;
    }

    public abstract (int, int) GetMove();
}

public class HumanPlayer : Player
{
    public HumanPlayer(char symbol) : base(symbol) { }

    public override (int, int) GetMove()
    {
        int x, y;
        while (true)
        {
            Console.Write($"Player {Symbol}, enter your move (row,col): ");
            var input = Console.ReadLine().Split(',');
            if (input.Length == 2 &&
                int.TryParse(input[0], out x) &&
                int.TryParse(input[1], out y) &&
                x >= 0 && x < 3 && y >= 0 && y < 3)
            {
                return (x, y);
            }
            Console.WriteLine("Invalid input. Try again.");
        }
    }
}

public class ComputerPlayer : Player
{
    public ComputerPlayer(char symbol) : base(symbol) { }

    public override (int, int) GetMove()
    {
        Random random = new Random();
        int x, y;
        do
        {
            x = random.Next(3);
            y = random.Next(3);
        } while (HasTaken(x, y)); // Check if the cell is already taken
        return (x, y);
    }

    private bool HasTaken(int x, int y)
    {
        // Check if the cell (x,y) is already occupied
        return false; // This should be implemented properly
    }
}

public class Game
{
    private Board board;
    private Player player1;
    private Player player2;
    private Player currentPlayer;

    public Game()
    {
        board = new Board();
        player1 = new HumanPlayer('X');
        player2 = new ComputerPlayer('O');
        currentPlayer = player1;
    }

    public void Play()
    {
        while (true)
        {
            board.Draw();
            var (x, y) = currentPlayer.GetMove();

            if (board.PlaceMove(x, y, currentPlayer.Symbol))
            {
                if (board.CheckWin(currentPlayer.Symbol))
                {
                    board.Draw();
                    Console.WriteLine($"Player {currentPlayer.Symbol} wins!");
                    LogResult(currentPlayer.Symbol);
                    break;
                }

                if (board.IsFull())
                {
                    board.Draw();
                    Console.WriteLine("It's a draw!");
                    break;
                }

                currentPlayer = (currentPlayer == player1) ? player2 : player1;
            }
            else
            {
                Console.WriteLine("Cell already taken. Try again.");
            }
        }
    }

    private void LogResult(char winner)
    {
        using (StreamWriter writer = new StreamWriter("game_results.txt", true))
        {
            writer.WriteLine($"{DateTime.Now}: Player {winner} wins!");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Play();
    }
}
