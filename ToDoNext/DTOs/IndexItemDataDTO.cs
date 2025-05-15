using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoNext.DTOs
{
    public class IndexItemDataDTO
    {
        public int Total { get; set; }
        public int Completed { get; set; }
        public string FinishedRate
        {
            get
            {
                if (Total == 0)
                {
                    return "0.00%";
                }
                return (Completed * 100.00 / Total).ToString("f2") + "%";
            }
        }
       

    }
}
