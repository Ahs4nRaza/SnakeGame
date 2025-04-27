using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    internal class Snake
    {
        private int Length = 1;
        private List<Coordinate> body;
        private Direction currentDirection;

        public IEnumerable<Coordinate> Body => body;

        public Snake(Coordinate startPosition)
        {
            body = new List<Coordinate> { startPosition };
            currentDirection = Direction.Down;
        }

        public Coordinate Head => body.Last();

        public void Move()
        {
            // Create a new head based on the current head's position
            Coordinate newHead = new Coordinate(Head.X, Head.Y);
            newHead.ApplyMovement(currentDirection); 

            body.Add(newHead);

            // Remove the tail if the snake is not growing
            if (body.Count > Length)
                body.RemoveAt(0);
        }

        public void Grow()
        {
            Length++; 
        }

        public void ChangeDirection(Direction newDirection)
        {
            // Prevent the snake from reversing direction
            if ((currentDirection == Direction.Left && newDirection == Direction.Right) ||
                (currentDirection == Direction.Right && newDirection == Direction.Left) ||
                (currentDirection == Direction.Up && newDirection == Direction.Down) ||
                (currentDirection == Direction.Down && newDirection == Direction.Up))
            {
                return; 
            }

            currentDirection = newDirection;
        }

        public bool IsOnBody(Coordinate coord)
        {
            return body
                    .SkipLast(1) // Skip the last segment (the head)
                    .Any(segment => segment.Equals(coord)); // Check if any segment equals the given coord
        }
    }
}
