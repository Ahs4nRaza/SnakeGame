using System;
using System.Linq;

namespace SnakeGame
{
    internal class Game
    {
        private readonly int frameDelayMs = 100;
        private readonly Coordinate gridSize = new(50, 20);
        private readonly Random random = new();
        private Snake snake;
        private Coordinate applePos;
        private int score;

        public Game()
        {
            snake = new Snake(new Coordinate(10, 1));
            SpawnApple();
            score = 0;

            // Hide the cursor for the duration of the game
            Console.CursorVisible = false;
        }

        public void Run()
        {
            while (true)
            {
                if (IsCollision())
                {
                    ShowGameOverScreen();
                    Reset();
                    continue;
                }

                // Render game board
                Render();

                // Snake move logic
                snake.Move();

                // Check if snake ate apple
                if (snake.Head.Equals(applePos))
                {
                    snake.Grow();
                    score++;
                    SpawnApple();
                }

                // Wait for next frame and handle key input
                WaitForNextFrame();
            }
        }

        private void SpawnApple()
        {
            applePos = new Coordinate(
                random.Next(1, gridSize.X - 1),
                random.Next(1, gridSize.Y - 1)
            );
        }

        private void Render()
        {
            Console.SetCursorPosition(0, 0); // Reset cursor position to the top-left corner
            Console.WriteLine($"Score: {score}");

            for (int y = 0; y < gridSize.Y; y++)
            {
                for (int x = 0; x < gridSize.X; x++)
                {
                    Coordinate current = new(x, y);

                    if (snake.Head.Equals(current) || snake.Body.Any(segment => segment.Equals(current)))
                        Console.Write('■'); // Snake body and head
                    else if (applePos.Equals(current))
                        Console.Write('*'); // Apple
                    else if (IsWall(x, y))
                        Console.Write('#'); // Wall
                    else
                        Console.Write(' '); // Empty space
                }
                Console.WriteLine();
            }
        }

        private bool IsWall(int x, int y)
        {
            return x == 0 || y == 0 || x == gridSize.X - 1 || y == gridSize.Y - 1;
        }

        private bool IsCollision()
        {
            return IsWall(snake.Head.X, snake.Head.Y) || snake.IsOnBody(snake.Head);
        }

        private void WaitForNextFrame()
        {
            var frameStart = DateTime.Now;

            while ((DateTime.Now - frameStart).Milliseconds < frameDelayMs)
            {
                if (!Console.KeyAvailable) continue;

                var key = Console.ReadKey(true).Key;
                var newDirection = key switch
                {
                    ConsoleKey.LeftArrow => Direction.Left,
                    ConsoleKey.RightArrow => Direction.Right,
                    ConsoleKey.UpArrow => Direction.Up,
                    ConsoleKey.DownArrow => Direction.Down,
                    _ => (Direction?)null
                };

                if (newDirection.HasValue)
                    snake.ChangeDirection(newDirection.Value);
            }
        }

        private void Reset()
        {
            Console.Clear();
            snake = new Snake(new Coordinate(10, 1));
            SpawnApple();
            score = 0;
        }

        private void ShowGameOverScreen()
        {
            Console.Clear();
            Console.WriteLine("Game Over");
            Console.WriteLine($"Final Score: {score}");
            Console.WriteLine("Press any key to restart...");
            Console.ReadKey(true); // Wait for user input before restarting
        }
    }
}
