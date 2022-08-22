namespace Blog.Models.DB
{
    public class CommentLike
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        //public ApplicationUser User { get; set; } = null!;
        public Guid CommentId { get; set; }
        public Comment Comment { get; set; } = null!;
    }
}