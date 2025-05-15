using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoNext.DTOs
{
    public class AccRegDTO
    {
        public string AccountName { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }
}
