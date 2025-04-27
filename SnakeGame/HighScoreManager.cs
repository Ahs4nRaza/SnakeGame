namespace SnakeGame
{
    internal class HighScoreManager
    {
        private const string ScoreFileName = "Scores.txt";
        private const int MaxHighScores = 5;

        private static string GetRootDirectory()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = new DirectoryInfo(currentDirectory);

            while (directoryInfo.Parent != null)
            {
                directoryInfo = directoryInfo.Parent;
                if (File.Exists(Path.Combine(directoryInfo.FullName, "SnakeGame.csproj")))
                {
                    return directoryInfo.FullName;
                }
            }

            return directoryInfo.FullName;
        }

        private string GetFilePath()
        {
            return Path.Combine(GetRootDirectory(), ScoreFileName);
        }

        public List<(string PlayerName, int Score)> GetHighScores()
        {
            List<(string PlayerName, int Score)> scores = new();
            string filePath = GetFilePath();

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int score))
                {
                    scores.Add((parts[0], score));
                }
            }

            return scores.OrderByDescending(x => x.Score).Take(MaxHighScores).ToList();
        }

        public void AddHighScore(string playerName, int score)
        {
            string filePath = GetFilePath();
            var highScores = GetHighScores();

            highScores.Add((playerName, score));

            // Sort the scores in descending order and keep the top 5
            highScores = highScores.OrderByDescending(x => x.Score).Take(MaxHighScores).ToList();

            // Write the updated high scores back to the file
            File.WriteAllLines(filePath, highScores.Select(x => $"{x.PlayerName},{x.Score}"));
        }
    }
}
