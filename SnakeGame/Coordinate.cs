using System;

namespace SnakeGame
{
    internal class Coordinate
    {
        private int x;
        private int y;

        public int X => x;
        public int Y => y;

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void ApplyMovement(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left: x--; break;
                case Direction.Right: x++; break;
                case Direction.Up: y--; break;
                case Direction.Down: y++; break;
            }
        }
        public override bool Equals(object? obj)
        {
            return obj is Coordinate other && other.x == x && other.y == y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }
    }
}
