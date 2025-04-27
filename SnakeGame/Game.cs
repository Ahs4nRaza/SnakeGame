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
        private string playerName;
        private HighScoreManager highScoreManager;
        private bool gameHasEnded = false;

        public Game()
        {
            highScoreManager = new HighScoreManager();
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
                Console.Clear();
                ShowMainMenu();

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                        StartGame();
                        break;
                    case ConsoleKey.D2:
                        ViewHighScores();
                        break;
                    case ConsoleKey.D3:
                        return; // Exit the game
                }
            }
        }

        private void ShowMainMenu()
        {
            Console.WriteLine("Snake Game");
            Console.WriteLine("1. Start Game");
            Console.WriteLine("2. View High Scores");
            Console.WriteLine("3. Quit");
            Console.Write("Select an option: ");
        }

        private void StartGame()
        {
            Console.Clear();

            // If playerName is empty (it's the first time), ask for the name
            if (string.IsNullOrEmpty(playerName))
            {
                while (true)
                {
                    Console.Write("Enter your name (max 15 characters): ");
                    playerName = Console.ReadLine()?.Trim() ?? string.Empty;

                    if (string.IsNullOrEmpty(playerName))
                    {
                        Console.WriteLine("Name cannot be empty. Please enter a valid name.");
                        continue;
                    }

                    if (playerName.Length > 15)
                    {
                        Console.WriteLine("Name cannot exceed 15 characters. Please enter a shorter name.");
                        continue;
                    }

                    break;
                }
            }


            Console.Clear();

            // Countdown before starting the game
            Console.WriteLine("Get ready!");
            for (int i = 3; i > 0; i--)
            {
                Console.WriteLine(i);
                System.Threading.Thread.Sleep(1000);
            }
            Console.Clear();

            // Start the game loop
            while (true)
            {
                if (IsCollision())
                {
                    highScoreManager.AddHighScore(playerName, score);
                    gameHasEnded = true;
                    ShowGameOverScreen();
                    break;
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

        private void ShowGameOverScreen()
        {
            Console.Clear();
            Console.WriteLine("Game Over");
            Console.WriteLine($"Final Score: {score}");
            Console.WriteLine("1. Restart");
            Console.WriteLine("2. View High Scores");
            Console.WriteLine("3. Quit");
            Console.Write("Select an option: ");

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.D1:
                    Reset();
                    StartGame();
                    break;
                case ConsoleKey.D2:
                    ViewHighScores();
                    break;
                case ConsoleKey.D3:
                    Console.Clear();
                    Environment.Exit(0); // Exit the game
                    break; 
            }
        }

        private void ViewHighScores()
        {
            Console.Clear();
            Console.WriteLine("High Scores");
            var highScores = highScoreManager.GetHighScores();
            int playerNameColumnWidth = 20;

            Console.WriteLine($"{"User".PadRight(playerNameColumnWidth)}Score");

            foreach (var score in highScores)
            {
                // Format the player name and score to align properly
                Console.WriteLine($"{score.PlayerName.PadRight(playerNameColumnWidth)}{score.Score}");
            }


            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey(true);

            if (gameHasEnded)
            {
                ShowGameOverScreen();
                return;
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
    }
}
