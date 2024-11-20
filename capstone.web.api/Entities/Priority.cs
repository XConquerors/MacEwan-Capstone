using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace capstone.web.api.Entities
{
    public class Priority
    {
        [Key]
        public int PriorityId { get; set; }
        public string PriorityName { get; set; }
        public string Description { get; set; }
        public int PriorityLevel { get; set; }

        public ICollection<ToDo> Todos { get; set; }

        public bool IsDeleted { get; set; } = false;

        public Priority()
        {
            Todos = new HashSet<ToDo>();
        }
    }
}
