using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextWebApi.Models
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string AccountName { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
    }
}
