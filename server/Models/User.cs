namespace server.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;

        //user can create multiple projects
        public ICollection<Project> Projects { get; set; } = new List<Project>();


        // ProjectMember Relation
        public ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();

         //user can create multiple tasks
        public ICollection<TaskItem> TaskItems{ get; set; } = new List<TaskItem>();


        // TaskAssignemtn Relation
        public ICollection<TaskAssignment> Assignments { get; set; } = new List<TaskAssignment>();

        //comments

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();


    } 
}
