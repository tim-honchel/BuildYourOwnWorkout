namespace Workouts.Entities.Database
{
    public class Workout
    {
        public long Id { get; set; }
        public string Title { get; set; } = "";
        public bool Active { get; set; }
        public string SettingsJson { get; set; } = "{}";
        public string ExercisesJson { get; set; } = "{}";
        public long UserId { get; set; }
    }
}
