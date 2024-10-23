using System;
using System.ComponentModel.DataAnnotations;

namespace capstone.web.api.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }


        public Boolean IsDeleted { get; set; } = false;


        public DateTime DateCreated { get; set; }

        public bool IsDelated { get; set; }

        public ICollection<ToDo> Todos { get;  set; }

        public Category()
        {
            Todos = new HashSet<ToDo>();
        }

    }
}

