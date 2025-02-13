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
using System.Windows.Shapes;


namespace ProjektWPF
{
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }

        private void StartQuizMenuItem_Click(object sender, RoutedEventArgs e)
        {
            
            StartQuizWindow startQuizWindow = new StartQuizWindow();
            startQuizWindow.ShowDialog();
        }

        private void AddQuizMenuItem_Click(object sender, RoutedEventArgs e)
        {
         
            MainWindow addQuizWindow = new MainWindow();
            addQuizWindow.ShowDialog();
        }

        private void DeleteQuizMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DeleteQuizWindow deleteQuizWindow = new DeleteQuizWindow();

            deleteQuizWindow.ShowDialog();
        }
    }
}