using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace capstone.web.api.Entities
{
    public class ToDo
    {
        [Key]
        public int ToDoId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        [JsonIgnore]
        public Category? category { get; set; }

        public int PriorityId { get; set; }

        [JsonIgnore]
        public Priority? Priority { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
