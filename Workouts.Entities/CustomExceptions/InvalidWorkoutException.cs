namespace Workouts.Entities.CustomExceptions
{
    public class InvalidWorkoutException : Exception
    {
        public InvalidWorkoutException()
        {
        }

        public InvalidWorkoutException(string message) : base(message)
        {
        }

        public InvalidWorkoutException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
