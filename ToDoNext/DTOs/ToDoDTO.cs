using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoNext.DTOs
{
    public class ToDoDTO:BindableBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }=false;
        public DateTime CreatedDate { get{
                    return DateTime.Now;
                    } }
        public string Color { get{
           return Status==false? "#1E90FF" : "#3CB371";
                    } }
    }
}
