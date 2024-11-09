namespace Workout.Entities.Database
{
    public class Workout
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public bool Active { get; set; }
        public string SettingsJson { get; set; } = "{}";
        public string ExercisesJson { get; set; } = "{}";
        public int UserId { get; set; }
    }
}
