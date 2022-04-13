using Projekat.Logovanje.AdminLogini;
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
    /// Interaction logic for AdminLog.xaml
    /// </summary>
    public partial class AdminLog : Page
    {
        public AdminLog()
        {
            DataContext = this;
            
            InitializeComponent();
         

        }

        public SizeToContent SizeToContent { get; }
        public BAZA3 B = new BAZA3();
        private string Ime;

        public string ime
        {
            get { return Ime; }
            set { Ime = value; }
        }




        public void TrenutniAdmin(){
         
            int i;
            IEnumerable<int> TrenutniID = (from a in B.TrenutnoUlogovanis
                                           select a.Id);
            i = TrenutniID.First();
            IEnumerable<string> TrAdmin = (from a in B.Administrators
                                           where a.Id==i select a.Ime );
            ime = TrAdmin.First();
            this.WindowTitle ="Ulogovani ste kao: "+ ime;

        }


        private void Potvrda_Click(object sender, RoutedEventArgs e)
        {
            AdminZaPotvrdjivanje a1 = new AdminZaPotvrdjivanje();
            Main.Content = a1;
        }

        private void PotvrdaSkoli(object sender, RoutedEventArgs e)
        {
            PotvrdaSkoli P1 = new PotvrdaSkoli();
            Main.Content = P1;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TrenutniAdmin();

        }

        private void FormSkolskog(object sender, RoutedEventArgs e)
        {
            FormiranjeSkolskogTakmicenja P = new FormiranjeSkolskogTakmicenja();
            Main.Content = P;

        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Application.Current.Windows[0].Close(); //radi

            Application.Current.MainWindow = mw;
            Application.Current.MainWindow.Show();

        }

        private void Igraj(object sender, RoutedEventArgs e)
        {
            TakmicenjeSkolsko T1 = new TakmicenjeSkolsko();
            Main.Content = T1;

        }

        private void Raspored(object sender, RoutedEventArgs e)
        {
            RS_Skolskog R1 = new RS_Skolskog();
            Main.Content = R1;
        }

        private void FormaOkruznog(object sender, RoutedEventArgs e)
        {
            FormranjeOkruznogTakmicenja F1 = new FormranjeOkruznogTakmicenja();
            Main.Content = F1;
        }

        private void IgranjeOk(object sender, RoutedEventArgs e)
        {
            IgranjeOkruznogTakmicenja IOT = new IgranjeOkruznogTakmicenja();
            Main.Content = IOT;
        }
    }
}
