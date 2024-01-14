
namespace Snake
{
    public class Direction
    {
        public readonly static Direction Left = new Direction(0, -1);
        public readonly static Direction Right = new Direction(0, 1);
        public readonly static Direction Up = new Direction(-1, 0);
        public readonly static Direction Down = new Direction(1, 0);

        public int RowOffset { get; }
        public int ColOffset { get; }

        private Direction(int rowOffset, int colOffset) // private constructor
        {
            RowOffset = rowOffset;
            ColOffset = colOffset;
        }

        public Direction Opposite() // public method to return opposite direction
        {
            return new Direction(-RowOffset, -ColOffset);
        }

        public override bool Equals(object obj)
        {
            return obj is Direction direction &&
                   RowOffset == direction.RowOffset &&
                   ColOffset == direction.ColOffset;
        }

        public override int GetHashCode() // override GetHashCode() to make compiler happy
        {
            return HashCode.Combine(RowOffset, ColOffset); // HashCode.Combine() is a C# 7.0 feature
        }

        public static bool operator ==(Direction left, Direction right) // overload == operator to make compiler happy
        {
            return EqualityComparer<Direction>.Default.Equals(left, right); // EqualityComparer<T>.Default is a C# 7.0 feature
        }

        public static bool operator !=(Direction left, Direction right) // overload != operator to make compiler happy
        {
            return !(left == right); // use == operator
        }
    }
}
