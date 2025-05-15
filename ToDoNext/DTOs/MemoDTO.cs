using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoNext.DTOs
{
    public class MemoDTO:BindableBase
    {
        private int _Id;

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private string _Title;
        public string Title { get { return _Title; } set { SetProperty(ref _Title, value); } }
        private string _Content;
        public string Content { get { return _Content; } set { SetProperty(ref _Content, value); } }
        
    }
}
