﻿using System.Windows;

namespace ImageViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            new Bootstrapper();

            Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
        }
    }
}
