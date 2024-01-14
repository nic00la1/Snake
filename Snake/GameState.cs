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

        private readonly LinkedList<Position> snakePositions = new LinkedList<Position>(); // private field to store snake positions
        private readonly Random random = new Random(); // private field to generate random numbers

        public GameState(int rows, int cols) // public constructor
        {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[rows, cols];
            Dir = Direction.Right;

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

    }
}