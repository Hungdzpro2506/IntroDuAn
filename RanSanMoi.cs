using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SnakeGame
{
    class Program
    {
        static char[,] screen = new char[20, 20]; // Màn hình game
        static List<(int, int)> snake = new List<(int, int)>(); // Vị trí của rắn
        static (int, int) food; // Vị trí của mồi
        static int direction = 0; // 0: lên, 1: phải, 2: xuống, 3: trái

        static void Main(string[] args)
        {
            InitializeGame();
            while (true)
            {
                Draw();
                Input();
                Update();
                Thread.Sleep(100); // Delay để điều chỉnh tốc độ
            }
        }

        static void InitializeGame()
        {
            snake.Add((10, 10)); // Khởi tạo rắn ở vị trí (10, 10)
            GenerateFood();
        }

        static void GenerateFood()
        {
            Random random = new Random();
            do
            {
                food = (random.Next(1, 19), random.Next(1, 19)); // Vị trí ngẫu nhiên cho mồi
            } while (snake.Contains(food)); // Đảm bảo mồi không nằm trên thân rắn
        }

        static void Draw()
        {
            Console.Clear();
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (i == 0 || i == 19 || j == 0 || j == 19)
                        screen[i, j] = '#';    // Tường
                    else
                        screen[i, j] = ' ';    // Ô trống
                }
            }

            // Vẽ rắn
            foreach (var part in snake)
            {
                screen[part.Item1, part.Item2] = '*';
            }

            // Vẽ mồi
            screen[food.Item1, food.Item2] = '@';

            // Hiển thị màn hình
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    Console.Write(screen[i, j]);
                }
                Console.WriteLine();
            }
        }

        static void Input()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow && direction != 2)
                    direction = 0; // Lên
                else if (key == ConsoleKey.RightArrow && direction != 3)
                    direction = 1; // Phải
                else if (key == ConsoleKey.DownArrow && direction != 0)
                    direction = 2; // Xuống
                else if (key == ConsoleKey.LeftArrow && direction != 1)
                    direction = 3; // Trái
            }
        }

        static void Update()
        {
            (int, int) newHead = snake.First(); // Lấy đầu rắn hiện tại
            switch (direction)
            {
                case 0: newHead.Item1--; break; // Di chuyển lên
                case 1: newHead.Item2++; break; // Di chuyển phải
                case 2: newHead.Item1++; break; // Di chuyển xuống
                case 3: newHead.Item2--; break; // Di chuyển trái
            }

            // Kiểm tra va chạm với tường
            if (newHead.Item1 == 0 || newHead.Item1 == 19 || newHead.Item2 == 0 || newHead.Item2 == 19 || snake.Contains(newHead))
            {
                Console.Clear();
                Console.WriteLine("Game Over!");
                Environment.Exit(0); // Thoát trò chơi
            }

            snake.Insert(0, newHead); // Thêm đầu mới vào rắn

            // Kiểm tra rắn có ăn mồi không
            if (newHead == food)
            {
                GenerateFood(); // Tạo mồi mới
            }
            else
            {
                snake.RemoveAt(snake.Count - 1); // Xóa đuôi rắn nếu không ăn mồi
            }
        }
    }
}
