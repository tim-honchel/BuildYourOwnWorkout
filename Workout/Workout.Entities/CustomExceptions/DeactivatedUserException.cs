namespace Workouts.Entities.CustomExceptions
{
    public class DeactivatedUserException : Exception
    {
        public DeactivatedUserException() 
        {
        }

        public DeactivatedUserException(string message) : base(message) 
        {
        }

        public DeactivatedUserException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
