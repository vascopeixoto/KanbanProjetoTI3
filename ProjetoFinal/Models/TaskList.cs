namespace ProjetoFinal.Models
{
    public class TaskList
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int EstimatedTime { get; set; }
        public Stage Stage { get; set; }
    }
}
