namespace ProjetoFinal.Models
{
    public class Task
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int EstimatedTime { get; set; }
        public DateTime DateCreated { get; set; }
        public User User { get; set; }
        public Stage Stage { get; set; }
    }
}
