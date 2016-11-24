using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelProject.ViewModel
{
    public class SecondHelloWorldViewModel : BindableBase
    {
        private string _SecondTestProperty;
        public string SecondTestProperty
        {
            get { return _SecondTestProperty; }
            set { SetProperty(ref _SecondTestProperty, value); }
        }

        public SecondHelloWorldViewModel()
        {
            this.SecondTestProperty = "SecondTest";
        }
    }
}
