using System.ComponentModel.DataAnnotations;

namespace capstone.web.api.Entities
{
    public class ToDo
    {
        [Key]
        public int ToDoId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public  Category category { get; set; }

        public int PriorityId { get; set; }

        public Priority Priority { get; set; }

    }
}
