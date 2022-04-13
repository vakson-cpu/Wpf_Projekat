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
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Page, INotifyPropertyChanged
    {
                                    
        

        public event PropertyChangedEventHandler PropertyChanged;


 

        /// <summary>
        /// AW AW AW 
        /// </summary>
        public Admin()
        {

            
            InitializeComponent();
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Regex
        //provera jmbga;
        public bool proveraJmbg()
        {
            string jmbg = Jmbg1.Text;
            Regex regex = new Regex(@"^[0-9]{10}$");
            Match match = regex.Match(jmbg);
            if (match.Success)
            {
                Jmbg1.BorderBrush = Brushes.Green;
                return true;
            }
            else
            {
                Jmbg1.BorderBrush = Brushes.Red;
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
            Regex regex = new Regex(@"^[A-Z]+[a-z]{3,}$");
            Match match = regex.Match(ime);
            if (match.Success)
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
            Regex regex = new Regex(@"^[A-Z]+[a-z]{3,}$");
            Match match = regex.Match(prezime);
            if (match.Success)
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
        //RadioBUtton
        public bool Radio()
        {
            if (R.IsChecked == true)
            {
                return true;
            }
            else
            {
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

        #endregion
        public string RADIO(){
            if (R.IsChecked == true)
                return "M";
            else return "Z";
        }


        //Dodavanje Novog Admina u Bazu
        public void dodajAdmina()
        {

            BAZA3 B = new BAZA3();
            try
            {
                Administrator A1 = new Administrator()
                {
                    Ime = Ime1.Text,
                    Prezime = Prezime1.Text,
                    Sifra = Sifra1.Password,
                    Drzava = "Srbija",
                    Pol = RADIO(),
                    JMBG = Jmbg1.Text,
                    Email = Mejl.Text
                };
                B.Administrators.Add(A1);

                B.SaveChanges();
            }
            catch(Exception )
                { MessageBox.Show("Morate uneti unikatan Jmbg ^__^ "); }
        }
        //Registracija Admina
        private void Registracija(object sender, RoutedEventArgs e)
        {
           
            
            boje();

            if (proveraImena() == true && proveraPrezime() == true && proveraSifre() == true && Mail() == true && proveraJmbg() == true)
            {

                dodajAdmina();

                MessageBox.Show("Uspesna Registracija!");
            }
            else
                MessageBox.Show("Molimo vas unesite podatke ponovo");

        }



        private void Vrati(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Application.Current.Windows[0].Close(); //radi

            Application.Current.MainWindow = mw;
            Application.Current.MainWindow.Show();

        }
    }
}
