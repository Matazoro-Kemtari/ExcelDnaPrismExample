﻿using Microsoft.Practices.Prism.Mvvm;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MvvmProject.View.Internal
{
    /// <summary>
    /// Interaction logic for PopupView.xaml
    /// </summary>
    public partial class PopupView : UserControl, IView
    {
        public PopupView(ILogger log)
        {
            InitializeComponent();

            log.Info("PopupView initialized");
        }
    }
}
