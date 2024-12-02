namespace Workouts.Entities.CustomExceptions
{
    public class WorkoutDoesNotExistException : Exception
    {
        public WorkoutDoesNotExistException()
        {
        }

        public WorkoutDoesNotExistException(string message) : base(message)
        {
        }

        public WorkoutDoesNotExistException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
