using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page,INotifyPropertyChanged
    {

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Page1()
        {
            DataContext = this;
            InitializeComponent();
        }

        public BAZA3 B1= new BAZA3();
        private ObservableCollection<string> skolice = new ObservableCollection<string>();

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Skolice
        {
            get { return skolice; }
            set { 
                skolice = value;
                OnPropertyChanged();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var Skolica = (from s in B1.Skolas
                               select s.Naziv);
            foreach (var a in Skolica)
                Skolice.Add(a);
            MessageBox.Show("Izvrseno loadovanje"); 
            
            
        }

        //provera jmbga;
        public bool proveraJmbg()
        {
            string jmbg = jmbg1.Text;
            Regex regex = new Regex(@"^[0-9]{10}$");
            Match match = regex.Match(jmbg);
            if (match.Success)
            {
                jmbg1.BorderBrush = Brushes.Green;
                return true;
            }
            else
            {
                jmbg1.BorderBrush = Brushes.Red;
                return false;
            }

        }
        //Provera istoce sifri
        public bool provera()
        {
            string a = Sifra1.Password;
            string b = Sifra2.Password;
            if (a.Equals(b) != true)
            {
                return false;
            }
            else return true;

        }

        //MEjl
        public bool Mail()
        {
            string email = Mejl.Text;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
            {
                Mejl.BorderBrush = Brushes.Green;
                return true;

            }

            else
            {
                Mejl.BorderBrush = Brushes.Red;
                return false;
            }
        }

        //Ime
        public bool proveraImena()
        {
            string ime = Ime1.Text;
  
            if (ime.Length>=8)
            {
                Ime1.BorderBrush = Brushes.Green;
                return true;
            }
            else
            {
                Ime1.BorderBrush = Brushes.Red;
                return false;
            }
        }
        //Prezime
        public bool proveraPrezime()
        {
            string prezime = Prezime1.Text;
   
            if (prezime.Length>=8)
            {
                Prezime1.BorderBrush = Brushes.Green;
                return true;
            }
            else
            {
                Prezime1.BorderBrush = Brushes.Red;
                return false;
            }
        }
        //SIfra
        public bool proveraSifre()
        {
            if (provera() == true)
            {
                string txt = (string)Sifra1.Password; //returns input 
                string txt2 = (string)Sifra2.Password;
                Regex r1 = new Regex("^[a-zA-Z0-9]{8,20}$"); //Sadrzi  slova i brojeve od 0-9
                Regex r2 = new Regex("^[a-zA-Z0-9]{8,20}$");
                if (r1.IsMatch(txt) == true && r2.IsMatch(txt2) == true)
                {
                    Sifra1.BorderBrush = Brushes.Green;
                    Sifra2.BorderBrush = Brushes.Green;
                    return true;
                }
                else
                {
                    Sifra1.BorderBrush = Brushes.Red;
                    Sifra2.BorderBrush = Brushes.Red;
                    return false;
                }
            }
            else
            {
                Sifra1.BorderBrush = Brushes.Red;
                Sifra2.BorderBrush = Brushes.Red;
                return false;
            }



        }

        //Promeni boje
        public void boje()
        {
            bool a;
            //da promene boje granice
            a = proveraImena(); a = proveraPrezime(); a = proveraSifre(); a = Mail(); a = proveraJmbg();
        }
        public string RADIO(){
            if (M.IsChecked == true)
                return "M";
            else
                return "Z";
        }
        public void DodajUcenika()
        {

            try
            {
                NepotvrdjeniUcenici N1 = new NepotvrdjeniUcenici()
                {
                    Ime = Ime1.Text,
                    Prezime = Prezime1.Text,
                    Sifra = Sifra1.Password,
                    Visina = double.Parse(Visina1.Text),
                    Godine = int.Parse(God.Text),
                    Naziv_Skole = c1.Text,
                    JMBG = jmbg1.Text,
                    pol = RADIO(),
                    Mobile = "1231231",
                    Email = Mejl.Text



                };
                B1.NepotvrdjeniUcenicis.Add(N1);

                B1.SaveChanges();
                MessageBox.Show("Ucenik JE USPESNO DODATA,Ceka se potvrda admina");

            }
            catch (Exception )
            { MessageBox.Show("Sva polja moraju biti popunjena"); }
        }

        private void Reg(object sender, RoutedEventArgs e)
        {
            boje();
            if (proveraImena() == true && proveraPrezime() == true && proveraSifre() == true && Mail() == true && proveraJmbg() == true)
                DodajUcenika();
            else MessageBox.Show("UNESITE ISPRAVNO PODATKE");
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Application.Current.Windows[0].Close(); //radi

            Application.Current.MainWindow = mw;
            Application.Current.MainWindow.Show();

        }
    }
}
