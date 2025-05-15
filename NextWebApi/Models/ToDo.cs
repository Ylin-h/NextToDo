using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextWebApi.Models
{
    [Table("ToDo")]
    public class ToDo
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
