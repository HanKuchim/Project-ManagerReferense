namespace Project_Manager.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public Task Task { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
