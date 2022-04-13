using Projekat.Logovanje.Skole;
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

namespace Projekat.Logovanje
{
    /// <summary>
    /// Interaction logic for SkolaLog.xaml
    /// </summary>
    public partial class SkolaLog : Page
    {
        public SkolaLog()
        {
            InitializeComponent();
        }

        private void FormirajTim(object sender, RoutedEventArgs e)
        {
            FormirajTim p = new FormirajTim();
            Frejm.Content = p;
        }

        private void PotvrdiUcenika(object sender, RoutedEventArgs e)
        {
            PotvrdjivanjeUcenika p = new PotvrdjivanjeUcenika();
            Frejm.Content = p;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TrenutnaSkola();
 
        }
        public void TrenutnaSkola()
        {
            BAZA3 B1 = new BAZA3();
            int i;
            string ime;
            IEnumerable<int> TrenutniID = (from a in B1.TrenutnoUlogovanis
                                           select a.Id);
            i = TrenutniID.First();
            IEnumerable<string> TrSKola = (from a in B1.Skolas
                                           where a.Id == i
                                           select a.Naziv);
            ime = TrSKola.First();
            this.WindowTitle = "Ulogovani ste kao: " + ime;

        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Application.Current.Windows[0].Close(); //radi

            Application.Current.MainWindow = mw;
            Application.Current.MainWindow.Show();

        }
    }
}
