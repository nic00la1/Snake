﻿namespace Snake
{
    public class GameState
    {
        public int Rows { get; }
        public int Cols { get; }
        public GridValue[,] Grid { get; } // 2D array
        public Direction Dir { get; private set; } // public property with private setter to prevent modification from outside
        public int Score { get; private set; }
        public bool GameOver { get; private set; }

        private readonly LinkedList<Direction> dirChanges = new LinkedList<Direction>();
        private readonly LinkedList<Position> snakePositions = new LinkedList<Position>(); // private field to store snake positions
        private readonly Random random = new Random(); // private field to generate random numbers

        public GameState(int rows, int cols) // public constructor
        {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[rows, cols];
            Dir = Direction.Right;

            AddSnake();
            AddFood();
        }

        private void AddSnake()
        {
            int r = Rows / 2; // get middle row

            for (int c = 1; c <= 3; c++) // loop 3 times
            {
                Grid[r, c] = GridValue.Snake; // set snake value in grid
                snakePositions.AddFirst(new Position(r, c)); // add position to linked list
            }
        }

        private IEnumerable<Position> EmptyPositions()
        {
            for (int r = 0; r < Rows; r++) // loop through rows
            {
                for (int c = 0; c < Cols; c++) // loop through columns
                {
                    if (Grid[r, c] == GridValue.Empty) // if grid value is empty
                    {
                        yield return new Position(r, c); // return position
                    }
                }
            }
        }

        private void AddFood()
        {
            List<Position> empty = new List<Position>(EmptyPositions()); // get empty positions

            if (empty.Count == 0) // if no empty positions
            {
                return;
            }

            Position pos = empty[random.Next(empty.Count)]; // get random position
            Grid[pos.Row, pos.Col] = GridValue.Food; // set food value in grid
        }

        public Position HeadPosition()
        {
            return snakePositions.First.Value; // return first position in linked list
        }

        public Position TailPosition()
        {
            return snakePositions.Last.Value; // return last position in linked list
        }

        public IEnumerable<Position> SnakePositions()
        {
            return snakePositions; // return linked list
        }

        private void AddHead(Position pos)
        {
            snakePositions.AddFirst(pos); // add position to linked list
            Grid[pos.Row, pos.Col] = GridValue.Snake; // set snake value in grid
        }

        private void RemoveTail()
        {
            Position tail = snakePositions.Last.Value; // get last position in linked list
            Grid[tail.Row, tail.Col] = GridValue.Empty; // set empty value in grid
            snakePositions.RemoveLast(); // remove last position from linked list
        }

        private Direction GetLastDirection()
        {
            if (dirChanges.Count == 0) // if no direction changes
            {
                return Dir; // return current direction
            }

            return dirChanges.Last.Value; // return last direction change
        }

        private bool CanChangeDirection(Direction newDir)
        {
            if (dirChanges.Count == 2)
            {
                return false; // return false if 2 direction changes
            }

            Direction lastDir = GetLastDirection(); // get last direction
            return newDir != lastDir && newDir != lastDir.Opposite(); // return true if new direction is not last direction and not opposite of last direction
        }

        public void ChangeDirection(Direction dir)
        {
            if (CanChangeDirection(dir))
            {
                dirChanges.AddLast(dir);

            }
        }

        private bool OutsideGrid(Position pos)
        {
            return pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Cols; // return true if position is outside grid
        }

        private GridValue WillHit(Position newHeadPos)
        {
            if (OutsideGrid(newHeadPos)) // if outside grid
            {
                return GridValue.Outside; // return outside value
            }

            if (newHeadPos == TailPosition()) // if new head position is tail position
            {
                return GridValue.Empty; // return snake value
            }

            return Grid[newHeadPos.Row, newHeadPos.Col]; // return grid value at position
        }

        public void Move()
        {
            if (dirChanges.Count > 0) // if direction changes
            {
                Dir = dirChanges.First.Value; // set direction to first direction change
                dirChanges.RemoveFirst(); // remove first direction change
            }

            Position newHeadPos = HeadPosition().Translate(Dir); // get new head position
            GridValue hit = WillHit(newHeadPos); // get grid value at new head position

            Console.WriteLine($"New Head Position: ({newHeadPos.Row}, {newHeadPos.Col}), Hit: {hit}");

            if (hit == GridValue.Outside || hit == GridValue.Snake) // PRZEZ TEN BLAD SIEDZIALAM 2 GODZINY (hit == GridValue.Empty) musi byc zamiast (hit != GridValue.Empty)
            {
                GameOver = true; // set game over to true
                return;
            }
            else if (hit == GridValue.Empty)
            {
                RemoveTail();
                AddHead(newHeadPos);
            }
            else if (hit == GridValue.Food)
            {
                AddHead(newHeadPos);
                Score++;
                AddFood();
            }
        }
    }
}
