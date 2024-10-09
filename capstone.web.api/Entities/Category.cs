using System.ComponentModel.DataAnnotations;

namespace capstone.web.api.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public bool IsDeleted {  get; set; }
    }
}
