using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelProject.ViewModel
{
    public class HelloWorldViewModel : BindableBase
    {
        private string _TestProperty;
        public string TestProperty
        {
            get { return _TestProperty; }
            set { SetProperty(ref _TestProperty, value); }
        }

        public HelloWorldViewModel()
        {
            this.TestProperty = "Test";
        }
    }
}
