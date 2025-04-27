
# Snake Game

## Introduction

This is a simple console-based Snake Game implemented in C#. In this game, you control a snake that moves around a grid, eating apples to grow in length and increasing your score. The game ends when the snake collides with a wall or its own body.

## Game Controls

- Use the arrow keys to control the direction of the snake:
  - **Left Arrow**: Move Left
  - **Right Arrow**: Move Right
  - **Up Arrow**: Move Up
  - **Down Arrow**: Move Down

## .env File Structure

The project supports loading environment variables from an `.env` file. Here is the structure for the `.env` file:

```
FRAME_DELAY_MS=
GRID_WIDTH=
GRID_HEIGHT=
MAX_HIGH_SCORES=
MAX_PLAYER_NAME_LENGTH=
SCORE_FILE_PATH=""
```

### Variables
- **FRAME_DELAY_MS**: Delay in milliseconds between each game frame (100 ms by default).
- **GRID_WIDTH**: The width of the game grid (50 by default).
- **GRID_HEIGHT**: The height of the game grid (20 by default).
- **MAX_HIGH_SCORES**: Maximum number of high scores to display (5 by default).
- **MAX_PLAYER_NAME_LENGTH**: The maximum length for the player's name (15 by default).
- **SCORE_FILE_PATH**: Path to the file where high scores are stored (`highscores.txt` by default).

## How to Run

1. Clone this repository to your local machine.

2. Open a terminal or command prompt and navigate to the project directory.

3. If you haven't already, make sure to install the required packages using:
   ```
   dotnet restore
   ```

4. Create an `.env` file in the root directory and configure the variables as needed. 

5. Build and run the project using:
   ```
   dotnet run
   ```

6. The game will start in the console window. Use the arrow keys to control the snake and try to get the highest score.

## High Scores

After finishing a game, your score will be saved and you can view the top 5 high scores. The scores are stored in the file defined by `SCORE_FILE_PATH`.

Enjoy the game and try to beat your high score!

