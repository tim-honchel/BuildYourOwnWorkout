namespace Workouts.Entities.Dto
{
    public class Exercise
    {
        public string Name { get; set; } = string.Empty;
        public int Time { get; set; } = 0;
        public int Order { get; set; }
    }
}
