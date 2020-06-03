using QuickHash.Gen;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _3iRegistry.WPF.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void QuitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pswBoxPassword.Password))
            {
                textBlockPswPlaceholder.Visibility = Visibility.Visible;
            }
            else
                textBlockPswPlaceholder.Visibility = Visibility.Collapsed;
        }
    }
}
