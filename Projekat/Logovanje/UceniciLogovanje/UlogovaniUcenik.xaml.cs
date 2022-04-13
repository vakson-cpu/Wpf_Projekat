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

namespace Projekat.Logovanje.UceniciLogovanje
{
    /// <summary>
    /// Interaction logic for UlogovaniUcenik.xaml
    /// </summary>
    public partial class UlogovaniUcenik : Page
    {
        public UlogovaniUcenik()
        {
            InitializeComponent();
        }
        private void PregledTimova(object sender, RoutedEventArgs e)
        {
            PregledTIma P1 = new PregledTIma();
            Frejm.Content = P1;
        }

        private void PregledFormacije(object sender, RoutedEventArgs e)
        {
            Formacije F1 = new Formacije();
            Frejm.Content = F1;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Application.Current.Windows[0].Close(); //radi

            Application.Current.MainWindow = mw;
            Application.Current.MainWindow.Show();


        }
    }
}
